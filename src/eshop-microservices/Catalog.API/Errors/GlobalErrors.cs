using SharedKernel;

namespace Catalog.API.Errors;

public static class GlobalErrors
{
    public static readonly Error ProductNotFound = new("Error.ProductNotFound", "The specified product was not found.");
}