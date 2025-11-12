namespace Bulb.Node;

public class VariableDeclarationStatement(Token identifier, Node value) : Node
{
    private Token Identifier { get; } = identifier;
    private Node Value { get; } = value;

    public override void Run(Runner runner)
    {
        if (runner.Variables.ContainsKey(Identifier.Value))
        {
            throw new InvalidSyntaxException($"Variable `{Identifier.Value}` already exists.", Identifier.LineNumber);
        }

        Value.Run(runner);

        runner.Variables[Identifier.Value] = runner.Stack.Count - 1;
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