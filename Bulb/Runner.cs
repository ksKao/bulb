namespace Bulb;

public static class Runner
{
    // key is the variable name, value is the index of the variable in stack
    // actually can directly store the actual value in the kv pair, but not doing so because of constant variable lookup time
    public static Dictionary<string, int> Variables { get; } = [];

    public static Stack<object> Stack { get; } = [];
}