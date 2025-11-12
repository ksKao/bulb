namespace Bulb.Node;

public class Identifier(Token identifierToken) : Node
{
    private Token IdentifierToken { get; } = identifierToken;

    public override void Run()
    {
        if (!Runner.Variables.TryGetValue(IdentifierToken.Value, out int stackIndex))
        {
            throw new InvalidSyntaxException($"Identifier '{IdentifierToken.Value}' does not exist.",
                IdentifierToken.LineNumber);
        }

        Runner.Stack.Push(Runner.Stack.ElementAt(stackIndex));
    }

    public override string ToString(string indent)
    {
        return indent + IdentifierToken.Value;
    }
}