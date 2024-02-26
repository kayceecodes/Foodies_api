namespace foodies_api.Endpoints;

public static class ReviewEndpionts
{
    public static void ConfigurationReviewEndpoints(this WebApplication app) 
    {
        app.MapGet("/api/review/{id}", async Task<IResult> (HttpContext context, string id) =>
        {
            var YelpApiClient = app.Services.GetRequiredService<YelpApiClient>();
            APIResult result = await YelpApiClient.GetReviewById(id);

            if (result.IsSuccess)
                return TypedResults.Ok(result.Data);
            else
                return TypedResults.BadRequest();
        
        }).WithName("GetReviewById").Accepts<ReviewDto>("application/json")
        .Produces<ReviewDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest);

        app.MapPost("/api/review", async () => {}).WithName("AddReview").Accepts<ReviewDto>("application/json")
        .Produces<ReviewDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest);
    }
}
