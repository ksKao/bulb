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

    [Fact(DisplayName = "Print Boolean")]
    public void Print_Boolean()
    {
        string output = Utils.RunCode("""
                                        print true;
                                      """);

        Assert.Equal("true\n", output);
    }

    [Fact(DisplayName = "Print Number Variable")]
    public void Print_Number_Variable()
    {
        string output = Utils.RunCode("""
                                        let x = 3;
                                        print x;
                                      """);

        Assert.Equal("3\n", output);
    }

    [Fact(DisplayName = "Print Boolean Variable")]
    public void Print_Boolean_Variable()
    {
        string output = Utils.RunCode("""
                                        let x = false;
                                        print x;
                                      """);

        Assert.Equal("false\n", output);
    }
}