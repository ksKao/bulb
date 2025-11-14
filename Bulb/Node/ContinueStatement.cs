using Bulb.Exceptions;

namespace Bulb.Node;

public class ContinueStatement(Token continueToken) : Node
{
    public Token ContinueToken { get; } = continueToken;

    public override void Run(Runner runner)
    {
        // check if we are actually in a valid breakable statement
        if (!runner.ScopeContexts.Any(x => x.IsStoppable))
        {
            throw new InvalidSyntaxException("Invalid continue statement.", ContinueToken.LineNumber);
        }

        // end all the scopes before the breaking loop
        while (!runner.ScopeContexts.Last().IsStoppable)
        {
            runner.EndScope();
        }

        throw new ContinueException();
    }

    public override string ToString(string indent)
    {
        return $"{indent} Continue Statement";
    }
}