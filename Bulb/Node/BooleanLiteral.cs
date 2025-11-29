using Bulb.DataType;

namespace Bulb.Node;

public class BooleanLiteral(bool value) : Expression
{
    private bool Value { get; } = value;

    public override BaseDataType? DataType { get; protected set; } = BaseDataType.Boolean;

    public override void Run(Runner runner)
    {
        runner.Stack.Add(Value);
    }

    public override string ToString(string indent)
    {
        return indent + Value.ToString().ToLower();
    }
}