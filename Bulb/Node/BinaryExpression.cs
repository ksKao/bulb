using Bulb.Enums;
using Bulb.Exceptions;

namespace Bulb.Node;

public class BinaryExpression(Token operatorToken, Expression left, Expression right) : Expression
{
    private Token OperatorToken { get; } = operatorToken;
    private Expression Left { get; } = left;
    private Expression Right { get; } = right;

    private bool IsBooleanOperator => OperatorToken.Type is TokenType.And or TokenType.Or;

    private bool IsMathOperator =>
        OperatorToken.Type is TokenType.Plus or TokenType.Minus or TokenType.Multiply or TokenType.Divide;

    private bool IsString => Left.DataType == DataType.String || Right.DataType == DataType.String;

    private bool IsComparisonOperator => OperatorToken.Type is TokenType.DoubleEqual or TokenType.NotEqual
        or TokenType.GreaterThan or TokenType.GreaterThanOrEqual or TokenType.LessThan or TokenType.LessThanOrEqual;

    public override DataType DataType { get; protected set; }

    public override void Run(Runner runner)
    {
        Left.Run(runner);
        Right.Run(runner);

        ValidateType();

        if (IsString && OperatorToken.Type == TokenType.Plus)
        {
            string? rightValue = runner.Stack.Pop().ToString();
            string? leftValue = runner.Stack.Pop().ToString();

            DataType = DataType.String;

            if (rightValue is null || leftValue is null)
            {
                throw new InvalidSyntaxException("Unexpected string null value", OperatorToken.LineNumber);
            }

            if (Left.DataType == DataType.Boolean)
            {
                leftValue = leftValue.ToLower();
            }

            if (Right.DataType == DataType.Boolean)
            {
                rightValue = rightValue.ToLower();
            }

            runner.Stack.Add(leftValue + rightValue);
        }
        else if (IsMathOperator)
        {
            double rightValue = (double)runner.Stack.Pop();
            double leftValue = (double)runner.Stack.Pop();

            DataType = DataType.Number;

            switch (OperatorToken.Type)
            {
                case TokenType.Plus:
                    runner.Stack.Add(leftValue + rightValue);
                    break;
                case TokenType.Minus:
                    runner.Stack.Add(leftValue - rightValue);
                    break;
                case TokenType.Multiply:
                    runner.Stack.Add(leftValue * rightValue);
                    break;
                case TokenType.Divide:
                    runner.Stack.Add(leftValue / rightValue);
                    break;
                default:
                    throw new InvalidSyntaxException(
                        $"Unexpected operator encountered in binary expression: `{OperatorToken.Value}`",
                        OperatorToken.LineNumber);
            }
        }
        else if (IsBooleanOperator)
        {
            bool rightValue = (bool)runner.Stack.Pop();
            bool leftValue = (bool)runner.Stack.Pop();

            DataType = DataType.Boolean;

            switch (OperatorToken.Type)
            {
                case TokenType.And:
                    runner.Stack.Add(leftValue && rightValue);
                    break;
                case TokenType.Or:
                    runner.Stack.Add(leftValue || rightValue);
                    break;
                default:
                    throw new InvalidSyntaxException(
                        $"Unexpected operator encountered in binary expression: `{OperatorToken.Value}`",
                        OperatorToken.LineNumber);
            }
        }
        else if (IsComparisonOperator)
        {
            DataType = DataType.Boolean;

            if (OperatorToken.Type is TokenType.DoubleEqual or TokenType.NotEqual)
            {
                object rightValue = runner.Stack.Pop();
                object leftValue = runner.Stack.Pop();

                switch (OperatorToken.Type)
                {
                    case TokenType.DoubleEqual:
                        // use .Equals here because of boxed value will default to compare by reference (https://stackoverflow.com/questions/814878/c-sharp-difference-between-and-equals)
                        runner.Stack.Add(leftValue.Equals(rightValue));
                        break;
                    case TokenType.NotEqual:
                        runner.Stack.Add(!leftValue.Equals(rightValue));
                        break;
                    default:
                        throw new InvalidSyntaxException(
                            $"Unexpected operator encountered in binary expression: `{OperatorToken.Value}`",
                            OperatorToken.LineNumber);
                }
            }
            else
            {
                double rightValue = (double)runner.Stack.Pop();
                double leftValue = (double)runner.Stack.Pop();

                switch (OperatorToken.Type)
                {
                    case TokenType.GreaterThanOrEqual:
                        runner.Stack.Add(leftValue >= rightValue);
                        break;
                    case TokenType.GreaterThan:
                        runner.Stack.Add(leftValue > rightValue);
                        break;
                    case TokenType.LessThanOrEqual:
                        runner.Stack.Add(leftValue <= rightValue);
                        break;
                    case TokenType.LessThan:
                        runner.Stack.Add(leftValue < rightValue);
                        break;
                    default:
                        throw new InvalidSyntaxException(
                            $"Unexpected operator encountered in binary expression: `{OperatorToken.Value}`",
                            OperatorToken.LineNumber);
                }
            }
        }
        else
        {
            throw new InvalidSyntaxException($"Unknown binary operator encountered (`{OperatorToken.Value}`)",
                OperatorToken.LineNumber);
        }
    }

    public override string ToString(string indent)
    {
        return $"""
                {indent} Binary Expression:
                {indent + "\t"} Operator: {OperatorToken.Value}
                {indent + "\t"} Left:
                {Left.ToString(indent + "\t\t")}
                {indent + "\t"} Right:
                {Right.ToString(indent + "\t\t")}
                """;
    }

    private void ValidateType()
    {
        if (Left.DataType != Right.DataType && !IsString)
        {
            throw new InvalidSyntaxException($"Unable to `{OperatorToken.Value}` {Left.DataType} and {Right.DataType}",
                OperatorToken.LineNumber);
        }

        if (IsString && !IsComparisonOperator && OperatorToken.Type != TokenType.Plus)
        {
            throw new InvalidSyntaxException($"Unable to `{OperatorToken.Value}` {Left.DataType} and {Right.DataType}",
                OperatorToken.LineNumber);
        }

        // from here on assume and left and right is the same, so we only check for the left data type
        if (Left.DataType == DataType.Number && !IsMathOperator && !IsComparisonOperator && !IsString)
        {
            throw new InvalidSyntaxException($"Unable to `{OperatorToken.Value}` {Left.DataType} and {Right.DataType}",
                OperatorToken.LineNumber);
        }

        if (Left.DataType == DataType.Boolean && !IsBooleanOperator && !IsComparisonOperator && !IsString)
        {
            throw new InvalidSyntaxException($"Unable to `{OperatorToken.Value}` {Left.DataType} and {Right.DataType}",
                OperatorToken.LineNumber);
        }
    }
}