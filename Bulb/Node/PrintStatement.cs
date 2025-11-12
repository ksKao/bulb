namespace Bulb.Node;

public class PrintStatement(Node value) : Node
{
    private Node Value { get; } = value;

    public override void Run(Runner runner)
    {
        Value.Run(runner);

        object value = runner.Stack.Pop();

        Console.WriteLine(value);
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