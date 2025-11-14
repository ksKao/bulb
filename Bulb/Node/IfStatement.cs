using System.Text;

using Bulb.Enums;

namespace Bulb.Node;

public class IfStatement(
    Expression condition,
    Scope scope,
    Token ifToken,
    IfStatement? alternateIf = null,
    Scope? alternateElse = null)
    : Node
{
    private Expression Condition { get; } = condition;
    private Scope Scope { get; } = scope;
    private Token IfToken { get; } = ifToken;
    private IfStatement? AlternateIf { get; } = alternateIf;
    private Scope? AlternateElse { get; } = alternateElse;

    public override void Run(Runner runner)
    {
        Condition.Run(runner);

        if (Condition.DataType != DataType.Boolean)
        {
            throw new InvalidSyntaxException($"Unable to perform `if` on a {Condition.DataType}", IfToken.LineNumber);
        }

        bool evaluatedCondition = (bool)runner.Stack.Pop();

        if (evaluatedCondition)
        {
            Scope.Run(runner);
        }
        else
        {
            AlternateIf?.Run(runner);
            AlternateElse?.Run(runner);
        }
    }

    public override string ToString(string indent)
    {
        StringBuilder sb = new();

        sb.AppendLine($"{indent} If Statement:");
        sb.AppendLine($"{indent + "\t"} Condition:");
        sb.AppendLine(Condition.ToString(indent + "\t\t"));

        if (AlternateIf is not null)
        {
            sb.AppendLine($"{indent + "\t"} Else If: ");
            sb.AppendLine(AlternateIf.ToString(indent + "\t\t"));
        }

        if (AlternateElse is not null)
        {
            sb.AppendLine($"{indent + "\t"} Else: ");
            sb.AppendLine(AlternateElse.ToString(indent + "\t\t"));
        }

        return sb.ToString();
    }
}