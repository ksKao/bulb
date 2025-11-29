using Bulb.DataType;
using Bulb.Node;

namespace Bulb;

public class Runner
{
    private readonly List<BaseDataType> DataTypes =
        [BaseDataType.Number, BaseDataType.Boolean, BaseDataType.String, BaseDataType.Void];

    public List<Variable> Variables { get; } = [];
    public List<FunctionDeclarationStatement> Functions { get; } = [];

    // cant use the actual Stack class here because we need to modify the elements in the middle for assignment statement
    public List<object> Stack { get; } = [];

    /*
     * stores the number of variables declared when different scope starts
     * for example:
     * ...some code...
     * let a = 1;*
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
     *          }
     *          -- At this point, the stack will be [1, 3]
     *      }
     * }
     * ...some more code...
     */
    public Stack<ScopeContext> ScopeContexts { get; } = [];

    public bool TryGetVariable(string identifier, out Variable variable)
    {
        Variable? found = Variables.LastOrDefault(v => v.Name == identifier);

        variable = found!;

        return found is not null;
    }

    public BaseDataType? GetDataType(string name)
    {
        return DataTypes.FirstOrDefault(d => d.Name == name);
    }

    public void BeginScope(bool isStoppable, string? returnType)
    {
        ScopeContexts.Push(new ScopeContext(Variables.Count, Functions.Count, isStoppable, returnType));
    }

    public void EndScope()
    {
        ScopeContext thisScopeContext = ScopeContexts.Pop();
        int variablePopCount = Variables.Count - thisScopeContext.NumberOfVariablesDeclaredBefore;
        int functionPopCount = Functions.Count - thisScopeContext.NumberOfFunctionsDeclaredBefore;

        if (variablePopCount > 0)
        {
            Variables.RemoveRange(Variables.Count - variablePopCount, variablePopCount);

            for (int i = 0; i < variablePopCount; i++)
            {
                Stack.Pop();
            }
        }

        if (functionPopCount > 0)
        {
            Functions.RemoveRange(Functions.Count - functionPopCount, functionPopCount);
        }
    }
}