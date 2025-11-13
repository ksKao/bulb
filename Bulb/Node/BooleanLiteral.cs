using Bulb.Enums;

namespace Bulb.Node;

public class BooleanLiteral(bool value) : Expression
{
    private bool Value { get; } = value;

    public override DataType DataType { get; protected set; } = DataType.Boolean;

    public override void Run(Runner runner)
    {
        runner.Stack.Add(Value);
    }

    public override string ToString(string indent)
    {
        return indent + Value.ToString().ToLower();
    }
}