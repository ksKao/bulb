using Bulb.Exceptions;

namespace Test;

public class FunctionTest
{
    [Fact(DisplayName = "Basic Function")]
    public void Basic_Function()
    {
        string output = Utils.RunCode("""
                                      function test() : void
                                      {
                                        print 10;
                                      }
                                      test();
                                      """);

        Assert.Equal("10\n", output);
    }

    [Fact(DisplayName = "Basic Function With Return Type")]
    public void Basic_Function_With_Return_Type()
    {
        string output = Utils.RunCode("""
                                      function test() : number
                                      {
                                        return 10;
                                      }
                                      print test();
                                      """);

        Assert.Equal("10\n", output);
    }

    [Fact(DisplayName = "Void Function With Conditional Return")]
    public void Void_Function_With_Conditional_Return()
    {
        string output = Utils.RunCode("""
                                      let a = 10;

                                      function test() : void
                                      {
                                        if (true)
                                        {
                                            return;
                                        }
                                        
                                        a = 10;
                                      }

                                      test();
                                      print a;
                                      """);

        Assert.Equal("10\n", output);
    }

    [Fact(DisplayName = "Function With Loop")]
    public void Function_With_Loop()
    {
        string output = Utils.RunCode("""
                                      let a = 10;

                                      function test() : void
                                      {
                                        for (let i = 0; i < 5; i++)
                                        {
                                            a++;
                                        }
                                      }

                                      test();
                                      print a;
                                      """);

        Assert.Equal("15\n", output);
    }

    [Fact(DisplayName = "Function With Arguments")]
    public void Function_With_Arguments()
    {
        string output = Utils.RunCode("""
                                      function test(a: bool, b: number, c: string) : void
                                      {
                                        print a;
                                        print b;
                                        print c;
                                      }

                                      test(true, 1.3, "hello");
                                      """);

        Assert.Equal("true\n1.3\nhello\n", output);
    }

    [Fact(DisplayName = "Function Overloading")]
    public void Function_Overloading()
    {
        string output = Utils.RunCode("""
                                      function test(a: number) : void
                                      {
                                        print a + " number";
                                      }

                                      function test(a: bool) : void
                                      {
                                        print a + " bool";
                                      }

                                      test(1);
                                      test(true);
                                      """);

        Assert.Equal("1 number\ntrue bool\n", output);
    }

    [Fact(DisplayName = "Function Parameter Shadow Variable")]
    public void Function_Parameter_Shadow_Variable()
    {
        string output = Utils.RunCode("""
                                      let a = 10;

                                      function test(a: number): void
                                      {
                                        print a;
                                      }

                                      test(3);
                                      """);

        Assert.Equal("3\n", output);
    }

    [Fact(DisplayName = "Function Nesting")]
    public void Function_Nesting()
    {
        string output = Utils.RunCode("""
                                      function decrementSum(a: number, b: number): number
                                      {
                                        function sum(a: number, b: number): number
                                        {
                                            return a + b;
                                        }
                                        
                                        return sum(a - 1, b);
                                      }

                                      print decrementSum(1, 2);
                                      """);

        Assert.Equal("2\n", output);
    }

    [Fact(DisplayName = "Cannot Call Nested Function")]
    public void Cannot_Call_Nested_Function()
    {
        InvalidSyntaxException ex = Assert.Throws<InvalidSyntaxException>(() => Utils.RunCode("""
            function decrementSum(a: number, b: number): number
            {
              function sum(a: number, b: number): number
              {
                  return a + b;
              }
              
              return sum(a, b) - 1;
            }

            print sum(1, 2);
            """));

        Assert.Equal("A function named `sum` that takes in number, number does not exist.", ex.Message);
        Assert.Equal(11, ex.LineNumber);
    }

    [Fact(DisplayName = "Non-Void Function Must Return")]
    public void Non_Void_Function_Must_Return()
    {
        InvalidSyntaxException ex = Assert.Throws<InvalidSyntaxException>(() => Utils.RunCode("""
            function test(): number
            {
                print "hello";
            }

            test();
            """));

        Assert.Equal("`test` function is missing a return statement.", ex.Message);
        Assert.Equal(1, ex.LineNumber);
    }

    [Fact(DisplayName = "Recursive Function")]
    public void Recursive_Function()
    {
        string output = Utils.RunCode("""
                                      function test(a: number): void
                                      {
                                          if (a == 0)
                                          {
                                              return;
                                          }
                                          
                                          test(a - 1);
                                          print a;
                                      }

                                      test(5);
                                      """);

        Assert.Equal("1\n2\n3\n4\n5\n", output);
    }
}