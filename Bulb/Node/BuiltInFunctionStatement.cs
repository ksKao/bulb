namespace Bulb.Node;

public class BuiltInFunctionStatement(
    Token identifierToken,
    List<(Token nameToken, Token typeToken)> parameters,
    Token returnTypeToken,
    Action<Runner> action) : FunctionDeclarationStatement(identifierToken, parameters, returnTypeToken, new Scope())
{
    public Action<Runner> Action { get; } = action;

    public override void Run(Runner runner)
    {
        Action(runner);
    }
}