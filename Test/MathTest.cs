namespace Test;

public class MathTest
{
    [Fact(DisplayName = "Add Two Numbers")]
    public void Add_Two_Numbers()
    {
        string output = Utils.RunCode("""
                                        print 5 + 3;
                                      """);

        Assert.Equal("8\n", output);
    }

    [Fact(DisplayName = "Subtract Two Numbers")]
    public void Subtract_Two_Numbers()
    {
        string output = Utils.RunCode("""
                                        print 5 - 3;
                                      """);

        Assert.Equal("2\n", output);
    }

    [Fact(DisplayName = "Subtract Two Numbers With Negative")]
    public void Subtract_Two_Numbers_With_Negative()
    {
        string output = Utils.RunCode("""
                                        print 3 - 5;
                                      """);

        Assert.Equal("-2\n", output);
    }

    [Fact(DisplayName = "Multiply Two Numbers")]
    public void Multiply_Two_Numbers()
    {
        string output = Utils.RunCode("""
                                        print 3 * 5;
                                      """);

        Assert.Equal("15\n", output);
    }

    [Fact(DisplayName = "Divide Two Numbers")]
    public void Divide_Two_Numbers()
    {
        string output = Utils.RunCode("""
                                        print 10 / 5;
                                      """);

        Assert.Equal("2\n", output);
    }

    [Fact(DisplayName = "Divide Two Numbers With Decimal")]
    public void Divide_Two_Numbers_With_Decimal()
    {
        string output = Utils.RunCode("""
                                        print 5 / 10;
                                      """);

        Assert.Equal("0.5\n", output);
    }

    [Fact(DisplayName = "Add Three Numbers")]
    public void Add_Three_Numbers()
    {
        string output = Utils.RunCode("""
                                      print 1 + 2 + 3;
                                      """);

        Assert.Equal("6\n", output);
    }

    [Fact(DisplayName = "Operator Precedence")]
    public void Operator_Precedence()
    {
        string output = Utils.RunCode("""
                                        print 1 + 2 / 4 - 5 * 6;
                                      """);

        Assert.Equal("-28.5\n", output);
    }

    [Fact(DisplayName = "Operator Precedence With Parentheses")]
    public void Operator_Precedence_With_Parentheses()
    {
        string output = Utils.RunCode("""
                                      print 5 + 5 / (10 + 10);
                                      """);

        Assert.Equal("5.25\n", output);
    }
}