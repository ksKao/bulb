using Bulb.DataType;

namespace Bulb.Node;

public class NumericLiteral(double value) : Expression
{
    private double Value { get; } = value;

    public override BaseDataType? DataType { get; protected set; } = BaseDataType.Number;

    public override void Run(Runner runner)
    {
        runner.Stack.Add(Value);
    }

    public override string ToString(string indent)
    {
        return indent + Value;
    }
}