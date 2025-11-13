using Bulb.Enums;

namespace Bulb.Node;

public class PrintStatement(Expression value) : Node
{
    private Expression Value { get; } = value;

    public override void Run(Runner runner)
    {
        Value.Run(runner);

        object value = runner.Stack.Pop();

        if (Value.DataType == DataType.Boolean)
        {
            Console.WriteLine(((bool)value).ToString().ToLower());
        }
        else
        {
            Console.WriteLine(value);
        }
    }

    public override string ToString(string indent)
    {
        return $"""
                {indent} Print Statement
                {indent + "\t"} Value:
                {Value.ToString(indent + "\t\t")}
                """;
    }
}