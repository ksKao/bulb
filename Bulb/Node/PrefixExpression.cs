using Bulb.Enums;
using Bulb.Exceptions;

namespace Bulb.Node;

public class PrefixExpression(Token operatorToken, Expression value) : Expression
{
    private Token OperatorToken { get; } = operatorToken;
    private Expression Value { get; } = value;

    public override DataType DataType { get; protected set; }

    public override void Run(Runner runner)
    {
        Value.Run(runner);

        if (Value.DataType != DataType.Boolean)
        {
            throw new InvalidSyntaxException($"Unable to `{OperatorToken.Value}` {Value.DataType}",
                OperatorToken.LineNumber);
        }

        switch (OperatorToken.Type)
        {
            case TokenType.Not:
                DataType = DataType.Boolean;
                bool value = (bool)runner.Stack.Pop();
                runner.Stack.Add(!value);
                break;
            default:
                throw new InvalidSyntaxException($"Unknown prefix operator encountered (`{OperatorToken.Value}`)",
                    OperatorToken.LineNumber);
        }
    }

    public override string ToString(string indent)
    {
        return $"""
                {indent} Prefix Expression:
                {indent + "\t"} Operator: {OperatorToken.Value}
                {indent + "\t"} Value:
                {Value.ToString(indent + "\t\t")}
                """;
    }
}