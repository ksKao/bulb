using Bulb.Exceptions;

namespace Test;

public class AssignmentTest
{
    [Fact(DisplayName = "Cannot Assign To Non-Declared Variable")]
    public void Cannot_Assign_To_Non_Declared_Variable()
    {
        InvalidSyntaxException ex = Assert.Throws<InvalidSyntaxException>(() => Utils.RunCode("""
            let x = 10;
            z = 7;
            print z;
            """));

        Assert.Equal("Variable `z` is not defined.", ex.Message);
        Assert.Equal(2, ex.LineNumber);
    }

    [Fact(DisplayName = "Assignment With Expression")]
    public void Assignment_With_Expression()
    {
        string output = Utils.RunCode("""
                                        let x = 10;
                                        x = 5 + 10 / 5;
                                        print x;
                                      """);

        Assert.Equal("7\n", output);
    }

    [Fact(DisplayName = "Assignment With Variable")]
    public void Assignment_With_Variable()
    {
        string output = Utils.RunCode("""
                                        let x = 10;
                                        let y = 20;
                                        x = y + 5;
                                        print x;
                                        print y;
                                      """);

        Assert.Equal("25\n20\n", output);
    }

    [Fact(DisplayName = "Assignment With Itself")]
    public void Assignment_With_Itself()
    {
        string output = Utils.RunCode("""
                                      let x = 10;
                                      x = x + x;
                                      print x;
                                      """);

        Assert.Equal("20\n", output);
    }

    [Fact(DisplayName = "Cannot Assign Different Type")]
    public void Cannot_Assign_Different_Type()
    {
        InvalidSyntaxException ex = Assert.Throws<InvalidSyntaxException>(() => Utils.RunCode("""
            let x = 10;
            x = true;
            """));

        Assert.Equal("Unable to assign bool to number.", ex.Message);
        Assert.Equal(2, ex.LineNumber);
    }

    [Fact(DisplayName = "Cannot Assign Void Type")]
    public void Cannot_Assign_Void_Type()
    {
        InvalidSyntaxException ex = Assert.Throws<InvalidSyntaxException>(() => Utils.RunCode("""
            function test() : void {}

            let x = 10;
            x = test();
            """));

        Assert.Equal("Unable to assign void to number.", ex.Message);
        Assert.Equal(4, ex.LineNumber);
    }

    [Fact(DisplayName = "Chain Assignment")]
    public void Chain_Assignment()
    {
        string output = Utils.RunCode("""
                                      let y = 0;
                                      let x = 0;
                                      x = y = 20;
                                      print x;
                                      print y;
                                      """);

        Assert.Equal("20\n20\n", output);
    }
}