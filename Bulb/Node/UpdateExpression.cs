using Bulb.Enums;
using Bulb.Exceptions;

namespace Bulb.Node;

public class UpdateExpression(Token identifierToken, Token operatorToken, bool isPrefix) : Expression
{
    private Token IdentifierToken { get; } = identifierToken;
    private Token OperatorToken { get; } = operatorToken;
    private bool IsPrefix { get; } = isPrefix;

    public override DataType DataType { get; protected set; } = DataType.Number;

    public override void Run(Runner runner)
    {
        if (!runner.TryGetVariable(IdentifierToken.Value, out Variable variable))
        {
            throw new InvalidSyntaxException($"Variable `{IdentifierToken.Value}` is not defined.",
                IdentifierToken.LineNumber);
        }

        if (variable.DataType != DataType.Number)
        {
            throw new InvalidSyntaxException($"Unable to perform `{OperatorToken.Value}` on a {variable.DataType}",
                IdentifierToken.LineNumber);
        }

        double oldValue = (double)runner.Stack[variable.StackLocation];
        double newValue = OperatorToken.Type switch
        {
            TokenType.Increment => oldValue + 1,
            TokenType.Decrement => oldValue - 1,
            _ => throw new InvalidSyntaxException($"Invalid update operator `{OperatorToken.Value}`",
                OperatorToken.LineNumber)
        };

        runner.Stack.Add(IsPrefix ? newValue : oldValue);

        runner.Stack[variable.StackLocation] = newValue;
    }

    public override string ToString(string indent)
    {
        return $"""
                 {indent} Update Expression:
                 {indent + "\t"} Operator: {OperatorToken.Value}
                 {indent + "\t"} Identifier: {IdentifierToken.Value}
                 {indent + "\t"} IsPrefix: {IsPrefix}
                """;
    }
}