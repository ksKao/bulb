namespace Bulb.Exceptions;

public class InvalidSyntaxException(string message, int lineNumber) : Exception(message)
{
    public int LineNumber { get; init; } = lineNumber;
}