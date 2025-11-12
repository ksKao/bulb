namespace Bulb.Node;

public class BinaryExpression(Token operatorToken, Node left, Node right) : Node
{
    private Token OperatorToken { get; } = operatorToken;
    private Node Left { get; } = left;
    private Node Right { get; } = right;

    public override void Run()
    {
        Left.Run();
        Right.Run();

        double rightValue = (double)Runner.Stack.Pop();
        double leftValue = (double)Runner.Stack.Pop();

        switch (OperatorToken.Type)
        {
            case TokenType.Plus:
                Runner.Stack.Push(leftValue + rightValue);
                break;
            case TokenType.Minus:
                Runner.Stack.Push(leftValue - rightValue);
                break;
            case TokenType.Multiply:
                Runner.Stack.Push(leftValue * rightValue);
                break;
            case TokenType.Divide:
                Runner.Stack.Push(leftValue / rightValue);
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
}