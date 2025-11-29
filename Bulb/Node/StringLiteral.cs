using Bulb.DataType;

namespace Bulb.Node;

public class StringLiteral(string value) : Expression
{
    private string Value { get; } = value;
    public override BaseDataType? DataType { get; protected set; } = BaseDataType.String;

    public override void Run(Runner runner)
    {
        runner.Stack.Add(Value);
    }

    public override string ToString(string indent)
    {
        return indent + Value;
    }
}