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
    public static void ConfigurationRestaurantEndpoints(this WebApplication app) 
    {
        app.MapGet("/api/restaurant/{id}", async (HttpContext context, string id) =>
        {
            var YelpApiClient = app.Services.GetRequiredService<YelpApiClient>();
            ApiResult<string> result = await YelpApiClient.GetBusiness(id);

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
 