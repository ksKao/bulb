using System.Text;

using Bulb.Enums;
using Bulb.Exceptions;

namespace Bulb.Node;

public class FunctionDeclarationStatement(
    Token identifierToken,
    List<(Token nameToken, Token typeToken)> parameters,
    Token returnTypeToken,
    Scope body) : Node
{
    public Token IdentifierToken { get; } = identifierToken;
    public List<(Token nameToken, Token typeToken)> Parameters { get; } = parameters;
    public Token ReturnTypeToken { get; } = returnTypeToken;
    public Scope Body { get; } = body;

    public override void Run(Runner runner)
    {
        if (ReturnTypeToken.Value != DataType.String.Name && ReturnTypeToken.Value != DataType.Boolean.Name &&
            ReturnTypeToken.Value != DataType.Number.Name && ReturnTypeToken.Type != TokenType.Void)
        {
            throw new InvalidSyntaxException($"`{ReturnTypeToken.Value}` is not a valid return type.",
                ReturnTypeToken.LineNumber);
        }

        Body.ReturnType = ReturnTypeToken.Value;

        FunctionDeclarationStatement? existingFunction = runner.Functions.FirstOrDefault(f => f.IsSameSignature(this));

        if (existingFunction is not null)
        {
            throw new InvalidSyntaxException(
                $"Function `{IdentifierToken.Value}` already exists with the same signature.",
                IdentifierToken.LineNumber);
        }

        runner.Functions.Add(this);
    }

    public override string ToString(string indent)
    {
        StringBuilder sb = new();

        sb.AppendLine($"""
                        {indent} Function Declaration Statement:
                        {indent + "\t"} Function Name: 
                        {indent + "\t\t"} {IdentifierToken.Value}
                        {indent + "\t"} Return Type:
                        {indent + "\t\t"} {ReturnTypeToken.Value}
                       """);

        if (Parameters.Count > 0)
        {
            sb.AppendLine($"{indent + "\t"} Parameters:");
        }

        foreach ((Token nameToken, Token typeToken) in Parameters)
        {
            sb.AppendLine($"{indent + "\t\t"} {nameToken.Value}: {typeToken.Value}");
        }

        sb.AppendLine($"{indent + "\t"} Body: ");

        sb.AppendLine(Body.ToString(indent + "\t\t"));

        return sb.ToString();
    }

    public bool IsSameSignature(string name, List<string> parameterTypes)
    {
        if (IdentifierToken.Value != name)
        {
            return false;
        }

        if (Parameters.Count != parameterTypes.Count)
        {
            return false;
        }

        for (int i = 0; i < Parameters.Count; i++)
        {
            if (Parameters[i].typeToken.Value != parameterTypes[i])
            {
                return false;
            }
        }

        return true;
    }

    // not using operator overloading here for clarity’s sake
    public bool IsSameSignature(FunctionDeclarationStatement other)
    {
        return IsSameSignature(other.IdentifierToken.Value, other.Parameters.Select(p => p.typeToken.Value).ToList());
    }
}