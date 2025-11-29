using Bulb.DataType;
using Bulb.Exceptions;

namespace Bulb.Node;

public class WhileStatement(Token whileToken, Expression condition, Scope scope) : Node
{
    private Token WhileToken { get; } = whileToken;
    private Expression Condition { get; } = condition;
    private Scope Scope { get; } = scope;

    public override void Run(Runner runner)
    {
        Scope.IsStoppable = true;

        Condition.Run(runner);

        if (Condition.DataType is null)
        {
            throw new InvalidSyntaxException("Unexpected null data type in while condition", WhileToken.LineNumber);
        }

        if (Condition.DataType != BaseDataType.Boolean)
        {
            throw new InvalidSyntaxException($"While loop condition cannot be of type `{Condition.DataType}`.",
                WhileToken.LineNumber);
        }

        bool shouldRun = (bool)runner.Stack.Pop();

        try
        {
            while (shouldRun)
            {
                try
                {
                    Scope.Run(runner);
                }
                catch (ContinueException) { }

                Condition.Run(runner);
                shouldRun = (bool)runner.Stack.Pop();
            }
        }
        catch (BreakException) { }
    }

    public override string ToString(string indent)
    {
        return $"""
                {indent} While Loop:
                {indent + "\t"} Condition:
                {Condition.ToString(indent + "\t\t")}
                {indent + "\t"} Body:
                {Scope.ToString(indent + "\t\t")}
                """;
    }
}