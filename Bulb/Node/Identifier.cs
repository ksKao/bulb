namespace Bulb.Node;

public class Identifier(Token identifierToken) : Node
{
    private Token IdentifierToken { get; } = identifierToken;

    public override void Run(Runner runner)
    {
        if (!runner.Variables.TryGetValue(IdentifierToken.Value, out int stackIndex))
        {
            throw new InvalidSyntaxException($"Identifier '{IdentifierToken.Value}' does not exist.",
                IdentifierToken.LineNumber);
        }

        runner.Stack.Push(runner.Stack.ElementAt(stackIndex));
    }

    public override string ToString(string indent)
    {
        return indent + IdentifierToken.Value;
    }
}