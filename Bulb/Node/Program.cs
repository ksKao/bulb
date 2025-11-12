using System.Text;

namespace Bulb.Node;

public class Program : Node
{
    public List<Node> Statements { get; } = [];

    public override void Run()
    {
        Statements.ForEach(s => s.Run());
    }

    public override string ToString(string indent)
    {
        StringBuilder sb = new();

        sb.AppendLine($"{indent} Program: ");

        foreach (Node node in Statements)
        {
            sb.AppendLine(node.ToString(indent + "\t"));
        }

        return sb.ToString();
    }
}