public class Tuple<T, U>
{
    public T First { get; private set; }
    public U Second { get; private set; }

    public Tuple(T first, U second)
    {
        First = first;
        Second = second;
    }
}

public static class Tuple
{
    public static Tuple<T, U> Create<T, U>(T First, U Second)
    {
        return new Tuple<T, U>(First, Second);
    }
}