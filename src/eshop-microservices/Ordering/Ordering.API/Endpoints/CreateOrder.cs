namespace Ordering.API.Endpoints;

public record CreateOrderRequest(OrderDto Order);

public record CreateOrderResponse(Guid Id);

public sealed class CreateOrder : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/orders",
                async (CreateOrderRequest request, ISender sender, CancellationToken cancellationToken) =>
                {
                    var result = await sender.Send(CreateOrderCommand.Of(request.Order), cancellationToken);

                    return Results.Created($"/orders/{result.Id}", new CreateOrderResponse(result.Id));
                })
            .WithName("CreateOrder")
            .Produces<CreateOrderResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Create Order")
            .WithDescription("Create Order");
    }
}