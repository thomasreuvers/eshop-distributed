using Catalog.Domain;

namespace Basket.ApiClients;

public class CatalogApiClient(HttpClient httpClient)
{
    public async Task<Product> GetProductByIdAsync(Guid productId)
    {
        var response = await httpClient.GetFromJsonAsync<Product>($"/products/{productId}");
        return response!;
    }
}