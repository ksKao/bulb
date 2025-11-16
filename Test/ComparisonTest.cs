using Bulb.Exceptions;

namespace Test;

public class ComparisonTest
{
    [Fact(DisplayName = "Basic Equals")]
    public void Basic_Equals()
    {
        string output = Utils.RunCode("""
                                      print 5 == 5;
                                      """);

        Assert.Equal("true\n", output);
    }

    [Fact(DisplayName = "Equals False")]
    public void Equals_False()
    {
        string output = Utils.RunCode("""
                                      print 5 == 4;
                                      """);

        Assert.Equal("false\n", output);
    }

    [Fact(DisplayName = "Basic Not Equals")]
    public void Basic_Not_Equals()
    {
        string output = Utils.RunCode("""
                                      print 5 != 5;
                                      """);

        Assert.Equal("false\n", output);
    }

    [Fact(DisplayName = "Greater Than True")]
    public void Greater_Than_True()
    {
        string output = Utils.RunCode("""
                                      print 10 > 5;
                                      """);

        Assert.Equal("true\n", output);
    }

    [Fact(DisplayName = "Greater Than False")]
    public void Greater_Than_False()
    {
        string output = Utils.RunCode("""
                                      print 10 > 20;
                                      """);

        Assert.Equal("false\n", output);
    }

    [Fact(DisplayName = "Greater Than Or Equals To")]
    public void Greater_Than_Or_Equals_To()
    {
        string output = Utils.RunCode("""
                                      print 5 >= 5;
                                      """);

        Assert.Equal("true\n", output);
    }

    [Fact(DisplayName = "Less Than True")]
    public void Less_Than_True()
    {
        string output = Utils.RunCode("""
                                      print 3 < 5;
                                      """);

        Assert.Equal("true\n", output);
    }

    [Fact(DisplayName = "Less Than False")]
    public void Less_Than_False()
    {
        string output = Utils.RunCode("""
                                      print 10 < 5;
                                      """);

        Assert.Equal("false\n", output);
    }

    [Fact(DisplayName = "Less Than Or Equals To")]
    public void Less_Than_Or_Equals_To()
    {
        string output = Utils.RunCode("""
                                      print 5 <= 5;
                                      """);

        Assert.Equal("true\n", output);
    }

    [Fact(DisplayName = "Comparison Precedence")]
    public void Comparison_Precedence()
    {
        string output = Utils.RunCode("""
                                      print 10 > 3 + 9;
                                      """);

        Assert.Equal("false\n", output);
    }

    [Fact(DisplayName = "Compare With Number Variable")]
    public void Compare_With_Number_Variable()
    {
        string output = Utils.RunCode("""
                                      let x = 3;
                                      print x < 10;
                                      """);

        Assert.Equal("true\n", output);
    }

    [Fact(DisplayName = "Cannot Compare Different Data Type")]
    public void Cannot_Compare_Different_Data_Type()
    {
        InvalidSyntaxException ex = Assert.Throws<InvalidSyntaxException>(() =>
            Utils.RunCode("""
                          print 5 < true;
                          """)
        );

        Assert.Equal("Unable to `<` Number and Boolean", ex.Message);
        Assert.Equal(1, ex.LineNumber);
    }

    [Fact(DisplayName = "Double Comparison")]
    public void Double_Comparison()
    {
        string output = Utils.RunCode("""
                                      print 5 == 5 == true;
                                      """);

        Assert.Equal("true\n", output);
    }

    [Fact(DisplayName = "String Comparison")]
    public void String_Comparison()
    {
        string output = Utils.RunCode("""
                                      print "abc" == "abc";
                                      """);

        Assert.Equal("true\n", output);
    }

    [Fact(DisplayName = "String Comparison Not Equals")]
    public void String_Comparison_Not_Equals()
    {
        string output = Utils.RunCode("""
                                      print "abc" != "def";
                                      """);

        Assert.Equal("true\n", output);
    }
}