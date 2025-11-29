using Bulb.Exceptions;

namespace Bulb.Node;

public class ReturnStatement(Token returnToken, Expression? returnValue) : Node
{
    private Token ReturnToken { get; } = returnToken;
    private Expression? ReturnValue { get; } = returnValue;

    public override void Run(Runner runner)
    {
        ScopeContext? functionScopeContext = runner.ScopeContexts.LastOrDefault(sc => sc.ReturnType is not null);

        // if not in function, return means exit the program
        if (functionScopeContext is null)
        {
            if (ReturnValue is not null)
            {
                throw new InvalidSyntaxException("Unable to return value when not inside function",
                    ReturnToken.LineNumber);
            }

            Environment.Exit(0);
        }

        if (functionScopeContext.ReturnType == "void")
        {
            if (ReturnValue is not null)
            {
                throw new InvalidSyntaxException("Unable to return value in a void function",
                    ReturnToken.LineNumber);
            }

            // end all the scopes before returning
            while (runner.ScopeContexts.Peek().ReturnType is not null)
            {
                runner.EndScope();
            }
        }
        else
        {
            if (ReturnValue is null)
            {
                throw new InvalidSyntaxException("A return value is required for non-void function return type.",
                    ReturnToken.LineNumber);
            }

            ReturnValue.Run(runner);

            if (ReturnValue is null)
            {
                throw new InvalidSyntaxException("Unexpected null type in return statement.", ReturnToken.LineNumber);
            }

            if (ReturnValue.DataType.Name != functionScopeContext.ReturnType)
            {
                throw new InvalidSyntaxException(
                    $"Unable to return `{ReturnValue.DataType.Name}` in a function with `{functionScopeContext.ReturnType}` return type.",
                    ReturnToken.LineNumber);
            }

            object returnValue = runner.Stack.Pop();

            // end all the scopes before returning
            while (runner.ScopeContexts.Peek().ReturnType is null)
            {
                runner.EndScope();
            }

            runner.Stack.Add(returnValue);
        }

        throw new ReturnException();
    }

    public override string ToString(string indent)
    {
        return $"""
                 {indent} Return Statement:
                 {indent + "\t"} Return Value:
                 {ReturnValue?.ToString(indent + "\t\t") ?? $"{indent + "\t\t"} <empty>"}
                """;
    }
}