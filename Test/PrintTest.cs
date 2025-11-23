using Bulb.Exceptions;

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

    [Fact(DisplayName = "Print String")]
    public void Print_String()
    {
        string output = Utils.RunCode("""
                                        print "abc";
                                      """);

        Assert.Equal("abc\n", output);
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

    [Fact(DisplayName = "Print String Variable")]
    public void Print_String_Variable()
    {
        string output = Utils.RunCode("""
                                        let x = "test";
                                        print x;
                                      """);

        Assert.Equal("test\n", output);
    }

    [Fact(DisplayName = "Cannot Print Void Type")]
    public void Cannot_Print_Void_Type()
    {
        InvalidSyntaxException ex = Assert.Throws<InvalidSyntaxException>(() => Utils.RunCode("""
            function test(): void {}

            print test();
            """));

        Assert.Equal("Unable to print `void`", ex.Message);
        Assert.Equal(3, ex.LineNumber);
    }
}