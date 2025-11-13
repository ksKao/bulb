namespace Bulb.Node;

public class BinaryExpression(Token operatorToken, Expression left, Expression right) : Expression
{
    private Token OperatorToken { get; } = operatorToken;
    private Expression Left { get; } = left;
    private Expression Right { get; } = right;

    public override DataType DataType { get; protected set; }

    public override void Run(Runner runner)
    {
        Left.Run(runner);
        Right.Run(runner);

        ValidateType();
        AssignDataType();

        double rightValue = (double)runner.Stack.Pop();
        double leftValue = (double)runner.Stack.Pop();

        switch (OperatorToken.Type)
        {
            case TokenType.Plus:
                runner.Stack.Add(leftValue + rightValue);
                break;
            case TokenType.Minus:
                runner.Stack.Add(leftValue - rightValue);
                break;
            case TokenType.Multiply:
                runner.Stack.Add(leftValue * rightValue);
                break;
            case TokenType.Divide:
                runner.Stack.Add(leftValue / rightValue);
                break;
            default:
                throw new InvalidSyntaxException(
                    $"Unexpected operator encountered in binary expression: `{OperatorToken.Value}`",
                    OperatorToken.LineNumber);
        }
    }

    public override string ToString(string indent)
    {
        return $"""
                {indent} Binary Expression:
                {indent + "\t"} Operator: {OperatorToken.Value}
                {indent + "\t"} Left:
                {Left.ToString(indent + "\t\t")}
                {indent + "\t"} Right:
                {Right.ToString(indent + "\t\t")}
                """;
    }

    private void ValidateType()
    {
        if (Left.DataType != Right.DataType)
        {
            throw new InvalidSyntaxException($"Unable to `{operatorToken.Value}` {Left.DataType} and {Right.DataType}",
                operatorToken.LineNumber);
        }
    }

    private void AssignDataType()
    {
        DataType = DataType.Number;
    }
}