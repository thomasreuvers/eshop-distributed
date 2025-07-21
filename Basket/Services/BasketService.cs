using System.Text.Json;
using Basket.ApiClients;
using Basket.Models;
using Microsoft.Extensions.Caching.Distributed;

namespace Basket.Services;

public class BasketService(
    IDistributedCache cache,
    CatalogApiClient catalogApiClient)
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
        foreach (var item in basket.Items)
        {
            var product = await catalogApiClient.GetProductByIdAsync(item.ProductId);
            item.Price = product.Price;
            item.ProductName = product.Name;
        }
        
        var serializedBasket = JsonSerializer.Serialize(basket);
        await cache.SetStringAsync(basket.UserName, serializedBasket);
    }
    
    public async Task DeleteBasketAsync(string userName)
    {
        await cache.RemoveAsync(userName);
    }
}