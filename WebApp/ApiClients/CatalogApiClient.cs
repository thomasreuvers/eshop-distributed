using Catalog.Domain;

namespace WebApp.ApiClients;

public class CatalogApiClient(HttpClient httpClient)
{
    public async Task<List<Product>> GetProductsAsync()
    {
        var response = await httpClient.GetFromJsonAsync<List<Product>>("/products");
        return response ?? new List<Product>();
    }
    
    public async Task<Product?> GetProductByIdAsync(Guid productId)
    {
        var response = await httpClient.GetFromJsonAsync<Product>($"/products/{productId}");
        return response;
    }
}