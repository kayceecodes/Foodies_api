using foodies_api.Middleware.Authentication;

namespace foodies_api;

public class ApiKeyEndpointFilter : IEndpointFilter
{
    private readonly IConfiguration _configuration;

    public ApiKeyEndpointFilter(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async ValueTask<object> InvokeAsync(
        EndpointFilterInvocationContext context, 
        EndpointFilterDelegate next
    )
    {
        if (!context.HttpContext.Request.Headers
            .TryGetValue(AuthConstants.ApiKeyHeaderName,
            out var extractedApiKey))
        {
            return new UnauthorizedHttpObjectResult("API Key Missing");
            // return TypedResults.Unauthorized();
        }

        var apiKey = _configuration.GetValue<string>(AuthConstants.ApiKeyHeaderName);
        if(apiKey.Equals(extractedApiKey))
        {
            return new UnauthorizedHttpObjectResult("Invalid API Key");
            // return TypedResults.Unauthorized();
        }

        return await next(context);
    }
}
