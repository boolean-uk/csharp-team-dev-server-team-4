using Microsoft.AspNetCore.Mvc;

namespace exercise.wwwapi.Endpoints;

public static class PostEndpoints
{
    public static void ConfigurePostEndpoints(this WebApplication app)
    {
        var posts = app.MapGroup("posts");
        posts.MapPost("/", CreatePost).WithSummary("Create post");
        posts.MapGet("/", GetAllPosts).WithSummary("Get all posts");
    }
    [ProducesResponseType(StatusCodes.Status200OK)]
    public static async Task<IResult> CreatePost()
    {
        return TypedResults.Ok();
    }
    [ProducesResponseType(StatusCodes.Status200OK)]
    public static async Task<IResult> GetAllPosts()
    {
        return TypedResults.Ok();
    }
}