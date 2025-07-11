namespace Catalog.API.Products.GetProducts;

public sealed record GetProductRequest(
    int? Page = 1,
    int? PageSize = 10);
public sealed record GetProductsResponse(
    IEnumerable<Product> Products,
    long Count,
    long PageNumber,
    long PageSize,
    long PageCount,
    long TotalItemCount,
    bool HasPreviousPage,
    bool HasNextPage,
    bool IsFirstPage,
    long FirstItemOnPage,
    long LastItemOnPage);

public sealed class GetProductsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products", 
            async ([AsParameters] GetProductRequest request, ISender sender, CancellationToken cancellationToken) =>
            {
                var query = request.Adapt<GetProductsQuery>();
                
                var result = await sender.Send(query, cancellationToken);
                return Results.Ok(result.Adapt<GetProductsResponse>());
            })
            .WithName("GetProducts")
            .Produces<GetProductsResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Products")
            .WithDescription("Retrieve a list of products from the catalog.");
    }
}