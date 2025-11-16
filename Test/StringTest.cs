using Bulb.Exceptions;

namespace Test;

public class StringTest
{
    [Fact(DisplayName = "String Concatenation")]
    public void String_Concatenation()
    {
        string output = Utils.RunCode("""
                                      print "hello" + " " + 1;
                                      """);

        Assert.Equal("hello 1\n", output);
    }

    [Fact(DisplayName = "String Escaping")]
    public void String_Escaping()
    {
        string output = Utils.RunCode("""
                                      print "\\t";
                                      """);

        Assert.Equal("\\t\n", output);
    }

    [Fact(DisplayName = "Invalid String Escaping")]
    public void Invalid_String_Escaping()
    {
        InvalidSyntaxException ex = Assert.Throws<InvalidSyntaxException>(() => Utils.RunCode("""
            print "\a";
            """));

        Assert.Equal("`\\a` is not a valid escape sequence.", ex.Message);
        Assert.Equal(1, ex.LineNumber);
    }

    [Fact(DisplayName = "String Unclosed Quotes")]
    public void String_Unclosed_Quotes()
    {
        InvalidSyntaxException ex = Assert.Throws<InvalidSyntaxException>(() => Utils.RunCode("""
            print "abc;
            """));

        Assert.Equal("Missing closing `\"` for string literal", ex.Message);
        Assert.Equal(1, ex.LineNumber);
    }

    [Fact(DisplayName = "String Concatenation With Non String Type First")]
    public void String_Concatenation_With_Non_String_Type_First()
    {
        string output = Utils.RunCode("""
                                      print 1 + " hello";
                                      """);

        Assert.Equal("1 hello\n", output);
    }

    [Fact(DisplayName = "String Concatenation With Two Numbers First")]
    public void String_Concatenation_With_Two_Numbers_First()
    {
        string output = Utils.RunCode("""
                                      print 1 + 2 + " hello";
                                      """);

        Assert.Equal("3 hello\n", output);
    }
}