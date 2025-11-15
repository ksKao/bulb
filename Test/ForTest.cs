using Bulb.Exceptions;

namespace Test;

public class ForTest
{
    [Fact(DisplayName = "Basic_For")]
    public void Basic_For()
    {
        string output = Utils.RunCode("""
                                      for (let i = 0; i < 3; i++)
                                      {
                                        print i;
                                      }
                                      """);

        Assert.Equal("0\n1\n2\n", output);
    }

    [Fact(DisplayName = "Stack Intact After For Loop")]
    public void Stack_Intact_After_For_Loop()
    {
        string output = Utils.RunCode("""
                                      for (let i = 0; i < 3; i++)
                                      {
                                        print i;
                                      }

                                      print true;
                                      """);

        Assert.Equal("0\n1\n2\ntrue\n", output);
    }

    [Fact(DisplayName = "Can Redeclare Variable After For Loop")]
    public void Can_Redeclare_Variable_After_For_Loop()
    {
        string output = Utils.RunCode("""
                                      for (let i = 0; i < 3; i++) {}

                                      let i = 10;
                                      print i;
                                      """);

        Assert.Equal("10\n", output);
    }

    [Fact(DisplayName = "Can Use Regular Assignment In For Loop")]
    public void Can_Use_Regular_Assignment_In_For_Loop()
    {
        string output = Utils.RunCode("""
                                      for (let i = 0; i < 3; i = i + 1) 
                                      {
                                        print i;
                                      }
                                      print true;
                                      """);

        Assert.Equal("0\n1\n2\ntrue\n", output);
    }

    [Fact(DisplayName = "Can Break In For Loop")]
    public void Can_Break_In_For_Loop()
    {
        string output = Utils.RunCode("""
                                      for (let i = 0; i < 3; i = i + 1) 
                                      {
                                        if (i == 2)
                                        {
                                            break;
                                        }
                                        
                                        print i;
                                      }
                                      print true;
                                      """);

        Assert.Equal("0\n1\ntrue\n", output);
    }

    [Fact(DisplayName = "Can Continue In For Loop")]
    public void Can_Continue_In_For_Loop()
    {
        string output = Utils.RunCode("""
                                      for (let i = 0; i < 3; i = i + 1) 
                                      {
                                        if (i == 1)
                                        {
                                            continue;
                                        }
                                        
                                        print i;
                                      }
                                      print true;
                                      """);

        Assert.Equal("0\n2\ntrue\n", output);
    }

    [Fact(DisplayName = "Nested For Loop")]
    public void Nested_For_Loop()
    {
        string output = Utils.RunCode("""
                                      for (let i = 0; i < 3; i = i + 1) 
                                      {
                                        print i;
                                        for (let j = 3; j < 6; j++)
                                        {
                                            print j;
                                        }
                                      }
                                      print true;
                                      """);

        Assert.Equal("0\n3\n4\n5\n1\n3\n4\n5\n2\n3\n4\n5\ntrue\n", output);
    }

    [Fact(DisplayName = "Empty For Loop")]
    public void Empty_For_Loop()
    {
        string output = Utils.RunCode("""
                                      let i = 0;
                                      for (;;) 
                                      {
                                        print i;
                                        
                                        if (i == 2)
                                        {
                                            break;
                                        }
                                        
                                        i++;
                                      }
                                      print true;
                                      """);

        Assert.Equal("0\n1\n2\ntrue\n", output);
    }

    [Fact(DisplayName = "Empty For Loop But Init")]
    public void Empty_For_Loop_But_Init()
    {
        string output = Utils.RunCode("""
                                      for (let i = 0;;) 
                                      {
                                        print i;
                                        
                                        if (i == 2)
                                        {
                                            break;
                                        }
                                        
                                        i++;
                                      }
                                      print true;
                                      """);

        Assert.Equal("0\n1\n2\ntrue\n", output);
    }

    [Fact(DisplayName = "Empty For Loop But Condition")]
    public void Empty_For_Loop_But_Condition()
    {
        string output = Utils.RunCode("""
                                      let i = 0;
                                      for (;i < 3;) 
                                      {
                                        print i;
                                        i++;
                                      }
                                      print true;
                                      """);

        Assert.Equal("0\n1\n2\ntrue\n", output);
    }

    [Fact(DisplayName = "Empty For Loop But Update")]
    public void Empty_For_Loop_But_Update()
    {
        string output = Utils.RunCode("""
                                      let i = 0;
                                      for (;; i++) 
                                      {
                                        print i;
                                        
                                        if (i == 2)
                                        {
                                            break;
                                        }
                                      }
                                      print true;
                                      """);

        Assert.Equal("0\n1\n2\ntrue\n", output);
    }

    [Fact(DisplayName = "Empty Init")]
    public void Empty_Init()
    {
        string output = Utils.RunCode("""
                                      let i = 0;
                                      for (;i < 3; i++) 
                                      {
                                        print i;
                                      }
                                      print true;
                                      """);

        Assert.Equal("0\n1\n2\ntrue\n", output);
    }

    [Fact(DisplayName = "Empty Condition")]
    public void Empty_Condition()
    {
        string output = Utils.RunCode("""
                                      for (let i = 0;; i++) 
                                      {
                                        print i;
                                        
                                        if (i == 2)
                                        {
                                            break;
                                        }
                                      }
                                      print true;
                                      """);

        Assert.Equal("0\n1\n2\ntrue\n", output);
    }

    [Fact(DisplayName = "Empty Update")]
    public void Empty_Update()
    {
        string output = Utils.RunCode(""""
                                      for (let i = 0; i < 3;) 
                                      {
                                        print i;
                                        
                                        i++;
                                      }
                                      print true;
                                      """");

        Assert.Equal("0\n1\n2\ntrue\n", output);
    }

    [Fact(DisplayName = "Cannot Redeclare Init Variable")]
    public void Cannot_Redeclare_Init_Variable()
    {
        InvalidSyntaxException ex = Assert.Throws<InvalidSyntaxException>(() => Utils.RunCode("""
            for (let i = 0; i < 3;) 
            {
                let i = 0;
            }
            """));

        Assert.Equal("Variable `i` already exists.", ex.Message);
        Assert.Equal(3, ex.LineNumber);
    }
}