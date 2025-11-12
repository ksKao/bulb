namespace Bulb;

public class InvalidSyntaxException(string message, int lineNumber) : Exception(message)
{
    public int LineNumber { get; init; } = lineNumber;
}