namespace Catalog.API.Products.GetProductById;

public sealed record GetProductByIdResponse(
    Product Product);

public sealed class GetProductByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products/{id:guid}", 
            async (Guid id, ISender sender, CancellationToken cancellationToken) =>
            {
                var result = await sender.Send(new GetProductByIdQuery(id), cancellationToken);
                return result.IsFailure 
                    ? Results.Problem(result.Error.Message, statusCode: StatusCodes.Status404NotFound) 
                    : Results.Ok(result.Value.Adapt<GetProductByIdResponse>());
            })
            .WithName("GetProductById")
            .Produces<GetProductByIdResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Product by Id")
            .WithDescription("Retrieves a product by its unique identifier.");
    }
}