using Bulb.Node;

namespace Bulb;

public static class Parser
{
    private static Token[] s_tokens = [];
    private static int s_i;

    private static Token CurrentToken => s_tokens[s_i];
    private static bool IsEof => CurrentToken.Type == TokenType.Eof;

    public static Program Parse(Token[] tokens)
    {
        s_tokens = tokens;

        Program program = new();

        while (!IsEof)
        {
            program.Statements.Add(ParseStatement());
        }

        return program;
    }

    private static Token Eat(TokenType? expectedType = null)
    {
        if (s_tokens.Length == 0)
        {
            throw new Exception("No tokens found.");
        }

        if (expectedType is not null && CurrentToken.Type != expectedType)
        {
            throw new Exception($"Expected {expectedType} but found {CurrentToken.Type}");
        }

        // need to cache first before increment otherwise the returned token will not be accurate
        Token cacheToken = CurrentToken;

        s_i++;

        return cacheToken;
    }

    private static Node.Node ParseExpression()
    {
        return ParsePrimaryExpression();
    }

    private static Node.Node ParseStatement()
    {
        return CurrentToken.Type switch
        {
            TokenType.Let => ParseVariableDeclarationStatement(),
            _ => throw new InvalidSyntaxException($"Unexpected token {CurrentToken.Value}", CurrentToken.LineNumber)
        };
    }

    private static VariableDeclarationStatement ParseVariableDeclarationStatement()
    {
        Eat(TokenType.Let);

        Token identifier = Eat(TokenType.Identifier);

        Eat(TokenType.Equals);

        VariableDeclarationStatement variableDeclarationStatement = new(identifier, ParseExpression());

        Eat(TokenType.Semicolon);

        return variableDeclarationStatement;
    }

    private static Node.Node ParsePrimaryExpression()
    {
        return CurrentToken.Type switch
        {
            TokenType.Number => new NumericLiteral(double.Parse(Eat(TokenType.Number).Value)),
            _ => throw new InvalidSyntaxException($"Unexpected token `{CurrentToken.Value}`", CurrentToken.LineNumber)
        };
    }
}