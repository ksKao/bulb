namespace Bulb.Node;

public class NumericLiteral(double value) : Expression
{
    private double Value { get; } = value;

    public override DataType DataType { get; protected set; } = DataType.Number;

    public override void Run(Runner runner)
    {
        runner.Stack.Add(Value);
    }

    public override string ToString(string indent)
    {
        return indent + Value;
    }
}