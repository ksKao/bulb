using System.Text;

using Bulb.Exceptions;

namespace Bulb.Node;

public class Program : Node
{
    public List<Node> Statements { get; } = [];

    public override void Run(Runner runner)
    {
        try
        {
            Statements.ForEach(s => s.Run(runner));
        }
        catch (ReturnException) { }
    }

    public override string ToString()
    {
        return ToString("");
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