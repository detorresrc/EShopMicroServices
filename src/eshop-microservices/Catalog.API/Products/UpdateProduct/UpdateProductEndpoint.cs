namespace Catalog.API.Products.UpdateProduct;

public sealed record UpdateProductRequest(
    string Name,
    List<string> Category,
    string Description,
    string ImageFile,
    decimal Price);

public sealed record UpdateProductResponse(
    Product Product);

public class UpdateProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/products/{id:guid}",
                async (
                    Guid id,
                    UpdateProductRequest request,
                    ISender sender,
                    CancellationToken cancellationToken) =>
                {
                    var product = request.Adapt<Product>();
                    var result = await sender.Send(new UpdateProductCommand(Id: id, Product: product),cancellationToken);

                    if (result.IsFailure)
                        return Results.Problem(result.Error.Message, statusCode: StatusCodes.Status400BadRequest);

                    var response = result.Value.Adapt<UpdateProductResponse>();
                    return Results.Ok(response);
                })
            .WithName("UpdateProduct")
            .Produces<UpdateProductResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Update Product")
            .WithDescription("Update a new product in the catalog.");
    }
}