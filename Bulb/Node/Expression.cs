using Bulb.DataType;

namespace Bulb.Node;

public abstract class Expression : Node
{
    public abstract BaseDataType? DataType { get; protected set; }
}