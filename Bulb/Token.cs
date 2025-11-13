namespace Bulb;

public class Token(TokenType type, string value, int lineNumber = 0)
{
    public TokenType Type { get; } = type;
    public string Value { get; } = value;
    public int LineNumber { get; } = lineNumber;

    public override string ToString()
    {
        return $"{Value} ({Type}, Line {LineNumber})";
    }
}