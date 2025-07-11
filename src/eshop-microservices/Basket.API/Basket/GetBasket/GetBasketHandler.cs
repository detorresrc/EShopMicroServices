namespace Basket.API.Basket.GetBasket;

public sealed record GetBasketQuery(string UserName) : IQuery<GetBasketResult>;

public sealed record GetBasketResult(ShoppingCart Cart);

public sealed class GetBasketHandler(IBasketRepository basketRepository)
    : IQueryHandler<GetBasketQuery, GetBasketResult>
{
    public async Task<GetBasketResult> Handle(GetBasketQuery request, CancellationToken cancellationToken)
    {
        return new GetBasketResult(await basketRepository.GetBasketAsync(userName: request.UserName, cancellationToken));
    }
}