namespace Bulb;

internal static class Program
{
    private static void Main(string[] args)
    {
        try
        {
            if (args.Length == 0)
                throw new ArgumentException("File path is reuiqred.");
            
            string filePath = args[0];

            string fileContent = File.ReadAllText(filePath);
            
            Console.WriteLine(fileContent);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            Environment.Exit(1);
        }
    }
}