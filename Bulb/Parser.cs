using Bulb.Node;

namespace Bulb;

public class Parser
{
    private int _i;
    private Token[] _tokens = [];

    private Token CurrentToken => _tokens[_i];
    private bool IsEof => CurrentToken.Type == TokenType.Eof;

    public Program Parse(Token[] tokens)
    {
        _tokens = tokens;

        Program program = new();

        while (!IsEof)
        {
            program.Statements.Add(ParseStatement());
        }

        return program;
    }

    private Token Eat(TokenType? expectedType = null)
    {
        if (_tokens.Length == 0)
        {
            throw new Exception("No tokens found.");
        }

        if (expectedType is not null && CurrentToken.Type != expectedType)
        {
            throw new Exception($"Expected {expectedType} but found {CurrentToken.Type}");
        }

        // need to cache first before increment otherwise the returned token will not be accurate
        Token cacheToken = CurrentToken;

        _i++;

        return cacheToken;
    }

    private Node.Node ParseExpression()
    {
        return ParseAdditiveExpression();
    }

    private Node.Node ParseStatement()
    {
        return CurrentToken.Type switch
        {
            TokenType.Let => ParseVariableDeclarationStatement(),
            TokenType.Print => ParsePrintStatement(),
            _ => throw new InvalidSyntaxException($"Unexpected token {CurrentToken.Value}", CurrentToken.LineNumber)
        };
    }

    private PrintStatement ParsePrintStatement()
    {
        Eat(TokenType.Print);

        PrintStatement printStatement = new(ParseExpression());

        Eat(TokenType.Semicolon);

        return printStatement;
    }

    private VariableDeclarationStatement ParseVariableDeclarationStatement()
    {
        Eat(TokenType.Let);

        Token identifier = Eat(TokenType.Identifier);

        Eat(TokenType.Equals);

        VariableDeclarationStatement variableDeclarationStatement = new(identifier, ParseExpression());

        Eat(TokenType.Semicolon);

        return variableDeclarationStatement;
    }

    private Node.Node ParseAdditiveExpression()
    {
        Node.Node left = ParseMultiplicativeExpression();

        while (CurrentToken.Type == TokenType.Plus || CurrentToken.Type == TokenType.Minus)
        {
            Token operatorToken = Eat();

            left = new BinaryExpression(operatorToken, left, ParseMultiplicativeExpression());
        }

        return left;
    }

    private Node.Node ParseMultiplicativeExpression()
    {
        Node.Node left = ParsePrimaryExpression();

        while (CurrentToken.Type == TokenType.Multiply || CurrentToken.Type == TokenType.Divide)
        {
            Token operatorToken = Eat();

            left = new BinaryExpression(operatorToken, left, ParsePrimaryExpression());
        }

        return left;
    }

    private Node.Node ParsePrimaryExpression()
    {
        switch (CurrentToken.Type)
        {
            case TokenType.Number:
                return new NumericLiteral(double.Parse(Eat(TokenType.Number).Value));
            case TokenType.Identifier:
                return new Identifier(Eat(TokenType.Identifier));
            case TokenType.OpenParenthesis:
                Eat(TokenType.OpenParenthesis);
                Node.Node expression = ParseExpression();
                Eat(TokenType.CloseParenthesis);
                return expression;
            default:
                throw new InvalidSyntaxException($"Unexpected token `{CurrentToken.Value}`", CurrentToken.LineNumber);
        }
    }
}