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
}