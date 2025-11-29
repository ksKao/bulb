using Bulb.Node;

namespace Bulb.DataType;

public class BuiltInDataType(
    string name,
    Dictionary<string, (string returnType, Action<Runner> action)> fields,
    List<(FunctionDeclarationStatement declaration, Action<Runner> action)> methods) : BaseDataType(name)
{
    public Dictionary<string, (string returnType, Action<Runner> action)> Fields { get; } = fields;
    public List<(FunctionDeclarationStatement declaration, Action<Runner> action)> Methods { get; } = methods;
}