using Catalog.Domain;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Data;

public class ProductDbContext(DbContextOptions<ProductDbContext> options)
    : DbContext(options)
{
    public DbSet<Product> Products => Set<Product>();
}