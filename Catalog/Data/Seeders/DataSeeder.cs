using Bogus;
using Catalog.Domain;

namespace Catalog.Data.Seeders;

public class DataSeeder
{
    public static void Seed(ProductDbContext dbContext)
    {
        if (dbContext.Products.Any())
        {
            return;
        }
        
        dbContext.Products.AddRange(Products);
        dbContext.SaveChanges();
    }

    private static IEnumerable<Product> Products => new ProductFaker().Generate(10);
}

public sealed class ProductFaker : Faker<Product>
{
    public ProductFaker()
    {
        RuleFor(p => p.Name, f => f.Commerce.ProductName());
        RuleFor(p => p.Description, f => f.Commerce.ProductDescription());
        RuleFor(p => p.Price, f => decimal.Parse(f.Commerce.Price()));
        RuleFor(p => p.ImageUrl, f => f.Image.PicsumUrl());
    }
}