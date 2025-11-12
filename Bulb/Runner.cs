namespace Bulb;

public class Runner
{
    // key is the variable name, value is the index of the variable in stack
    // actually can directly store the actual value in the kv pair, but not doing so because of constant variable lookup time
    public Dictionary<string, int> Variables { get; } = [];

    public Stack<object> Stack { get; } = [];
}