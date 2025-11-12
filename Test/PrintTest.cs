[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace Test;

public class PrintTest
{
    [Fact(DisplayName = "Print Number")]
    public void Print_Number()
    {
        string output = Utils.RunCode("""
                                        print 5;
                                      """);

        Assert.Equal("5\n", output);
    }

    [Fact(DisplayName = "Print Variable")]
    public void Print_Variable()
    {
        string output = Utils.RunCode("""
                                        let x = 3;
                                        print x;
                                      """);

        Assert.Equal("3\n", output);
    }
}