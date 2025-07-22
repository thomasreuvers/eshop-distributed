using Catalog.Domain;
using Catalog.Services;

namespace Catalog.Endpoints;

public static class ProductEndpoints
{
    public static void MapProductEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/products");
        
        // GET all
        group.MapGet("/", async (ProductService service) =>
        {
            var products = await service.GetProductsAsync();
            return Results.Ok(products);
        })
        .WithName("GetAllProducts")
        .Produces<List<Product>>();
        
        // GET by id
        group.MapGet("/{id:Guid}", async (Guid id, ProductService service) =>
        {
            var product = await service.GetProductByIdAsync(id);
            return product is not null ? Results.Ok(product) : Results.NotFound();
        })
        .WithName("GetProductById")
        .Produces<Product>()
        .Produces(StatusCodes.Status404NotFound);
        
        // POST create
        group.MapPost("/", async (Product product, ProductService service) =>
            {
                await service.CreateProductAsync(product);
                return Results.Created($"/products/{product.Id}", product);
            })
            .WithName("CreateProduct")
            .Produces<Product>(StatusCodes.Status201Created);
        
        // PUT update
        group.MapPut("/{id:Guid}", async (Guid id, Product inputProduct, ProductService service) =>
        {
            var product = await service.GetProductByIdAsync(id);
            if (product is null)
            {
                return Results.NotFound();
            }

            await service.UpdateProductAsync(product, inputProduct);
            return Results.NoContent();
        })
        .WithName("UpdateProduct")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound);
        
        // DELETE
        group.MapDelete("/{id:guid}", async (Guid id, ProductService service) =>
        {
            var product = await service.GetProductByIdAsync(id);
            if (product is null)
            {
                return Results.NotFound();
            }

            await service.DeleteProductAsync(product);
            return Results.NoContent();
        })
        .WithName("DeleteProduct")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound);

        group.MapGet("/support/{query}", async (string query, ProductAIService service) =>
        {
            var response = await service.SupportAsync(query);

            return Results.Ok(response);
        })
        .WithName("Support")
        .Produces(StatusCodes.Status200OK);
    }
}