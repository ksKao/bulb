namespace Bulb;

public class Token(TokenType type, string value, int lineNumber = 0)
{
    public TokenType Type { get; init; } = type;
    public string Value { get; init; } = value;
    public int LineNumber { get; init; } = lineNumber;

    public override string ToString()
    {
        return $"{Value} ({Type}, Line {LineNumber})";
    }
}