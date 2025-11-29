using Bulb.DataType;
using Bulb.Exceptions;

namespace Bulb.Node;

public class PrintStatement(Token printToken, Expression value) : Node
{
    private Token PrintToken { get; } = printToken;
    private Expression Value { get; } = value;

    public override void Run(Runner runner)
    {
        Value.Run(runner);

        object value = runner.Stack.Pop();

        if (Value.DataType is null)
        {
            throw new InvalidSyntaxException("Unexpected null data type in print statement.", PrintToken.LineNumber);
        }

        if (Value.DataType.Name == "void")
        {
            throw new InvalidSyntaxException("Unable to print `void`", PrintToken.LineNumber);
        }

        if (Value.DataType == BaseDataType.Boolean)
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