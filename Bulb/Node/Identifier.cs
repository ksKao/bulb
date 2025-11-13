namespace Bulb.Node;

public class Identifier(Token identifierToken) : Expression
{
    private Token IdentifierToken { get; } = identifierToken;

    public override DataType DataType { get; protected set; }

    public override void Run(Runner runner)
    {
        if (!runner.TryGetVariable(IdentifierToken.Value, out Variable variable))
        {
            throw new InvalidSyntaxException($"Identifier '{IdentifierToken.Value}' does not exist.",
                IdentifierToken.LineNumber);
        }

        DataType = variable.DataType;
        runner.Stack.Add(runner.Stack.ElementAt(variable.StackLocation));
    }

    public override string ToString(string indent)
    {
        return indent + IdentifierToken.Value;
    }
}