namespace Bulb.Node;

public class NumericLiteral(double value) : Node
{
    private double Value { get; } = value;

    public override void Run(Runner runner)
    {
        runner.Stack.Add(Value);
    }

    public override string ToString(string indent)
    {
        return indent + Value;
    }
}