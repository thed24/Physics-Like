public class Maybe<T>
{
    public T Value { get; private set; }
    public bool HasValue { get; private set; }

    private Maybe(T value)
    {
        Value = value;
        HasValue = true;
    }

    private Maybe()
    {
        HasValue = false;
    }

    public static Maybe<T> Some(T value)
    {
        return new Maybe<T>(value);
    }

    public static Maybe<T> None()
    {
        return new Maybe<T>();
    }
}