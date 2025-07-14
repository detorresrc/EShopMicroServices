namespace Ordering.API.Endpoints;

public record GetOrdersByCustomerResponse(IEnumerable<OrderDto> Orders);

public sealed class GetOrdersByCustomer : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/orders/customer/{customerId:guid}",
                async (Guid customerId, ISender sender, CancellationToken cancellationToken) =>
                {
                    var result = await sender.Send(GetOrdersByCustomerQuery.Of(customerId), cancellationToken);
                    return Results.Ok(result.Adapt<GetOrdersByCustomerResponse>());
                })
            .WithName("GetOrdersByCustomer")
            .Produces<GetOrdersResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Get Orders by Customer")
            .WithDescription("Get Orders by Customer");
    }
}