namespace Bulb;

public class Runner
{
    public List<Variable> Variables { get; } = [];

    public Stack<object> Stack { get; } = [];

    /*
     * {
     *      -- At this point, the value will be [1]
     *      let b = 2;
     *      let c = 3;
     *      {
     *          -- At this point, the value will be [1, 3]
     *          let d = 4;
     *          {
     *              -- At this point, the value will be [1, 3, 4]
     *              let e = 5;

     * stores the number of variables declared when different scope starts
     * for example:
     * ...some code...
     * let a = 1;*
     *          }
     *          -- At this point, the stack will be [1, 3]
     *      }
     * }
     * ...some more code...
     */
    private Stack<int> NumberOfVariablesDeclaredBeforeScope { get; } = [];

    public bool TryGetVariable(string identifier, out Variable variable)
    {
        Variable? found = Variables.FirstOrDefault(v => v.Name == identifier);

        variable = found!;

        return found is not null;
    }

    public void BeginScope()
    {
        NumberOfVariablesDeclaredBeforeScope.Push(Variables.Count);
    }

    public void EndScope()
    {
        int popCount = Variables.Count - NumberOfVariablesDeclaredBeforeScope.Pop();

        if (popCount <= 0)
        {
            return;
        }

        Variables.RemoveRange(Variables.Count - popCount, popCount);

        for (int i = 0; i < popCount; i++)
        {
            Stack.Pop();
        }
    }
}