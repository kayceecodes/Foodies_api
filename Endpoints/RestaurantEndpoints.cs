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
        app.MapPost("/api/restaurant", async context =>
        {
            // Use the typed HTTP client to make a request
            var externalApiClient = context.RequestServices.GetRequiredService<IExternalApiClient>();
            string data = await externalApiClient.GetBusiness();

            await context.Response.WriteAsync(data);
        } 
        
        ).WithName("Restaurant").Accepts<RestaurantDto>("application/json")
            .Produces<APIResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);
    }

    public static async Task<IResult> GetRestaurant()
    {
        IHttpContextAccessor 
        return Results.Ok();
    }
}
 