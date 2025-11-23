namespace Test;

public class CommentTest
{
    [Fact(DisplayName = "Ignore Comment")]
    public void Ignore_Comment()
    {
        string output = Utils.RunCode("""
                                      // this is a comment
                                      print 10;
                                      """);

        Assert.Equal("10\n", output);
    }

    [Fact(DisplayName = "Ignore Same Line Comment")]
    public void Ignore_Same_Line_Comment()
    {
        string output = Utils.RunCode("""
                                      print 10; // this is a comment
                                      print 20;
                                      """);

        Assert.Equal("10\n20\n", output);
    }
}