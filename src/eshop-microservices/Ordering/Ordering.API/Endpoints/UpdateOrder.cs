namespace Ordering.API.Endpoints;

public record UpdateOrderRequest(OrderDto Order);

public record UpdateOrderResponse(bool IsSuccess);

public sealed class UpdateOrder : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/orders",
                async (UpdateOrderRequest request, ISender sender, CancellationToken cancellationToken) =>
                {
                    var response = await sender.Send(UpdateOrderCommand.Of(request.Order), cancellationToken);
                    return Results.Ok(new UpdateOrderResponse(response.IsSuccess));
                })
            .WithName("UpdateOrder")
            .Produces<UpdateOrderResponse>()
            .Produces<CustomErrorDetails>(StatusCodes.Status422UnprocessableEntity)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Update Order")
            .WithDescription("Update Order");
        ;
    }
}