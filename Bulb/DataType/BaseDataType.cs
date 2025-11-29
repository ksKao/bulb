using System.Globalization;

using Bulb.Enums;
using Bulb.Exceptions;
using Bulb.Node;

namespace Bulb.DataType;

public abstract class BaseDataType(string name) : IEquatable<BaseDataType>
{
    public static readonly BuiltInDataType Number = new("number", [], [
        (new FunctionDeclarationStatement(new Token(TokenType.Identifier, "toString"), [], new Token(TokenType.Identifier, "string"), new Scope()),
            runner =>
            {
                double value = (double)runner.Stack.Last();
                runner.Stack.Add(value.ToString(CultureInfo.InvariantCulture));
            }),
        (new FunctionDeclarationStatement(new Token(TokenType.Identifier, "toFixed"), [(new Token(TokenType.Identifier, "a"), new Token(TokenType.Identifier, "number"))], new Token(TokenType.Identifier, "string"), new Scope()),
            runner =>
            {
                double decimalPlaces = (double)runner.Stack.Pop();
                double value = (double)runner.Stack.Last();
                runner.Stack.Add(value.ToString($"N{Convert.ToInt32(decimalPlaces)}"));
            })
    ]);

    public static readonly BuiltInDataType Boolean = new("bool", [], []);

    public static readonly BuiltInDataType String = new("string",
        new Dictionary<string, (string returnType, Action<Runner> action)>
        {
            {
                "length", ("number", runner =>
                {
                    string value = (string)runner.Stack.Last();
                    runner.Stack.Add((double)value.Length);
                })
            }
        }, [
            (new FunctionDeclarationStatement(new Token(TokenType.Identifier, "isNumber"), [], new Token(TokenType.Identifier, "bool"), new Scope()),
                runner =>
                {
                    string value = (string)runner.Stack.Last();
                    bool isNumber = double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out _);
                    runner.Stack.Add(isNumber);
                }),
            (new FunctionDeclarationStatement(new Token(TokenType.Identifier, "toNumber"), [], new Token(TokenType.Identifier, "number"), new Scope()),
                runner =>
                {
                    string value = (string)runner.Stack.Last();

                    if (double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out double number))
                    {
                        runner.Stack.Add(number);
                    }
                    else
                    {
                        throw new RuntimeException($"Unable to convert `{value}` to a number.");
                    }
                }),
            (new FunctionDeclarationStatement(new Token(TokenType.Identifier, "toUpper"), [], new Token(TokenType.Identifier, "string"), new Scope()),
                runner =>
                {
                    string value = (string)runner.Stack.Last();

                    runner.Stack.Add(value.ToUpper());
                }),
            (new FunctionDeclarationStatement(new Token(TokenType.Identifier, "toLower"), [], new Token(TokenType.Identifier, "string"), new Scope()),
                runner =>
                {
                    string value = (string)runner.Stack.Last();

                    runner.Stack.Add(value.ToLower());
                }),
            (new FunctionDeclarationStatement(new Token(TokenType.Identifier, "charAt"), [(new Token(TokenType.Identifier, "a"), new Token(TokenType.Identifier, "number"))], new Token(TokenType.Identifier, "string"), new Scope()),
                runner =>
                {
                    double index = (double)runner.Stack.Pop();
                    string value = (string)runner.Stack.Last();

                    if (index < 0)
                    {
                        throw new RuntimeException("`charAt` index cannot be less than 0.");
                    }

                    if (index >= value.Length)
                    {
                        throw new RuntimeException("`charAt` index cannot be more than or equal to the string length.");
                    }

                    runner.Stack.Add(value[Convert.ToInt32(index)].ToString());
                })
        ]);

    public static readonly BuiltInDataType Void = new("void", [], []);

    public string Name { get; } = name;

    public bool Equals(BaseDataType? other)
    {
        return Name == other?.Name;
    }

    public static bool operator ==(BaseDataType left, BaseDataType right)
    {
        return left.Name == right.Name;
    }

    public static bool operator !=(BaseDataType left, BaseDataType right)
    {
        return !(left == right);
    }

    public override bool Equals(object? obj)
    {
        return obj is BaseDataType other && Equals(other);
    }

    public override int GetHashCode()
    {
        return Name.GetHashCode();
    }

    public override string ToString()
    {
        return Name;
    }
}