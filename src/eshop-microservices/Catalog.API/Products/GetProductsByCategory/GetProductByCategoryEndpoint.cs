namespace Catalog.API.Products.GetProductsByCategory;

public sealed record GetProductByCategoryResponse(
    IEnumerable<Product> Products);

public sealed class GetProductsByCategoryEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products/category/{category:required}", 
            async (string category, ISender sender, CancellationToken cancellationToken) =>
            {
                var result = await sender.Send(new GetProductsByCategoryQuery(category), cancellationToken);
                return Results.Ok(result.Adapt<GetProductByCategoryResponse>());
            })
            .WithName("GetProductsByCategory")
            .Produces<GetProductByCategoryResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Products by Category")
            .WithDescription("Get Products by Category");
    }
}