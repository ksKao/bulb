namespace Bulb.Node;

public abstract class Node
{
    public abstract void Run(Runner runner);

    public abstract string ToString(string indent);
}