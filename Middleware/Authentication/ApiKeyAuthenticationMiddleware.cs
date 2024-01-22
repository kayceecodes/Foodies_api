using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace foodies_api;

public class ApiKeyAuthenticationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly string _apiKey;

    public ApiKeyAuthenticationMiddleware(RequestDelegate next, string apiKey)
    {
        _next = next;
        _apiKey = apiKey;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (!AuthorizeRequest(context))
        {
            context.Response.StatusCode = 401; // Unauthorized
            await context.Response.WriteAsync("Invalid API Key");
            return;
        }

        await _next(context);
    }

    private bool AuthorizeRequest(HttpContext context)
    {
        string apiKey = context.Request.Headers["ApiKey"].ToString();

        return apiKey == _apiKey;
    }
}
