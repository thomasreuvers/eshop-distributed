using System.Text.Json;
using Basket.Models;
using Microsoft.Extensions.Caching.Distributed;

namespace Basket.Services;

public class BasketService(IDistributedCache cache)
{
    public async Task<ShoppingCart?> GetBasketAsync(string userName)
    {
        var basket = await cache.GetStringAsync(userName);
        return string.IsNullOrEmpty(basket) 
            ? null 
            : JsonSerializer.Deserialize<ShoppingCart>(basket);
    }
    
    public async Task UpdateBasketAsync(ShoppingCart basket)
    {
        var serializedBasket = JsonSerializer.Serialize(basket);
        await cache.SetStringAsync(basket.UserName, serializedBasket);
    }
    
    public async Task DeleteBasketAsync(string userName)
    {
        await cache.RemoveAsync(userName);
    }
}