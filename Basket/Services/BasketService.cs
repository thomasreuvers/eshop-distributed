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

    internal async Task UpdateBasketItemProductPrices(Guid productid, decimal price)
    {
        // IDistributedCache not supported list of keys function
        // https://github.com/dotnet/runtime/issues/36402

        var basket = await GetBasketAsync("swn");
        var item = basket!.Items.FirstOrDefault(x => x.ProductId == productid);
        if (item != null)
        {
            item.Price = price;
            await cache.SetStringAsync(basket.UserName, JsonSerializer.Serialize(basket));
        }
    }
}