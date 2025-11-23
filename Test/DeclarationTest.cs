using Bulb.Exceptions;

namespace Test;

public class DeclarationTest
{
    [Fact(DisplayName = "Cannot Declare Same Identifier")]
    public void Cannot_Declare_Same_Identifier()
    {
        InvalidSyntaxException ex = Assert.Throws<InvalidSyntaxException>(() =>
            Utils.RunCode("""
                          let x = 0;
                          let x = 5;
                          """)
        );

        Assert.Equal("Variable `x` already exists.", ex.Message);
        Assert.Equal(2, ex.LineNumber);
    }

    [Fact(DisplayName = "Cannot Initialize Void Type")]
    public void Cannot_Initialize_Void_Type()
    {
        InvalidSyntaxException ex = Assert.Throws<InvalidSyntaxException>(() =>
            Utils.RunCode("""
                          function test(): void {}

                          let x = test();
                          """)
        );

        Assert.Equal("Unable to declare a variable with `void` type", ex.Message);
        Assert.Equal(3, ex.LineNumber);
    }
}