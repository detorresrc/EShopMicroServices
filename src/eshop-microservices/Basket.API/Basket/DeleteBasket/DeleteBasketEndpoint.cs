using Mapster;

namespace Basket.API.Basket.DeleteBasket;

public sealed record DeleteBasketResponse(bool IsSuccess);

public sealed class DeleteBasketEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/basket/{username}",
                async (string username, ISender sender, CancellationToken cancellationToken) =>
                {
                    var command = new DeleteBasketCommand(username);
                    var result = await sender.Send(command, cancellationToken);
                    var response = result.Adapt<DeleteBasketResponse>();

                    return Results.Ok(response);
                }
            )
            .WithName("DeleteBasket")
            .Produces<DeleteBasketResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Delete Basket")
            .WithDescription("Delete Basket");
    }
}