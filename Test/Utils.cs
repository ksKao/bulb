using Bulb;
using Bulb.Node;

namespace Test;

public static class Utils
{
    public static string RunCode(string code)
    {
        StringWriter stringWriter = new();
        Console.SetOut(stringWriter);

        Token[] tokens = new Lexer().Tokenize(code);

        Program program = new Parser().Parse(tokens);

        program.Run(new Runner());

        return stringWriter.ToString();
    }
}