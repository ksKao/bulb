using Bulb.Exceptions;
using Bulb.Node;

namespace Bulb;

internal static class App
{
    private static void Main(string[] args)
    {
        try
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Missing path.");
                Console.WriteLine("Usage: bulb <path_to_script>");
                Environment.Exit(1);
            }

            string filePath = args[0];

            string fileContent = File.ReadAllText(filePath);

            Token[] tokens = new Lexer().Tokenize(fileContent);

            Program program = new Parser().Parse(tokens);

            program.Run(new Runner());
        }
        catch (InvalidSyntaxException e)
        {
            Console.WriteLine($"Error at line {e.LineNumber}: {e.Message}");
            Environment.Exit(1);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            Environment.Exit(1);
        }
    }
}