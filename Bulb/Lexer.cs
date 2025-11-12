namespace Bulb;

public static class Lexer
{
    private static readonly Token[] Keywords = [new(TokenType.Let, "let")];

    private static string s_src = "";
    private static int s_i;
    private static int s_lineNumber = 1;

    private static char CurrentChar => s_src.Length <= s_i ? '\0' : s_src[s_i];
    private static char NextChar => s_src.Length <= s_i + 1 ? '\0' : s_src[s_i + 1];

    public static Token[] Tokenize(string src)
    {
        List<Token> tokens = [];
        s_i = 0;
        s_src = src;

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

        tokens.Add(new Token(TokenType.Eof, "", s_lineNumber));
        return tokens.ToArray();
    }

    private static void Advance()
    {
        if (CurrentChar == '\n')
        {
            s_lineNumber++;
        }

        s_i++;
    }

    private static bool GetIsValidWordChar(char c)
    {
        // only alphabets, numbers, and underscores are allowed to be in an identifier/keyword
        return char.IsLetterOrDigit(c) || c == '_';
    }

    private static Token ParseNumber()
    {
        string value = "";

        while (char.IsDigit(CurrentChar))
        {
            value += CurrentChar;
            Advance();
        }

        return new Token(TokenType.Number, value, s_lineNumber);
    }

    private static Token ParseWord()
    {
        string value = "";

        while (GetIsValidWordChar(CurrentChar))
        {
            value += CurrentChar;

            if (!GetIsValidWordChar(NextChar))
            {
                Token? found = Keywords.FirstOrDefault(t => t.Value == value);

                if (found is not null)
                {
                    Advance();
                    return new Token(found.Type, found.Value, s_lineNumber);
                }

                // reaching this case means there are no matching keywords, i.e., the word is an identifier
                Advance();
                return new Token(TokenType.Identifier, value, s_lineNumber);
            }

            // if no words are matched (e.g. we are in the middle of a keyword like 'le' for let, just continue to the next character
            Advance();
        }

        throw new InvalidSyntaxException($"Invalid symbol encountered. `{value}`", s_lineNumber);
    }

    private static Token ParseSymbol()
    {
        Token token;

        switch (CurrentChar)
        {
            case '=':
                token = new Token(TokenType.Equals, "=", s_lineNumber);
                break;
            case ';':
                token = new Token(TokenType.Semicolon, ";", s_lineNumber);
                break;
            default:
                throw new InvalidSyntaxException($"Invalid symbol encountered. `{CurrentChar}`", s_lineNumber);
        }

        Advance();
        return token;
    }
}