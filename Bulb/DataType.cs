namespace Bulb;

public readonly struct DataType(string name) : IEquatable<DataType>
{
    public static DataType Number { get; } = new("number");
    public static DataType Boolean { get; } = new("bool");
    public static DataType String { get; } = new("string");

    public string Name { get; } = name;

    public static bool operator ==(DataType left, DataType right)
    {
        return left.Name == right.Name;
    }

    public static bool operator !=(DataType left, DataType right)
    {
        return !(left == right);
    }

    public bool Equals(DataType other)
    {
        return Name == other.Name;
    }

    public override bool Equals(object? obj)
    {
        return obj is DataType other && Equals(other);
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