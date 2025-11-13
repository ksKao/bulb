namespace Bulb.Node;

public abstract class Expression : Node
{
    public abstract DataType DataType { get; protected set; }
}