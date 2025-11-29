using System.Text;

using Bulb.DataType;
using Bulb.Exceptions;

namespace Bulb.Node;

public class CallExpression(Token identifierToken, List<Expression> arguments) : Expression
{
    private Token IdentifierToken { get; } = identifierToken;
    private List<Expression> Arguments { get; } = arguments;

    public override BaseDataType? DataType { get; protected set; }

    public override void Run(Runner runner)
    {
        Arguments.ForEach(a => a.Run(runner));

        FunctionDeclarationStatement? existingFunction = runner.Functions.FirstOrDefault(f =>
            f.IsSameSignature(IdentifierToken.Value, Arguments.Select(a => a.DataType?.ToString() ?? "null").ToList()));

        if (existingFunction is null)
        {
            throw new InvalidSyntaxException(
                $"A function named `{IdentifierToken.Value}` that takes in {(Arguments.Count > 0 ? string.Join(", ", Arguments.Select(a => a.DataType?.ToString())) : "0 arguments")} does not exist.",
                IdentifierToken.LineNumber);
        }

        // no need to verify the validity of ReturnTypeToken here because the function declaration should already have verified it.
        DataType = runner.GetDataType(existingFunction.ReturnTypeToken.Value) ??
                   throw new InvalidSyntaxException("Invalid return type", existingFunction.ReturnTypeToken.LineNumber);

        for (int i = 0; i < Arguments.Count; i++)
        {
            runner.Variables.Add(new Variable(existingFunction.Parameters[i].nameToken.Value,
                runner.Stack.Count + i - Arguments.Count,
                runner.GetDataType(existingFunction.Parameters[i].typeToken.Value) ??
                throw new InvalidSyntaxException("Invalid argument type",
                    existingFunction.Parameters[i].typeToken.LineNumber)));
        }

        bool returned = false;
        try
        {
            existingFunction.Body.Run(runner);
        }
        catch (ReturnException)
        {
            returned = true;
        }

        if (!returned && DataType.Name != "void")
        {
            throw new InvalidSyntaxException(
                $"`{existingFunction.IdentifierToken.Value}` function is missing a return statement.",
                existingFunction.IdentifierToken.LineNumber);
        }

        object? returnValue = null;

        if (DataType.Name != "void")
        {
            returnValue = runner.Stack.Pop();
        }

        for (int i = 0; i < Arguments.Count; i++)
        {
            runner.Variables.Pop();
            runner.Stack.Pop(); // pop the arguments
        }

        runner.Stack.Add(returnValue ??
                         0); // if no return value, just push some random value there because ExpressionStatement will have something to pop out
    }

    public override string ToString(string indent)
    {
        StringBuilder sb = new();

        sb.AppendLine($"""
                       {indent} Call Expression:
                       {indent + "\t"} Function Name:
                       {indent + "\t\t"} {IdentifierToken.Value}
                       """);

        if (Arguments.Count > 0)
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