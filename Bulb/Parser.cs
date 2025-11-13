using Bulb.Enums;
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

    private Expression ParseExpression()
    {
        return ParseAdditiveExpression();
    }

    private Node.Node ParseStatement()
    {
        return CurrentToken.Type switch
        {
            TokenType.Let => ParseVariableDeclarationStatement(),
            TokenType.Print => ParsePrintStatement(),
            TokenType.OpenCurly => ParseScope(),
            TokenType.Identifier => ParseAssignmentStatement(),
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

    private AssignmentStatement ParseAssignmentStatement()
    {
        Token identifier = Eat(TokenType.Identifier);

        Eat(TokenType.Equals);

        AssignmentStatement assignmentStatement = new(identifier, ParseExpression());

        Eat(TokenType.Semicolon);

        return assignmentStatement;
    }

    private Scope ParseScope()
    {
        Eat(TokenType.OpenCurly);

        Scope scope = new();

        while (CurrentToken.Type != TokenType.CloseCurly)
        {
            scope.Statements.Add(ParseStatement());
        }

        Eat(TokenType.CloseCurly);

        return scope;
    }

    private Expression ParseAdditiveExpression()
    {
        Expression left = ParseMultiplicativeExpression();

        while (CurrentToken.Type is TokenType.Plus or TokenType.Minus)
        {
            Token operatorToken = Eat();

            left = new BinaryExpression(operatorToken, left, ParseMultiplicativeExpression());
        }

        return left;
    }

    private Expression ParseMultiplicativeExpression()
    {
        Expression left = ParsePrimaryExpression();

        while (CurrentToken.Type is TokenType.Multiply or TokenType.Divide)
        {
            Token operatorToken = Eat();

            left = new BinaryExpression(operatorToken, left, ParsePrimaryExpression());
        }

        return left;
    }

    private Expression ParsePrimaryExpression()
    {
        switch (CurrentToken.Type)
        {
            case TokenType.Number:
                return new NumericLiteral(double.Parse(Eat(TokenType.Number).Value));
            case TokenType.Identifier:
                return new Identifier(Eat(TokenType.Identifier));
            case TokenType.OpenParenthesis:
                Eat(TokenType.OpenParenthesis);
                Expression expression = ParseExpression();
                Eat(TokenType.CloseParenthesis);
                return expression;
            case TokenType.True:
                Eat(TokenType.True);
                return new BooleanLiteral(true);
            case TokenType.False:
                Eat(TokenType.False);
                return new BooleanLiteral(false);
            default:
                throw new InvalidSyntaxException($"Unexpected token `{CurrentToken.Value}`", CurrentToken.LineNumber);
        }
    }
}