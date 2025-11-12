namespace Bulb.Node;

public class NumericLiteral(double value) : Node
{
    public double Value { get; } = value;

    public override string ToString(string indent)
    {
        return indent + Value;
    }
}