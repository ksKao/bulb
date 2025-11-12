namespace Bulb.Node;

public class PrintStatement(Node value) : Node
{
    public Node Value { get; } = value;

    public override string ToString(string indent)
    {
        return $"""
                {indent} Print Statement
                {indent + "\t"} Value:
                {Value.ToString(indent + "\t\t")}
                """;
    }
}