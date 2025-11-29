using Bulb.Exceptions;

namespace Test;

public class BuiltInFunctionTest
{
    [Fact(DisplayName = "Cannot Redeclare Built In Function")]
    public void Cannot_Redeclare_Built_In_Function()
    {
        InvalidSyntaxException ex = Assert.Throws<InvalidSyntaxException>(() => Utils.RunCode("""
                function prompt(): string
                {
                    return "";
                }
                """
        ));

        Assert.Equal("Function `prompt` already exists with the same signature.", ex.Message);
        Assert.Equal(1, ex.LineNumber);
    }
}