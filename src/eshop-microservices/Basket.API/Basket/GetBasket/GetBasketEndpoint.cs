using Mapster;

namespace Basket.API.Basket.GetBasket;

public sealed record GetBasketResponse(ShoppingCart Cart);

public sealed class GetBasketEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/basket/{username}",
            async (string username, ISender sender, CancellationToken cancellationToken = default) =>
            {
                var result = await sender.Send(new GetBasketQuery(username), cancellationToken);

                return Results.Ok(result.Adapt<GetBasketResponse>());
            })
            .WithName("GetBasket")
            .Produces<GetBasketResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Get Basket by Username")
            .WithDescription("Get Basket by Username");;
    }
}