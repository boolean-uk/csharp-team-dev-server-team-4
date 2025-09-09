using exercise.wwwapi.DTOs;
using exercise.wwwapi.DTOs.GetObjects;
using exercise.wwwapi.DTOs.Posts;
using exercise.wwwapi.DTOs.Posts.UpdatePost;
using exercise.wwwapi.Helpers;
using exercise.wwwapi.Repository;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Post = exercise.wwwapi.Models.Post;

namespace exercise.wwwapi.Endpoints;

public static class PostEndpoints
{
    public static void ConfigurePostEndpoints(this WebApplication app)
    {
        var posts = app.MapGroup("posts");
        posts.MapPost("/", CreatePost).WithSummary("Create post");
        posts.MapGet("/", GetAllPosts).WithSummary("Get all posts");
        posts.MapPatch("/{id}", UpdatePost).RequireAuthorization().WithSummary("Update a post");
        posts.MapDelete("/{id}", DeletePost).RequireAuthorization().WithSummary("Delete a post");
    }

[Authorize]
[ProducesResponseType(StatusCodes.Status201Created)]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
public static async Task<IResult> CreatePost(
    CreatePostRequestDTO request,
    IRepository<Post> postRepository,
    IValidator<CreatePostRequestDTO> validator,
    ClaimsPrincipal claimsPrincipal)
{
    var userId = claimsPrincipal.UserRealId();
    if (userId == null) return Results.Unauthorized();

    var validation = await validator.ValidateAsync(request);
    if (!validation.IsValid)
    {
        var failureDTO = new CreatePostFailureDTO();
        foreach (var error in validation.Errors)
            if (error.PropertyName.Equals("Body", StringComparison.OrdinalIgnoreCase))
                failureDTO.BodyErrors.Add(error.ErrorMessage);

        return Results.BadRequest(new ResponseDTO<CreatePostFailureDTO>
        {
            Status = "fail",
            Data = failureDTO
        });
    }

    var post = new Post
    {
        AuthorId = userId.Value,
        Body = request.Body!,
        Likes = 0,
        CreatedAt = DateTime.UtcNow
    };

    postRepository.Insert(post);
    await postRepository.SaveAsync();

    var response = new ResponseDTO<CreatePostSuccessDTO>
    {
        Status = "success",
        Data = new CreatePostSuccessDTO
        {
            Posts = new PostDTO
            {
                Id = post.Id,
                AuthorId = post.AuthorId,
                Body = post.Body,
                Likes = post.Likes,
                CreatedAt = post.CreatedAt
            }
        }
    };

    return Results.Created($"/posts/{post.Id}", response);
}


    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    private static async Task<IResult> GetAllPosts(IRepository<Post> postRepository,
            ClaimsPrincipal user)
    {
        var results = (await postRepository.GetAllAsync(
            p => p.Author,
            p => p.Comments
        )).ToList();

        var postData = new PostsSuccessDTO
        {
            Posts = results
        };

        var response = new ResponseDTO<PostsSuccessDTO>
        {
            Status = "success",
            Data = postData
        };

        return TypedResults.Ok(response);
    }

    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public static async Task<IResult> UpdatePost(IRepository<Post> postRepository, int id,
            UpdatePostRequestDTO request,
            IValidator<UpdatePostRequestDTO> validator, ClaimsPrincipal claimsPrincipal)
    {
        var userIdClaim = claimsPrincipal.UserRealId();
        if (userIdClaim == null)
        {
            return Results.Unauthorized();
        }

        var post = await postRepository.GetByIdAsync(
            id,
            p => p.Author,
            p => p.Comments
        );

        if (post == null)
        {
            return TypedResults.NotFound();
        }

        if (post.AuthorId != userIdClaim)
        {
            return Results.Unauthorized();
        }

        var validation = await validator.ValidateAsync(request);
        if (!validation.IsValid)
        {
            var failureDto = new UpdatePostFailureDTO();
            foreach (var error in validation.Errors)
            {
                if (error.PropertyName.Equals("Body", StringComparison.OrdinalIgnoreCase))
                    failureDto.BodyErrors.Add(error.ErrorMessage);
            }

            var failResponse = new ResponseDTO<UpdatePostFailureDTO>
            {
                Status = "fail",
                Data = failureDto
            };
            return Results.BadRequest(failResponse);
        }

        if (request.Body != null) post.Body = request.Body;

        postRepository.Update(post);
        await postRepository.SaveAsync();

        var response = new ResponseDTO<UpdatePostSuccessDTO>
        {
            Status = "success",
            Data = new UpdatePostSuccessDTO
            {
                Id = post.Id,
                AuthorId = post.AuthorId,
                Body = post.Body,
                Likes = post.Likes,
                CreatedAt = post.CreatedAt
            }
        };

        return TypedResults.Ok(response);
    }

    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public static async Task<IResult> DeletePost(IRepository<Post> postRepository, int id,
        ClaimsPrincipal claimsPrincipal)
    {
        var userIdClaim = claimsPrincipal.UserRealId();
        if (userIdClaim == null)
        {
            return Results.Unauthorized();
        }

        var post = await postRepository.GetByIdAsync(
            id,
            p => p.Author,
            p => p.Comments
        );

        if (post == null)
        {
            return TypedResults.NotFound();
        }

        if (post.AuthorId != userIdClaim)
        {
            return Results.Unauthorized();
        }

        postRepository.Delete(post);
        await postRepository.SaveAsync();

        var response = new ResponseDTO<PostDTO>
        {
            Status = "success",
            Data = new PostDTO
            {
                Id = post.Id,
                AuthorId = post.AuthorId,
                Body = post.Body,
                Likes = post.Likes,
                CreatedAt = post.CreatedAt
            }
        };

        return TypedResults.Ok(response);
    }
}