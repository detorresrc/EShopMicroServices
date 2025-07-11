using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Products.DeleteProduct;

public class DeleteProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/products/{id:guid}",
                async (
                    Guid id,
                    ISender sender,
                    CancellationToken cancellationToken) =>
                {
                    var result = await sender.Send(new DeleteProductCommand(id), cancellationToken);

                    if (result.IsFailure)
                        return Results.Problem(result.Error.Message, statusCode: StatusCodes.Status400BadRequest);

                    return Results.NoContent();
                })
            .WithName("DeleteProduct")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Update Product")
            .WithDescription("Update a new product in the catalog.");
    }
}