using Catalog.Data;
using Catalog.Data.Seeders;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Extensions;

public static class WebApplicationExtensions
{
    public static void UseMigration(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ProductDbContext>();
        dbContext.Database.Migrate();
        DataSeeder.Seed(dbContext);
    }
}