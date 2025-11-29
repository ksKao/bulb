using Bulb.Exceptions;

namespace Test;

public class BuiltInPropertyTest
{
    [Fact(DisplayName = "Number to String")]
    public void Number_to_String()
    {
        string output = Utils.RunCode("print 1.toString();");

        Assert.Equal("1\n", output);
    }

    [Fact(DisplayName = "Number to Fixed")]
    public void Number_to_Fixed()
    {
        string output = Utils.RunCode("print 1.123456.toFixed(1 + 1);");

        Assert.Equal("1.12\n", output);
    }

    [Fact(DisplayName = "String Is Number")]
    public void String_Is_Number()
    {
        string output = Utils.RunCode("print \"123\".isNumber();");

        Assert.Equal("true\n", output);
    }

    [Fact(DisplayName = "String Is Not Number")]
    public void String_Is_Not_Number()
    {
        string output = Utils.RunCode("print \"abc\".isNumber();");

        Assert.Equal("false\n", output);
    }

    [Fact(DisplayName = "String to Number")]
    public void String_To_Number()
    {
        string output = Utils.RunCode("print \"123\".toNumber() + 1;");

        Assert.Equal("124\n", output);
    }

    [Fact(DisplayName = "Cannot Convert Non Number String To Number")]
    public void Cannot_Convert_Non_Number_String_To_Number()
    {
        RuntimeException ex = Assert.Throws<RuntimeException>(() => Utils.RunCode("print \"abc\".toNumber();"));

        Assert.Equal("Unable to convert `abc` to a number.", ex.Message);
    }

    [Fact(DisplayName = "String to Upper")]
    public void String_To_Upper()
    {
        string output = Utils.RunCode("""
                                      let str = "abc";
                                      print str.toUpper();
                                      print str; // make sure its immutable
                                      """);

        Assert.Equal("ABC\nabc\n", output);
    }

    [Fact(DisplayName = "String to Lower")]
    public void String_To_Lower()
    {
        string output = Utils.RunCode("""
                                      let str = "ABC";
                                      print str.toLower();
                                      print str; // make sure its immutable
                                      """);

        Assert.Equal("abc\nABC\n", output);
    }

    [Fact(DisplayName = "String Char At")]
    public void String_Char_At()
    {
        string output = Utils.RunCode("""
                                      let str = "ABC";
                                      print str.charAt(1);
                                      print str; // make sure its immutable
                                      """);

        Assert.Equal("B\nABC\n", output);
    }

    [Fact(DisplayName = "String Char At Index Less Than Zero")]
    public void String_Char_At_Index_Less_Than_Zero()
    {
        RuntimeException ex = Assert.Throws<RuntimeException>(() => Utils.RunCode("""
            let str = "abc";
            print str.charAt(-1);
            """));

        Assert.Equal("`charAt` index cannot be less than 0.", ex.Message);
    }

    [Fact(DisplayName = "String Char At Index Equals to Length")]
    public void String_Char_At_Index_Equals_To_Length()
    {
        RuntimeException ex = Assert.Throws<RuntimeException>(() => Utils.RunCode("""
            let str = "abc";
            print str.charAt(3);
            """));

        Assert.Equal("`charAt` index cannot be more than or equal to the string length.", ex.Message);
    }

    [Fact(DisplayName = "Cannot Access Invalid Method")]
    public void Cannot_Access_Invalid_Method()
    {
        InvalidSyntaxException ex = Assert.Throws<InvalidSyntaxException>(() => Utils.RunCode("print 1.isNumber();"));

        Assert.Equal("A method named `isNumber` that takes in 0 arguments for type `number` does not exist.",
            ex.Message);
        Assert.Equal(1, ex.LineNumber);
    }

    [Fact(DisplayName = "Cannot Access Method With Invalid Arguments")]
    public void Cannot_Access_Method_With_Invalid_Arguments()
    {
        InvalidSyntaxException ex =
            Assert.Throws<InvalidSyntaxException>(() => Utils.RunCode("print 1.toFixed(\"\");"));

        Assert.Equal("A method named `toFixed` that takes in string for type `number` does not exist.",
            ex.Message);
        Assert.Equal(1, ex.LineNumber);
    }

    [Fact(DisplayName = "Cannot Access Invalid Field")]
    public void Cannot_Access_Invalid_Field()
    {
        InvalidSyntaxException ex = Assert.Throws<InvalidSyntaxException>(() => Utils.RunCode("print 1.length;"));

        Assert.Equal("Field `length` does not exist in type `number`", ex.Message);
        Assert.Equal(1, ex.LineNumber);
    }

    [Fact(DisplayName = "Method Field Chaining")]
    public void Method_Field_Chaining()
    {
        string output = Utils.RunCode("""
                                      let str = "ABC";
                                      print (str.length + 1).toFixed(3).isNumber();
                                      """);

        Assert.Equal("true\n", output);
    }
}