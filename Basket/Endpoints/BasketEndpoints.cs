using Basket.Models;
using Basket.Services;

namespace Basket.Endpoints;

public static class BasketEndpoints
{
    public static void MapBasketEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("basket");

        group.MapGet("/{userName}", async (string userName, BasketService service) =>
        {
            var basket = await service.GetBasketAsync(userName);
            return Results.Ok(basket);
        })
        .WithName("GetBasket")
        .Produces<ShoppingCart>()
        .Produces(StatusCodes.Status200OK);

        group.MapPost("/", async (ShoppingCart basket, BasketService service) =>
        {
            await service.UpdateBasketAsync(basket);
            return Results.Created("GetBasket", basket);
        })
        .WithName("UpdateBasket")
        .Produces<ShoppingCart>(StatusCodes.Status201Created);

        group.MapDelete("/{userName}", async (string userName, BasketService service) =>
        {
            await service.DeleteBasketAsync(userName);
            return Results.NoContent();
        })
        .WithName("DeleteBasket")
        .Produces(StatusCodes.Status204NoContent);
    }
}