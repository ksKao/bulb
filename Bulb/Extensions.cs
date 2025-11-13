namespace Bulb;

public static class Extensions
{
    public static T Pop<T>(this List<T> list)
    {
        T lastElement = list[^1];
        list.RemoveAt(list.Count - 1);
        return lastElement;
    }
}