namespace Bulb.Node;

public class Identifier(Token identifierToken) : Node
{
    public Token IdentifierToken { get; } = identifierToken;

    public override string ToString(string indent)
    {
        return indent + IdentifierToken.Value;
    }
}