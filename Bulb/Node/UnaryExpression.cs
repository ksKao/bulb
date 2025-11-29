using Bulb.DataType;
using Bulb.Enums;
using Bulb.Exceptions;

namespace Bulb.Node;

public class UnaryExpression(Token operatorToken, Expression value) : Expression
{
    private Token OperatorToken { get; } = operatorToken;
    private Expression Value { get; } = value;

    public override BaseDataType? DataType { get; protected set; }

    public override void Run(Runner runner)
    {
        Value.Run(runner);

        if (Value.DataType is null)
        {
            throw new InvalidSyntaxException("Unexpected null type in unary expression.", OperatorToken.LineNumber);
        }

        switch (OperatorToken.Type)
        {
            case TokenType.Not:
                {
                    if (Value.DataType != BaseDataType.Boolean)
                    {
                        throw new InvalidSyntaxException($"Unable to `{OperatorToken.Value}` {Value.DataType}",
                            OperatorToken.LineNumber);
                    }

                    DataType = BaseDataType.Boolean;
                    bool value = (bool)runner.Stack.Pop();
                    runner.Stack.Add(!value);
                    break;
                }
            case TokenType.Minus:
                {
                    if (Value.DataType != BaseDataType.Number)
                    {
                        throw new InvalidSyntaxException($"Unable to `{OperatorToken.Value}` {Value.DataType}",
                            OperatorToken.LineNumber);
                    }

                    DataType = BaseDataType.Number;
                    double value = (double)runner.Stack.Pop();
                    runner.Stack.Add(-value);

                    break;
                }
            default:
                throw new InvalidSyntaxException($"Unknown unary operator encountered (`{OperatorToken.Value}`)",
                    OperatorToken.LineNumber);
        }
    }

    public override string ToString(string indent)
    {
        return $"""
                {indent} Unary Expression:
                {indent + "\t"} Operator: {OperatorToken.Value}
                {indent + "\t"} Value:
                {Value.ToString(indent + "\t\t")}
                """;
    }
}