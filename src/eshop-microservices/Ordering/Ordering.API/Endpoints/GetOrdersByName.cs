namespace Ordering.API.Endpoints;

public record GetOrdersByNameResponse(IEnumerable<OrderDto> Orders);

public sealed class GetOrdersByName : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/orders/name/{name}",
                async (string name, ISender sender, CancellationToken cancellationToken) =>
                {
                    var result = await sender.Send(GetOrdersByNameQuery.Of(name), cancellationToken);
                    return Results.Ok(result.Adapt<GetOrdersByNameResponse>());
                })
            .WithName("GetOrdersByName")
            .Produces<GetOrdersResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Get Orders by Name")
            .WithDescription("Get Orders by Name");
    }
}