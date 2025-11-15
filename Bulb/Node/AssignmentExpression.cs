using Bulb.Enums;
using Bulb.Exceptions;

namespace Bulb.Node;

public class AssignmentExpression(Token identifierToken, Expression value) : Expression
{
    private Token Identifier { get; } = identifierToken;
    private Expression Value { get; } = value;

    public override DataType DataType { get; protected set; }


    public override void Run(Runner runner)
    {
        if (!runner.TryGetVariable(Identifier.Value, out Variable variable))
        {
            throw new InvalidSyntaxException($"Variable `{Identifier.Value}` is not defined.", Identifier.LineNumber);
        }

        Value.Run(runner);

        DataType = Value.DataType;

        if (Value.DataType != variable.DataType)
        {
            throw new InvalidSyntaxException($"Unable to assign {Value.DataType} to {variable.DataType}.",
                Identifier.LineNumber);
        }

        object value = runner.Stack.Pop();

        runner.Stack[variable.StackLocation] = value;

        runner.Stack.Add(value);
    }

    public override string ToString(string indent)
    {
        return $"""
                {indent} Assignment:
                {indent + "\t"} Identifier: 
                {indent + "\t\t"} {Identifier.Value}
                {indent + "\t"} Value: 
                {Value.ToString(indent + "\t\t")}
                """;
    }
}