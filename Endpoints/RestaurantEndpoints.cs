using foodies_api.Models.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace foodies_api.Endpoints;

public static class RestaurantEndpoints
{
    public static void ConfigurationRestaurantEndpoints(this WebApplication app, HttpContext context) 
    {

        app.MapGet("/api/restaurant", GetRestaurant
        ).WithName("Restaurant").Accepts<RestaurantDto>("application/json")
            .Produces<APIResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);

        app.MapGet("/api/restaurant", async context =>
        {
 // Use the typed HTTP client to make a request
            var YelpApiClient = context.RequestServices.GetRequiredService<IYelpApiClient>();
            ApiResult<string> result = await YelpApiClient.GetBusiness();

            if (result.IsSuccessStatusCode)
            {
                await context.Response.WriteAsync($"Success: {result.Data}");
            }
            else
            {
                context.Response.StatusCode = result.StatusCode ?? 500; // Default to Internal Server Error
                await context.Response.WriteAsync($"Error: {result.ErrorMessage}");
            }
        });
    }

    public static async Task<IResult> GetRestaurant()
    {
 
        return Results.Ok();
    }
}
 