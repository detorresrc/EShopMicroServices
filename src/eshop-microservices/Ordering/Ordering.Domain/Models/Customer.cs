namespace Ordering.Domain.Models;

public sealed class Customer : Entity<CustomerId>
{
    public string Name { get; private set; } = default!;
    public string Email { get; private set; } = default!;

    public static Customer Create(CustomerId id, string name, string email)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(email);

        Customer customer = new()
        {
            Id = id,
            Name = name,
            Email = email
        };

        return customer;
    }
}