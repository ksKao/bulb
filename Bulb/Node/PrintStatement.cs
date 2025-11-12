namespace Bulb.Node;

public class PrintStatement(Node value) : Node
{
    private Node Value { get; } = value;

    public override void Run()
    {
        Value.Run();

        object value = Runner.Stack.Pop();

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