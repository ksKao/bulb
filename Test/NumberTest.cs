using Bulb.Exceptions;

namespace Test;

public class NumberTest
{
    [Fact(DisplayName = "Number With Decimal")]
    public void Number_With_Decimal()
    {
        string output = Utils.RunCode("print 1.5;");

        Assert.Equal("1.5\n", output);
    }

    [Fact(DisplayName = "Cannot Number With Double Decimal")]
    public void Cannot_Number_With_Double_Decimal()
    {
        InvalidSyntaxException ex = Assert.Throws<InvalidSyntaxException>(() => Utils.RunCode("print 1.5.3;"));

        Assert.Equal("Expected Identifier but found Number", ex.Message);
        Assert.Equal(1, ex.LineNumber);
    }

    [Fact(DisplayName = "Negative Number")]
    public void Negative_Numbers()
    {
        string output = Utils.RunCode("print -1.5;");

        Assert.Equal("-1.5\n", output);
    }
}