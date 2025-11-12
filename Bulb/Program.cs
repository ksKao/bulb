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
                throw new ArgumentException("File path is required.");
            }

            string filePath = args[0];

            string fileContent = File.ReadAllText(filePath);

            Token[] tokens = Lexer.Tokenize(fileContent);

            Program program = Parser.Parse(tokens);

            Console.WriteLine(program.ToString(""));
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