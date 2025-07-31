namespace Ordering.Domain.ValueObjects;

public record OrderName
{
    private const int DefaultLength = 50;

    private OrderName(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static OrderName Of(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(name.Length, DefaultLength);

        return new OrderName(name);
    }
}