using Catalog.Data;
using Catalog.Domain;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using ServiceDefaults.Messaging.Events;

namespace Catalog.Services;

public class ProductService(
    ProductDbContext dbContext,
    IBus bus)
{
    public async Task<IEnumerable<Product>> GetProductsAsync()
    {
        return await dbContext.Products.ToListAsync();
    }
    
    public async Task<Product?> GetProductByIdAsync(Guid id)
    {
        return await dbContext.Products
            .FindAsync(id);
    }
    
    public async Task CreateProductAsync(Product product)
    {
        dbContext.Products.Add(product);
        await dbContext.SaveChangesAsync();
    }

    public async Task UpdateProductAsync(Product updatedProduct, Product inputProduct)
    {
        // If price has changed, raise ProductPriceChanged integration event
        if (updatedProduct.Price != inputProduct.Price)
        {
            // Publish product price change integration event for update basket prices
            var integrationEvent = new ProductPriceChangedIntegrationEvent
            {
                ProductId = updatedProduct.Id, // Id only comes from db entity
                Name = inputProduct.Name,
                Description = inputProduct.Description,
                Price = inputProduct.Price, // Set updated product price
                ImageUrl = inputProduct.ImageUrl
            };

            await bus.Publish(integrationEvent);
        }
        
        updatedProduct.Name = inputProduct.Name;
        updatedProduct.Description = inputProduct.Description;
        updatedProduct.Price = inputProduct.Price;
        updatedProduct.ImageUrl = inputProduct.ImageUrl;
        
        dbContext.Products.Update(updatedProduct);
        await dbContext.SaveChangesAsync();
    }
    
    public async Task DeleteProductAsync(Product product)
    {
        dbContext.Products.Remove(product);
        await dbContext.SaveChangesAsync();
    }
}