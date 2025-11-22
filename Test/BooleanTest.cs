using Bulb.Exceptions;

namespace Test;

public class BooleanTest
{
    [Fact(DisplayName = "True Or False")]
    public void True_Or_False()
    {
        string output = Utils.RunCode("""
                                      print true || false;
                                      """);

        Assert.Equal("true\n", output);
    }

    [Fact(DisplayName = "False Or False")]
    public void False_Or_False()
    {
        string output = Utils.RunCode("""
                                      print false || false;
                                      """);

        Assert.Equal("false\n", output);
    }

    [Fact(DisplayName = "True And False")]
    public void True_And_False()
    {
        string output = Utils.RunCode("""
                                      print true && false;
                                      """);

        Assert.Equal("false\n", output);
    }

    [Fact(DisplayName = "True And True")]
    public void True_And_True()
    {
        string output = Utils.RunCode("""
                                      print true && true;
                                      """);

        Assert.Equal("true\n", output);
    }

    [Fact(DisplayName = "Not True")]
    public void Not_True()
    {
        string output = Utils.RunCode("""
                                      print !true;
                                      """);

        Assert.Equal("false\n", output);
    }

    [Fact(DisplayName = "And Or Precedence")]
    public void And_Or_Precedence()
    {
        string output = Utils.RunCode("""
                                      print false && true || false && true;
                                      """);

        Assert.Equal("false\n", output);
    }

    [Fact(DisplayName = "Not Or Precedence")]
    public void Not_Or_Precedence()
    {
        string output = Utils.RunCode("""
                                      print !false || true;
                                      """);

        Assert.Equal("true\n", output);
    }

    [Fact(DisplayName = "Not Or Precedence With Brackets")]
    public void Not_Or_Precedence_With_Brackets()
    {
        string output = Utils.RunCode("""
                                      print !(false || true);
                                      """);

        Assert.Equal("false\n", output);
    }

    [Fact(DisplayName = "Double Not")]
    public void Double_Not()
    {
        string output = Utils.RunCode("""
                                      print !!(false || true);
                                      """);

        Assert.Equal("true\n", output);
    }

    [Fact(DisplayName = "Cannot Boolean Or Number")]
    public void Cannot_Boolean_Or_Number()
    {
        InvalidSyntaxException ex = Assert.Throws<InvalidSyntaxException>(() =>
            Utils.RunCode("""
                          print 5 || true;
                          """)
        );

        Assert.Equal("Unable to `||` number and bool", ex.Message);
        Assert.Equal(1, ex.LineNumber);
    }

    [Fact(DisplayName = "Cannot Not Number")]
    public void Cannot_Not_Number()
    {
        InvalidSyntaxException ex = Assert.Throws<InvalidSyntaxException>(() =>
            Utils.RunCode("""
                          let x = !5;
                          """)
        );

        Assert.Equal("Unable to `!` number", ex.Message);
        Assert.Equal(1, ex.LineNumber);
    }
}