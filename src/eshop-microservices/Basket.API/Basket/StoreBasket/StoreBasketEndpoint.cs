using Mapster;

namespace Basket.API.Basket.StoreBasket;

public sealed record StoreBasketRequest(ShoppingCart Cart);

public sealed record StoreBasketResponse(ShoppingCart Cart);

public class StoreBasketEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/basket",
                async (StoreBasketRequest request, ISender sender, CancellationToken cancellationToken) =>
                {
                    var command = request.Adapt<StoreBasketCommand>();
                    var result = await sender.Send(command, cancellationToken);
                    var response = result.Adapt<StoreBasketResponse>();

                    return Results.Created($"/basket/{request.Cart.Username}", response);
                })
            .WithName("CreateUpdateBasket")
            .Produces<StoreBasketResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Create/Update Basket")
            .WithDescription("Create/Update Basket");
    }
}