using Bulb.Exceptions;

namespace Test;

public class ScopeTest
{
    [Fact(DisplayName = "Cannot Redeclare Same Variable Name In Deeper Scope")]
    public void Cannot_Redeclare_Same_Variable_Name_In_Deeper_Scope()
    {
        InvalidSyntaxException ex = Assert.Throws<InvalidSyntaxException>(() =>
            Utils.RunCode("""
                            {
                                let x = 10;
                                {
                                    let x = 10;
                                }
                            }
                          """)
        );

        Assert.Equal("Variable `x` already exists.", ex.Message);
        Assert.Equal(4, ex.LineNumber);
    }

    [Fact(DisplayName = "Redeclare Same Variable Name In Outer Scope")]
    public void Redeclare_Same_Variable_Name_In_Outer_Scope()
    {
        string output = Utils.RunCode("""
                                      {
                                        let x = 10;
                                      }
                                      let x = 8;
                                      print x;
                                      """);

        Assert.Equal("8\n", output);
    }
}