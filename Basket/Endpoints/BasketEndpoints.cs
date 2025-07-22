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
        .Produces(StatusCodes.Status200OK)
        .RequireAuthorization();

        group.MapPost("/", async (ShoppingCart basket, BasketService service) =>
        {
            await service.UpdateBasketAsync(basket);
            return Results.Created("GetBasket", basket.UserName);
        })
        .WithName("UpdateBasket")
        .Produces<ShoppingCart>(StatusCodes.Status201Created)
        .RequireAuthorization();

        group.MapDelete("/{userName}", async (string userName, BasketService service) =>
            {
                await service.DeleteBasketAsync(userName);
                return Results.NoContent();
            })
            .WithName("DeleteBasket")
            .Produces(StatusCodes.Status204NoContent)
            .RequireAuthorization();
    }
}