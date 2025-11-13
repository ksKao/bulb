using Bulb.Enums;

namespace Bulb.Node;

public class BinaryExpression(Token operatorToken, Expression left, Expression right) : Expression
{
    private Token OperatorToken { get; } = operatorToken;
    private Expression Left { get; } = left;
    private Expression Right { get; } = right;

    private bool IsBooleanOperator => OperatorToken.Type is TokenType.And or TokenType.Or;

    private bool IsMathOperator =>
        OperatorToken.Type is TokenType.Plus or TokenType.Minus or TokenType.Multiply or TokenType.Divide;

    public override DataType DataType { get; protected set; }

    public override void Run(Runner runner)
    {
        Left.Run(runner);
        Right.Run(runner);

        ValidateType();

        if (IsMathOperator)
        {
            double rightValue = (double)runner.Stack.Pop();
            double leftValue = (double)runner.Stack.Pop();

            DataType = DataType.Number;

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
        else if (IsBooleanOperator)
        {
            bool rightValue = (bool)runner.Stack.Pop();
            bool leftValue = (bool)runner.Stack.Pop();

            DataType = DataType.Boolean;

            switch (OperatorToken.Type)
            {
                case TokenType.And:
                    runner.Stack.Add(leftValue && rightValue);
                    break;
                case TokenType.Or:
                    runner.Stack.Add(leftValue || rightValue);
                    break;
                default:
                    throw new InvalidSyntaxException(
                        $"Unexpected operator encountered in binary expression: `{OperatorToken.Value}`",
                        OperatorToken.LineNumber);
            }
        }
        else
        {
            throw new InvalidSyntaxException($"Unknown binary operator encountered (`{OperatorToken.Value}`)",
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
            throw new InvalidSyntaxException($"Unable to `{OperatorToken.Value}` {Left.DataType} and {Right.DataType}",
                OperatorToken.LineNumber);
        }

        // from here on assume and left and right is the same, so we only check for the left data type
        if (Left.DataType == DataType.Number && !IsMathOperator)
        {
            throw new InvalidSyntaxException($"Unable to `{OperatorToken.Value}` {Left.DataType} and {Right.DataType}",
                OperatorToken.LineNumber);
        }

        if (Left.DataType == DataType.Boolean && !IsBooleanOperator)
        {
            throw new InvalidSyntaxException($"Unable to `{OperatorToken.Value}` {Left.DataType} and {Right.DataType}",
                OperatorToken.LineNumber);
        }
    }

    private void AssignDataType()
    {
        DataType = DataType.Number;
    }
}