namespace Bulb.Node;

public class NumericLiteral(double value) : Node
{
    public double Value { get; } = value;
}