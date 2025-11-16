using System.Text;

using Bulb.Enums;
using Bulb.Exceptions;

namespace Bulb;

public class Lexer
{
    private readonly Token[] _keywords =
    [
        new(TokenType.Let, "let"), new(TokenType.Print, "print"), new(TokenType.True, "true"),
        new(TokenType.False, "false"), new(TokenType.If, "if"), new(TokenType.Else, "else"),
        new(TokenType.While, "while"), new(TokenType.Break, "break"), new(TokenType.Continue, "continue"),
        new(TokenType.For, "for")
    ];

    private int _i;
    private int _lineNumber = 1;

    private string _src = "";

    private char CurrentChar => _src.Length <= _i ? '\0' : _src[_i];
    private char NextChar => _src.Length <= _i + 1 ? '\0' : _src[_i + 1];

    public Token[] Tokenize(string src)
    {
        List<Token> tokens = [];
        _i = 0;
        _src = src;

        while (CurrentChar != '\0')
        {
            if (char.IsDigit(CurrentChar))
            {
                tokens.Add(ParseNumber());
            }
            else if (char.IsLetter(CurrentChar))
            {
                tokens.Add(ParseWord());
            }
            else if (char.IsWhiteSpace(CurrentChar))
            {
                Advance();
            }
            else
            {
                tokens.Add(ParseSymbol());
            }
        }

        tokens.Add(new Token(TokenType.Eof, "", _lineNumber));
        return tokens.ToArray();
    }

    private void Advance()
    {
        if (CurrentChar == '\n')
        {
            _lineNumber++;
        }

        _i++;
    }

    private bool GetIsValidWordChar(char c)
    {
        // only alphabets, numbers, and underscores are allowed to be in an identifier/keyword
        return char.IsLetterOrDigit(c) || c == '_';
    }

    private Token ParseNumber()
    {
        string value = "";

        bool decimalEncountered = false;
        while (char.IsDigit(CurrentChar) || (!decimalEncountered && CurrentChar == '.'))
        {
            if (CurrentChar == '.')
            {
                decimalEncountered = true;
            }

            value += CurrentChar;
            Advance();
        }

        return new Token(TokenType.Number, value, _lineNumber);
    }

    private Token ParseWord()
    {
        string value = "";

        while (GetIsValidWordChar(CurrentChar))
        {
            value += CurrentChar;

            if (!GetIsValidWordChar(NextChar))
            {
                Token? found = _keywords.FirstOrDefault(t => t.Value == value);

                if (found is not null)
                {
                    Advance();
                    return new Token(found.Type, found.Value, _lineNumber);
                }

                // reaching this case means there are no matching keywords, i.e., the word is an identifier
                Advance();
                return new Token(TokenType.Identifier, value, _lineNumber);
            }

            // if no words are matched (e.g. we are in the middle of a keyword like 'le' for let, just continue to the next character
            Advance();
        }

        throw new InvalidSyntaxException($"Invalid symbol encountered. `{value}`", _lineNumber);
    }

    private Token ParseSymbol()
    {
        Token token;

        switch (CurrentChar)
        {
            case '=':
                if (NextChar == '=')
                {
                    Advance();
                    token = new Token(TokenType.DoubleEqual, "==", _lineNumber);
                    break;
                }

                token = new Token(TokenType.Equals, "=", _lineNumber);
                break;
            case ';':
                token = new Token(TokenType.Semicolon, ";", _lineNumber);
                break;
            case '+':
                if (NextChar == '+')
                {
                    Advance();
                    token = new Token(TokenType.Increment, "++", _lineNumber);
                    break;
                }

                token = new Token(TokenType.Plus, "+", _lineNumber);
                break;
            case '-':
                if (NextChar == '-')
                {
                    Advance();
                    token = new Token(TokenType.Decrement, "--", _lineNumber);
                    break;
                }

                token = new Token(TokenType.Minus, "-", _lineNumber);
                break;
            case '*':
                token = new Token(TokenType.Multiply, "*", _lineNumber);
                break;
            case '/':
                token = new Token(TokenType.Divide, "/", _lineNumber);
                break;
            case '(':
                token = new Token(TokenType.OpenParenthesis, "(", _lineNumber);
                break;
            case ')':
                token = new Token(TokenType.CloseParenthesis, ")", _lineNumber);
                break;
            case '{':
                token = new Token(TokenType.OpenCurly, "{", _lineNumber);
                break;
            case '}':
                token = new Token(TokenType.CloseCurly, "}", _lineNumber);
                break;
            case '!':
                if (NextChar == '=')
                {
                    Advance();
                    token = new Token(TokenType.NotEqual, "!=", _lineNumber);
                    break;
                }

                token = new Token(TokenType.Not, "!", _lineNumber);
                break;
            case '|':
                if (NextChar == '|')
                {
                    Advance();
                    token = new Token(TokenType.Or, "||", _lineNumber);
                    break;
                }

                throw new InvalidSyntaxException($"Invalid symbol encountered. `{CurrentChar}`", _lineNumber);
            case '&':
                if (NextChar == '&')
                {
                    Advance();
                    token = new Token(TokenType.And, "&&", _lineNumber);
                    break;
                }

                throw new InvalidSyntaxException($"Invalid symbol encountered. `{CurrentChar}`", _lineNumber);
            case '>':
                if (NextChar == '=')
                {
                    Advance();
                    token = new Token(TokenType.GreaterThanOrEqual, ">=", _lineNumber);
                    break;
                }

                token = new Token(TokenType.GreaterThan, ">", _lineNumber);
                break;
            case '<':
                if (NextChar == '=')
                {
                    Advance();
                    token = new Token(TokenType.LessThanOrEqual, "<=", _lineNumber);
                    break;
                }

                token = new Token(TokenType.LessThan, "<", _lineNumber);
                break;
            case '"':
                StringBuilder sb = new();

                int startLineNumber = _lineNumber;
                while (NextChar != '"')
                {
                    if (NextChar == '\0')
                    {
                        throw new InvalidSyntaxException("Missing closing `\"` for string literal", _lineNumber);
                    }

                    if (NextChar == '\\')
                    {
                        Advance();

                        switch (NextChar)
                        {
                            case 'n':
                                sb.Append('\n');
                                break;
                            case 't':
                                sb.Append('\t');
                                break;
                            case '\\':
                                sb.Append('\\');
                                break;
                            case '\"':
                                sb.Append('\"');
                                break;
                            default:
                                throw new InvalidSyntaxException($"`\\{NextChar}` is not a valid escape sequence.",
                                    _lineNumber);
                        }
                    }
                    else
                    {
                        sb.Append(NextChar);
                    }

                    Advance();
                }

                token = new Token(TokenType.String, sb.ToString(), startLineNumber);
                Advance();
                break;
            default:
                throw new InvalidSyntaxException($"Invalid symbol encountered. `{CurrentChar}`", _lineNumber);
        }

        Advance();
        return token;
    }
}