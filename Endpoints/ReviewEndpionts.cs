namespace foodies_api;

public static class ReviewEndpionts
{
    public static void ConfigurationReviewEndpoints(this WebApplication app) 
    {
        app.MapGet("/api/rating", GetReview).WithName("GetReview").Accepts<ReviewDto>("application/json")
            .Produces<ReviewDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);
        app.MapPost("/api/rating", AddReview).WithName("AddReview").Accepts<ReviewDto>("application/json")
            .Produces<ReviewDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);

    }

    public static async Task<IResult> GetReview()
    {
        return Results.Ok();
    }

    public static async Task<IResult> AddReview()
    {
        return Results.Ok();
    }
}
