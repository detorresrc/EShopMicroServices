namespace Ordering.API.Endpoints;

public record GetOrdersResponse(PaginatedResult<OrderDto> Orders);

public sealed class GetOrders : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/orders",
                async ([AsParameters] PaginationRequest request, ISender sender, CancellationToken cancellationToken) =>
                {
                    var result = await sender.Send(new GetOrdersQuery(request), cancellationToken);

                    return Results.Ok(result.Adapt<GetOrdersResponse>());
                })
            .WithName("GetOrders")
            .Produces<GetOrdersResponse>()
            .Produces<CustomErrorDetails>(StatusCodes.Status422UnprocessableEntity)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Get Orders")
            .WithDescription("Get Orders");
    }
}