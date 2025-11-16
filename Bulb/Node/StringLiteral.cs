using Bulb.Enums;

namespace Bulb.Node;

public class StringLiteral(string value) : Expression
{
    private string Value { get; } = value;
    public override DataType DataType { get; protected set; } = DataType.String;

    public override void Run(Runner runner)
    {
        runner.Stack.Add(Value);
    }

    public override string ToString(string indent)
    {
        return indent + Value;
    }
}