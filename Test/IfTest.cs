using Bulb.Exceptions;

namespace Test;

public class IfTest
{
    [Fact(DisplayName = "If With Literal Condition")]
    public void If_With_Literal_Condition()
    {
        string output = Utils.RunCode("""
                                      if (true) { print 10; }
                                      """);

        Assert.Equal("10\n", output);
    }

    [Fact(DisplayName = "If With Expression Condition")]
    public void If_With_Expression_Condition()
    {
        string output = Utils.RunCode("""
                                      if (5 + 2 > 2) { print 10; }
                                      """);

        Assert.Equal("10\n", output);
    }

    [Fact(DisplayName = "If With Else Condition")]
    public void If_With_Else_Condition()
    {
        string output = Utils.RunCode("""
                                      if (5 + 2 < 2) { print 10; }
                                      else { print 20; }
                                      """);

        Assert.Equal("20\n", output);
    }

    [Fact(DisplayName = "If With Else If And Else")]
    public void If_With_Else_If_And_Else()
    {
        string output = Utils.RunCode("""
                                      let x = true;

                                      if (5 + 2 < 2) { print 10; }
                                      else if (x) { print 20; }
                                      else { print 30; }
                                      """);

        Assert.Equal("20\n", output);
    }

    [Fact(DisplayName = "If With Multiple Else Ifs")]
    public void If_With_Multiple_Else_Ifs()
    {
        string output = Utils.RunCode("""
                                      let x = false;

                                      if (5 + 2 < 2) { print 10; }
                                      else if (x) { print 20; }
                                      else if (!x) { print 30; }
                                      else { print 40; }
                                      """);

        Assert.Equal("30\n", output);
    }

    [Fact(DisplayName = "Cannot If With Two Else's")]
    public void Cannot_If_With_Two_Elses()
    {
        InvalidSyntaxException ex = Assert.Throws<InvalidSyntaxException>(() => Utils.RunCode("""
            if (5 + 2 < 2) { print 10; }
            else { print 20; }
            else { print 30; }
            """));

        Assert.Equal("Unexpected token `else`", ex.Message);
        Assert.Equal(3, ex.LineNumber);
    }

    [Fact(DisplayName = "If Scope Works Normally")]
    public void If_Scope_Works_Normally()
    {
        string output = Utils.RunCode("""
                                      let x = false;

                                      if (true)
                                      {
                                          let y = true;
                                          
                                          print x;
                                          print y;
                                          
                                          x = true;
                                      }

                                      print x;
                                      """);

        Assert.Equal("false\ntrue\ntrue\n", output);
    }

    [Fact(DisplayName = "If Within If")]
    public void If_Within_If()
    {
        string output = Utils.RunCode("""
                                      if (true)
                                      {
                                          if (false) { print 10; }
                                          else
                                          {
                                            if (true) { print 20; }
                                            else { print 30; }
                                          }
                                      }
                                      """);

        Assert.Equal("20\n", output);
    }
}