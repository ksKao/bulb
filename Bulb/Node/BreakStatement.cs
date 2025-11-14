using Bulb.Exceptions;

namespace Bulb.Node;

public class BreakStatement(Token breakToken) : Node
{
    private Token BreakToken { get; } = breakToken;

    public override void Run(Runner runner)
    {
        // check if we are actually in a valid breakable statement
        if (!runner.ScopeContexts.Any(x => x.IsStoppable))
        {
            throw new InvalidSyntaxException("Invalid break statement.", BreakToken.LineNumber);
        }

        // end all the scopes before the breaking loop
        while (!runner.ScopeContexts.Last().IsStoppable)
        {
            runner.EndScope();
        }

        throw new BreakException();
    }

    public override string ToString(string indent)
    {
        return $"{indent} Break Statement";
    }
}