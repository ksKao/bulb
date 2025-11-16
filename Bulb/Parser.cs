using Bulb.Enums;
using Bulb.Exceptions;
using Bulb.Node;

namespace Bulb;

public class Parser
{
    private int _i;
    private Token[] _tokens = [];

    private Token CurrentToken => _tokens[_i];
    private Token? NextToken => _i < _tokens.Length - 1 ? _tokens[_i + 1] : null;
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

    private Node.Node ParseStatement()
    {
        return CurrentToken.Type switch
        {
            TokenType.Let => ParseVariableDeclarationStatement(),
            TokenType.Print => ParsePrintStatement(),
            TokenType.OpenCurly => ParseScope(),
            TokenType.If => ParseIfStatement(),
            TokenType.While => ParseWhileStatement(),
            TokenType.Break => ParseBreakStatement(),
            TokenType.Continue => ParseContinueStatement(),
            TokenType.For => ParseForStatement(),
            _ => ParseExpressionStatement()
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

    private IfStatement ParseIfStatement()
    {
        IfStatement ifStatement;

        Token ifToken = Eat(TokenType.If);

        Eat(TokenType.OpenParenthesis);
        Expression condition = ParseExpression();
        Eat(TokenType.CloseParenthesis);

        Scope scope = ParseScope();

        if (CurrentToken.Type == TokenType.Else)
        {
            Eat(TokenType.Else);

            ifStatement = CurrentToken.Type == TokenType.If
                ? new IfStatement(condition, scope, ifToken, ParseIfStatement())
                : new IfStatement(condition, scope, ifToken, null, ParseScope());
        }
        else
        {
            ifStatement = new IfStatement(condition, scope, ifToken);
        }

        return ifStatement;
    }

    private WhileStatement ParseWhileStatement()
    {
        Token whileToken = Eat(TokenType.While);

        Eat(TokenType.OpenParenthesis);

        Expression condition = ParseExpression();

        Eat(TokenType.CloseParenthesis);

        Scope scope = ParseScope();

        return new WhileStatement(whileToken, condition, scope);
    }

    private BreakStatement ParseBreakStatement()
    {
        Token breakToken = Eat(TokenType.Break);
        Eat(TokenType.Semicolon);

        return new BreakStatement(breakToken);
    }

    private ContinueStatement ParseContinueStatement()
    {
        Token continueToken = Eat(TokenType.Continue);
        Eat(TokenType.Semicolon);

        return new ContinueStatement(continueToken);
    }

    private ForStatement ParseForStatement()
    {
        Token forToken = Eat(TokenType.For);

        Eat(TokenType.OpenParenthesis);

        Node.Node? initStatement = null;

        if (CurrentToken.Type != TokenType.Semicolon)
        {
            initStatement = ParseStatement();
        }
        else
        {
            Eat(TokenType.Semicolon);
        }

        Expression? conditionExpression = CurrentToken.Type == TokenType.Semicolon ? null : ParseExpression();

        Eat(TokenType.Semicolon);

        Expression? updateExpression = CurrentToken.Type == TokenType.CloseParenthesis ? null : ParseExpression();

        Eat(TokenType.CloseParenthesis);

        return new ForStatement(forToken, initStatement, conditionExpression, updateExpression, ParseScope());
    }

    private Expression ParseExpressionStatement()
    {
        Expression expression = ParseExpression();

        Eat(TokenType.Semicolon);

        return expression;
    }

    private Expression ParseExpression()
    {
        return ParseAssignmentExpression();
    }

    private Expression ParseAssignmentExpression()
    {
        while (CurrentToken.Type == TokenType.Identifier && NextToken?.Type == TokenType.Equals)
        {
            Token identifier = Eat(TokenType.Identifier);

            Eat(TokenType.Equals);

            AssignmentExpression assignmentExpression = new(identifier, ParseAssignmentExpression());

            return assignmentExpression;
        }

        return ParseComparisonExpression();
    }


    private Expression ParseComparisonExpression()
    {
        Expression left = ParseAdditiveExpression();

        while (CurrentToken.Type is TokenType.DoubleEqual or TokenType.NotEqual or TokenType.GreaterThan
               or TokenType.GreaterThanOrEqual or TokenType.LessThan or TokenType.LessThanOrEqual)
        {
            Token operatorToken = Eat();

            left = new BinaryExpression(operatorToken, left, ParseAdditiveExpression());
        }

        return left;
    }

    private Expression ParseAdditiveExpression()
    {
        Expression left = ParseMultiplicativeExpression();

        while (CurrentToken.Type is TokenType.Plus or TokenType.Minus or TokenType.Or)
        {
            Token operatorToken = Eat();

            left = new BinaryExpression(operatorToken, left, ParseMultiplicativeExpression());
        }

        return left;
    }

    private Expression ParseMultiplicativeExpression()
    {
        Expression left = ParseUnaryExpression();

        while (CurrentToken.Type is TokenType.Multiply or TokenType.Divide or TokenType.And)
        {
            Token operatorToken = Eat();

            left = new BinaryExpression(operatorToken, left, ParseUnaryExpression());
        }

        return left;
    }

    private Expression ParseUnaryExpression()
    {
        while (CurrentToken.Type is TokenType.Not)
        {
            switch (CurrentToken.Type)
            {
                case TokenType.Not:
                    Token operatorToken = Eat();
                    UnaryExpression unaryExpression = new(operatorToken, ParseUnaryExpression());
                    return unaryExpression;
            }
        }

        return ParseUpdateExpression();
    }

    private Expression ParseUpdateExpression()
    {
        if (CurrentToken.Type is TokenType.Identifier && NextToken?.Type is TokenType.Increment or TokenType.Decrement)
        {
            Token identifierToken = Eat(TokenType.Identifier);
            Token operatorToken = Eat();
            return new UpdateExpression(identifierToken, operatorToken, false);
        }

        if (CurrentToken.Type is TokenType.Increment or TokenType.Decrement)
        {
            Token operatorToken = Eat();
            Token identifierToken = Eat(TokenType.Identifier);
            return new UpdateExpression(identifierToken, operatorToken, true);
        }

        return ParsePrimaryExpression();
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
            case TokenType.String:
                return new StringLiteral(Eat(TokenType.String).Value);
            default:
                throw new InvalidSyntaxException($"Unexpected token `{CurrentToken.Value}`", CurrentToken.LineNumber);
        }
    }
}