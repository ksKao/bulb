using System.Text;

namespace Bulb.Node;

public class Scope : Node
{
    public List<Node> Statements { get; } = [];

    // only true for interruptable scopes like while, for, functions, and etc...
    public bool IsStoppable { get; set; }
    public string? ReturnType { get; set; } = null;

    public override void Run(Runner runner)
    {
        runner.BeginScope(IsStoppable, ReturnType);

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