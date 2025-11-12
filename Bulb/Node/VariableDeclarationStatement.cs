namespace Bulb.Node;

public class VariableDeclarationStatement(Token identifier, Node value) : Node
{
    public Token Identifier { get; } = identifier;
    public Node Value { get; } = value;

    public override string ToString(string indent)
    {
        return $$"""
                 {{indent}} Variable Declaration:
                 {{indent + "\t"}} Identifier: 
                 {{indent + "\t\t"}} {{Identifier.Value}}
                 {{indent + "\t"}} Value: 
                 {{Value.ToString(indent + "\t\t")}}
                 """;
    }
}