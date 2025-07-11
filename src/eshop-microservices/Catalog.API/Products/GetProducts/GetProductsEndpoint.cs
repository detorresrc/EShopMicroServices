namespace Catalog.API.Products.GetProducts;

public sealed record GetProductsResponse(
    IEnumerable<Product> Products);

public sealed class GetProductsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products", 
            async (ISender sender, CancellationToken cancellationToken) =>
            {
                var result = await sender.Send(new GetProductsQuery(), cancellationToken);
                return Results.Ok(result.Adapt<GetProductsResponse>());
            })
            .WithName("GetProducts")
            .Produces<GetProductsResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Products")
            .WithDescription("Retrieve a list of products from the catalog.");
    }
}