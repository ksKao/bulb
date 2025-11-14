using Bulb.Exceptions;

namespace Bulb.Node;

public class VariableDeclarationStatement(Token identifier, Expression value) : Node
{
    private Token Identifier { get; } = identifier;
    private Expression Value { get; } = value;

    public override void Run(Runner runner)
    {
        if (runner.TryGetVariable(Identifier.Value, out _))
        {
            throw new InvalidSyntaxException($"Variable `{Identifier.Value}` already exists.", Identifier.LineNumber);
        }

        Value.Run(runner);

        runner.Variables.Add(new Variable(Identifier.Value, runner.Stack.Count - 1, Value.DataType));
    }

    public override string ToString(string indent)
    {
        return $"""
                {indent} Variable Declaration:
                {indent + "\t"} Identifier: 
                {indent + "\t\t"} {Identifier.Value}
                {indent + "\t"} Value: 
                {Value.ToString(indent + "\t\t")}
                """;
    }
}