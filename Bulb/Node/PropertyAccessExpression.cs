using System.Text;

using Bulb.DataType;
using Bulb.Exceptions;

namespace Bulb.Node;

public class PropertyAccessExpression(
    Expression member,
    Token propertyIdentifierToken,
    List<Expression>? arguments = null) : Expression
{
    private Expression Member { get; } = member;
    private Token PropertyIdentifierToken { get; } = propertyIdentifierToken;
    private List<Expression>? Arguments { get; } = arguments;

    public override BaseDataType? DataType { get; protected set; }

    public override void Run(Runner runner)
    {
        Member.Run(runner);

        BaseDataType? dataType = Member.DataType;

        switch (dataType)
        {
            case null:
                throw new InvalidSyntaxException("Unexpected member null data type",
                    PropertyIdentifierToken.LineNumber);
            case BuiltInDataType builtInDataType:
                if (Arguments is null)
                {
                    if (!builtInDataType.Fields.TryGetValue(PropertyIdentifierToken.Value,
                            out (string returnType, Action<Runner> action) field))
                    {
                        throw new InvalidSyntaxException(
                            $"Field `{PropertyIdentifierToken.Value}` does not exist in type `{builtInDataType.Name}`",
                            PropertyIdentifierToken.LineNumber);
                    }

                    field.action(runner);

                    DataType = runner.GetDataType(field.returnType);
                }
                else
                {
                    Arguments.ForEach(a => a.Run(runner));

                    (FunctionDeclarationStatement declaration, Action<Runner> action) method =
                        builtInDataType.Methods.FirstOrDefault(m =>
                            m.declaration.IsSameSignature(PropertyIdentifierToken.Value,
                                Arguments.Select(a => a.DataType?.ToString() ?? "null").ToList()));

                    if (method.declaration is null)
                    {
                        throw new InvalidSyntaxException(
                            $"A method named `{PropertyIdentifierToken.Value}` that takes in {(Arguments.Count > 0 ? string.Join(", ", Arguments.Select(a => a.DataType?.ToString())) : "0 arguments")} for type `{builtInDataType.Name}` does not exist.",
                            // check if field/method exists
                            PropertyIdentifierToken.LineNumber);
                    }

                    method.action(runner);

                    DataType = runner.GetDataType(method.declaration.ReturnTypeToken.Value);
                }

                break;
            default:
                throw new InvalidSyntaxException("Unexpected data type", PropertyIdentifierToken.LineNumber);
        }
    }

    public override string ToString(string indent)
    {
        StringBuilder sb = new();

        sb.AppendLine($"""
                       {indent} Property Access Expression:
                       {indent + "\t"} Member:
                       {Member.ToString(indent + "\t\t")}
                       {indent + "\t"} Property Name:
                       {indent + "\t\t"} {PropertyIdentifierToken.Value}
                       {indent + "\t"} Type:
                       {indent + "\t\t"} {(Arguments is null ? "Field" : "Method")}
                       """);

        if (Arguments?.Count > 0)
        {
            sb.AppendLine($"{indent + "\t"} Arguments:");

            foreach (Expression arg in Arguments)
            {
                sb.AppendLine(arg.ToString(indent + "\t\t"));
            }
        }

        return sb.ToString();
    }
}