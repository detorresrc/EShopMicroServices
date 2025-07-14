namespace Ordering.API.Endpoints;

public record DeleteOrderResponse(bool IsSuccess);

public sealed class DeleteOrder : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/orders/{orderId:guid}",
                async (Guid orderId, ISender sender, CancellationToken cancellationToken) =>
                {
                    var response = await sender.Send(DeleteOrderCommand.Of(orderId), cancellationToken);
                    return Results.Ok(new DeleteOrderResponse(response.IsSuccess));
                }
            )
            .WithName("DeleteOrder")
            .Produces<DeleteOrderResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Delete Order")
            .WithDescription("Delete Order");
        ;
    }
}