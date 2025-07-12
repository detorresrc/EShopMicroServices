using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace Basket.API.Data;

public class CacheDBasketRepository(
    IBasketRepository basketRepository,
    IDistributedCache cache) 
    : IBasketRepository
{
    public async Task<ShoppingCart> GetBasketAsync(string userName, CancellationToken cancellationToken = default)
    {
        var cachedBasket = await cache.GetStringAsync(userName, cancellationToken);
        if (!string.IsNullOrEmpty(cachedBasket))
        {
            var cacheBasketObject = JsonSerializer.Deserialize<ShoppingCart>(cachedBasket);
            if (cacheBasketObject is not null)
                return cacheBasketObject;
        }
        
        var freshBasket = await basketRepository.GetBasketAsync(userName, cancellationToken);
        await cache.SetStringAsync(userName, JsonSerializer.Serialize(freshBasket), cancellationToken);

        return freshBasket;
    }

    public async Task<ShoppingCart> StoreBasketAsync(ShoppingCart shoppingCart,
        CancellationToken cancellationToken = default)
    {
        var savedShoppingCart = await basketRepository.StoreBasketAsync(shoppingCart, cancellationToken);
        await cache.SetStringAsync(savedShoppingCart.Username, JsonSerializer.Serialize(savedShoppingCart),
            cancellationToken);

        return savedShoppingCart;
    }

    public async Task<bool> DeleteBasketAsync(string userName, CancellationToken cancellationToken = default)
    {
        await cache.RemoveAsync(userName, cancellationToken);
        return await basketRepository.DeleteBasketAsync(userName, cancellationToken);
    }
}