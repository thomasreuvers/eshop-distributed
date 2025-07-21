using Catalog.Data;
using Catalog.Domain;

namespace Catalog.Services;

public class ProductService(ProductDbContext dbContext)
{
    public async Task CreateProductAsync(Product product)
    {
        dbContext.Products.Add(product);
        await dbContext.SaveChangesAsync();
    }
}