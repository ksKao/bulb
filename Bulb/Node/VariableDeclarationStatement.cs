namespace Bulb.Node;

public class VariableDeclarationStatement(Token identifier, Node value) : Node
{
    public Token Identifier { get; } = identifier;
    public Node Value { get; set; } = value;
}