using Bulb;
using Bulb.Node;

namespace Test;

public static class Utils
{
    public static void RunCode(string code)
    {
        Token[] tokens = new Lexer().Tokenize(code);

        Program program = new Parser().Parse(tokens);

        program.Run();
    }
}