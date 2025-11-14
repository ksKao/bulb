using System.Text;

namespace Bulb.Node;

public class Scope : Node
{
    public List<Node> Statements { get; } = [];

    // only true for interruptable scopes like while, for, functions, and etc...
    public bool IsStoppable { get; set; } = false;

    public override void Run(Runner runner)
    {
        runner.BeginScope(IsStoppable);

        Statements.ForEach(s => s.Run(runner));

        runner.EndScope();
    }

    public override string ToString(string indent)
    {
        StringBuilder sb = new();

        sb.AppendLine($"{indent} Scope: ");

        foreach (Node node in Statements)
        {
            sb.AppendLine(node.ToString(indent + "\t"));
        }

        return sb.ToString();
    }
}