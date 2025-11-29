using Bulb.DataType;
using Bulb.Exceptions;

namespace Bulb.Node;

public class ForStatement(
    Token forToken,
    Node? initStatement,
    Expression? conditionExpression,
    Expression? updateExpression,
    Scope scope)
    : Node
{
    private Token ForToken { get; } = forToken;
    private Node? InitStatement { get; } = initStatement;
    private Expression? ConditionExpression { get; } = conditionExpression;
    private Expression? UpdateExpression { get; } = updateExpression;
    private Scope Scope { get; } = scope;

    public override void Run(Runner runner)
    {
        Scope.IsStoppable = true;

        // begin a scope at the start because we want to clean up the variable declared in the init statement
        runner.BeginScope(false, null);

        InitStatement?.Run(runner);

        try
        {
            bool shouldRun = true;

            while (shouldRun)
            {
                if (ConditionExpression is not null)
                {
                    ConditionExpression.Run(runner);

                    if (ConditionExpression.DataType is null || ConditionExpression.DataType != BaseDataType.Boolean)
                    {
                        throw new InvalidSyntaxException(
                            $"Unexpected data type in for loop condition `{ConditionExpression.DataType}`",
                            ForToken.LineNumber);
                    }

                    shouldRun = (bool)runner.Stack.Pop();
                }
                else
                {
                    // if no condition, means true by default
                    shouldRun = true;
                }

                if (!shouldRun)
                {
                    break;
                }

                try
                {
                    Scope.Run(runner);
                }
                catch (ContinueException) { }

                if (UpdateExpression is not null)
                {
                    UpdateExpression.Run(runner);

                    // pop off the value from the update expression since we don't need the value, but expression will always push value to the stack
                    runner.Stack.Pop();
                }
            }
        }
        catch (BreakException) { }

        runner.EndScope();
    }

    public override string ToString(string indent)
    {
        return $"""
                {indent} For Statement:
                {indent + "\t"} Init Statement:
                {InitStatement?.ToString(indent + "\t\t") ?? "<empty>"}
                {indent + "\t"} Condition Expression:
                {ConditionExpression?.ToString(indent + "\t\t") ?? "<empty>"}
                {indent + "\t"} Update Expression:
                {UpdateExpression?.ToString(indent + "\t\t") ?? "<empty>"}
                {indent + "\t"} Body:
                {Scope.ToString(indent + "\t\t")}
                """;
    }
}