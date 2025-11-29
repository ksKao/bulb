using Bulb.Exceptions;

namespace Test;

public class WhileTest
{
    [Fact(DisplayName = "Basic While")]
    public void Basic_While()
    {
        string output = Utils.RunCode("""
                                      let x = 0;

                                      while (x < 5)
                                      {
                                        x = x + 1;
                                      }

                                      print x;
                                      """);

        Assert.Equal("5\n", output);
    }

    [Fact(DisplayName = "Nested While")]
    public void Nested_While()
    {
        string output = Utils.RunCode("""
                                      let x = 0;
                                      let y = 0;

                                      while (x < 3)
                                      {
                                        x = x + 1;
                                        y = 0;
                                        while (y < 3)
                                        {
                                            y = y + 1;
                                            print y;
                                        }
                                      }

                                      print x;
                                      """);

        Assert.Equal("1\n2\n3\n1\n2\n3\n1\n2\n3\n3\n", output);
    }

    [Fact(DisplayName = "Break With Nested While")]
    public void Break_With_Nested_While()
    {
        string output = Utils.RunCode("""
                                      let x = 0;
                                      let y = 0;

                                      while (x < 3)
                                      {
                                        x = x + 1;
                                        y = 0;
                                        while (y < 3)
                                        {
                                          y = y + 1;
                                          print y;
                                          
                                           if (y == 2)
                                           {
                                               break;
                                           }
                                        }
                                      }

                                      print x;
                                      """);

        Assert.Equal("1\n2\n1\n2\n1\n2\n3\n", output);
    }

    [Fact(DisplayName = "Continue With Nested While")]
    public void Continue_With_Nested_While()
    {
        string output = Utils.RunCode("""
                                      let x = 0;
                                      let y = 0;

                                      while (x < 3)
                                      {
                                        x = x + 1;
                                        y = 0;
                                        while (y < 3)
                                        {
                                          y = y + 1;
                                          print y;
                                          
                                           if (y == 2)
                                           {
                                                continue;
                                           }
                                           
                                           print true;
                                        }
                                      }

                                      print x;
                                      """);

        Assert.Equal("1\ntrue\n2\n3\ntrue\n1\ntrue\n2\n3\ntrue\n1\ntrue\n2\n3\ntrue\n3\n", output);
    }

    [Fact(DisplayName = "Cannot Break In Root")]
    public void Cannot_Break_In_Root()
    {
        InvalidSyntaxException ex = Assert.Throws<InvalidSyntaxException>(() => Utils.RunCode("break;"));

        Assert.Equal("Invalid break statement.", ex.Message);
        Assert.Equal(1, ex.LineNumber);
    }

    [Fact(DisplayName = "Cannot Continue In Root")]
    public void Cannot_Continue_In_Root()
    {
        InvalidSyntaxException ex = Assert.Throws<InvalidSyntaxException>(() => Utils.RunCode("continue;"));

        Assert.Equal("Invalid continue statement.", ex.Message);
        Assert.Equal(1, ex.LineNumber);
    }

    [Fact(DisplayName = "Cannot Break in Scope")]
    public void Cannot_Break_In_Scope()
    {
        InvalidSyntaxException ex = Assert.Throws<InvalidSyntaxException>(() => Utils.RunCode("{ break; }"));

        Assert.Equal("Invalid break statement.", ex.Message);
        Assert.Equal(1, ex.LineNumber);
    }

    [Fact(DisplayName = "Variable Declaration in While With Continue")]
    public void Variable_Declaration_In_While_With_Continue()
    {
        string output = Utils.RunCode("""
                                      let x = 0;

                                      while (x < 5)
                                      {
                                        let y = 0;
                                        x = x + 1;
                                        
                                        if (x == 2)
                                        {
                                            continue;
                                        }
                                      }

                                      print x;
                                      """);

        Assert.Equal("5\n", output);
    }

    [Fact(DisplayName = "Variable Redeclaration After While Break")]
    public void Variable_Redeclaration_After_While_Break()
    {
        string output = Utils.RunCode("""
                                      let x = 0;

                                      while (x < 5)
                                      {
                                        let y = 0;
                                        x = x + 1;
                                        
                                        if (x == 2)
                                        {
                                            break;
                                        }
                                      }

                                      let y = 5;
                                      print y;
                                      """);

        Assert.Equal("5\n", output);
    }
}