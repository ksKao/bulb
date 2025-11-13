using System.Text;

namespace Bulb.Node;

public class Scope : Node
{
    public List<Node> Statements { get; } = [];

    public override void Run(Runner runner)
    {
        runner.BeginScope();

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