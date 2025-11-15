namespace Bulb.Node;

public class ExpressionStatement(Expression expression) : Node
{
    private Expression Expression { get; } = expression;

    public override void Run(Runner runner)
    {
        Expression.Run(runner);

        // after run, pop the value off since we don't need it anyway
        runner.Stack.Pop();
    }

    public override string ToString(string indent)
    {
        return $"""
                {indent} Expression Statement:
                {Expression.ToString(indent + "\t")}";"
                """;
    }
}