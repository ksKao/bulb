namespace Test;

public class PrintTest
{
    private readonly StringWriter _stringWriter = new();

    public PrintTest()
    {
        Console.SetOut(_stringWriter);
        _stringWriter.Flush();
    }

    [Fact]
    public void Print_Number()
    {
        Utils.RunCode("""
                        print 5;
                      """);

        Assert.Equal("5\n", _stringWriter.ToString());
    }

    [Fact]
    public void Print_Variable()
    {
        Utils.RunCode("""
                        let x = 3;
                        print x;
                      """);

        Assert.Equal("3\n", _stringWriter.ToString());
    }
}