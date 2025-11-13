namespace Bulb.Node;

public class AssignmentStatement(Token identifierToken, Expression value) : Node
{
    private Token Identifier { get; } = identifierToken;
    private Expression Value { get; } = value;


    public override void Run(Runner runner)
    {
        if (!runner.TryGetVariable(Identifier.Value, out Variable variable))
        {
            throw new InvalidSyntaxException($"Variable `{Identifier.Value}` is not defined.", Identifier.LineNumber);
        }

        Value.Run(runner);

        if (Value.DataType != variable.DataType)
        {
            throw new InvalidSyntaxException($"Unable to assign {Value.DataType} to {variable.DataType}.",
                Identifier.LineNumber);
        }

        object value = runner.Stack.Pop();

        runner.Stack[variable.StackLocation] = value;
    }

    public override string ToString(string indent)
    {
        return $"""
                {indent} Assignment:
                {indent + "\t"} Identifier: 
                {indent + "\t\t"} {Identifier.Value}
                {indent + "\t"} Value: 
                {Value.ToString(indent + "\t\t")}
                """;
    }
}