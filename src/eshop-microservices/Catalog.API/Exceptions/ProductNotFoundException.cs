namespace Catalog.API.Exceptions;

public class ProductNotFoundException : Exception
{
    public ProductNotFoundException(Guid id)
        : base($"Product with id {id} not found.")
    {
    }

    public ProductNotFoundException(string name)
        : base($"Product with name {name} not found.")
    {
    }
}