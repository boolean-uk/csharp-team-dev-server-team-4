using exercise.wwwapi.DTOs;
using exercise.wwwapi.DTOs.Comments;
using exercise.wwwapi.DTOs.Comments.UpdateComment;
using exercise.wwwapi.Helpers;
using exercise.wwwapi.Models;
using exercise.wwwapi.Repository;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Post = exercise.wwwapi.Models.Post;

namespace exercise.wwwapi.Endpoints;

public static class CommentEndpoints
{
    public static async Task ConfigureCommentEndpoints(this WebApplication app)
    {
        var comments = app.MapGroup("comments");
        comments.MapPatch("/{id}", UpdateComment).RequireAuthorization().WithSummary("Update a comment");
        comments.MapDelete("/{id}", DeleteComment).RequireAuthorization().WithSummary("Delete a comment");
        
        app.MapGet("/posts/{postId}/comments", GetCommentsPerPost).WithSummary("Get all comments for a post");
        app.MapPost("/posts/{postId}/comments", CreateComment).RequireAuthorization().WithSummary("Create a comment");
    }
    [ProducesResponseType(StatusCodes.Status200OK)]
    private static async Task<IResult> GetCommentsPerPost(IRepository<Comment> commentRepository,
            ClaimsPrincipal comment, int postId)
    {
        var commentsForPost = await commentRepository.GetWithIncludes(c => c.Where(c => c.PostId == postId));

        var commentData = new CommentsSuccessDTO
        {
            Comments = commentsForPost.Select(c => new CommentDTO
            {
                Id = c.Id,
                PostId = postId,
                UserId = c.UserId,
                Body = c.Body,
                CreatedAt = c.CreatedAt
            }).ToList()
        };

        var response = new ResponseDTO<CommentsSuccessDTO>
        {
            Status = "success",
            Data = commentData
        };

        return TypedResults.Ok(response);
    }

    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public static async Task<IResult> CreateComment(
        CreateCommentRequestDTO request,
        IRepository<Comment> commentRepository,
        IRepository<Post> postRepository,
        IValidator<CreateCommentRequestDTO> validator,
        ClaimsPrincipal claimsPrincipal,
        int postId)
    {
        var userIdClaim = claimsPrincipal.UserRealId();

        var validation = await validator.ValidateAsync(request);
        if (!validation.IsValid)
        {
            var failureDTO = new CreateCommentFailureDTO();

            foreach (var error in validation.Errors)
            {
                if (error.PropertyName.Equals("body", StringComparison.OrdinalIgnoreCase))
                    failureDTO.BodyErrors.Add(error.ErrorMessage);
            }

            var failResponse = new ResponseDTO<CreateCommentFailureDTO> { Status = "fail", Data = failureDTO };
            return Results.BadRequest(failResponse);
        }

        var post = await postRepository.GetByIdAsync(postId);
        if (post == null)
        {
            return Results.NotFound();
        }

        var comment = new Comment
        {
            PostId = postId,
            UserId = userIdClaim.Value,
            Body = request.Body,
            CreatedAt = DateTime.UtcNow,
        };

        commentRepository.Insert(comment);
        await commentRepository.SaveAsync();

        var commentData = new CommentDTO
        {
            Id = comment.Id,
            PostId = comment.PostId,
            UserId = comment.UserId,
            Body = comment.Body,
            CreatedAt = comment.CreatedAt
        };

        var response = new ResponseDTO<CommentDTO>
        {
            Status = "success",
            Data = commentData
        };

        return Results.Created($"/comments/{comment.Id}", response);
    }

    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public static async Task<IResult> UpdateComment(
        IRepository<Comment> commentRepository,
        int id,
        UpdateCommentRequestDTO request,
        IValidator<UpdateCommentRequestDTO> validator,
        ClaimsPrincipal claimsPrincipal)
    {

        var userIdClaim = claimsPrincipal.UserRealId();
        if (userIdClaim == null)
        {
            return Results.Unauthorized();
        }

        var comment = await commentRepository.GetByIdAsync(id);

        if (comment == null)
        {
            return TypedResults.NotFound();
        }

        if (comment.UserId != userIdClaim)
        {
            return Results.Unauthorized();
        }

        var validation = await validator.ValidateAsync(request);
        if (!validation.IsValid)
        {
            var failureDto = new UpdateCommentFailureDTO();
            foreach (var error in validation.Errors)
            {
                if (error.PropertyName.Equals("Body", StringComparison.OrdinalIgnoreCase))
                    failureDto.BodyErrors.Add(error.ErrorMessage);
            }

            var failResponse = new ResponseDTO<UpdateCommentFailureDTO>
            {
                Status = "fail",
                Data = failureDto
            };
            return Results.BadRequest(failResponse);
        }

        if (request.Body != null) comment.Body = request.Body;

        commentRepository.Update(comment);
        await commentRepository.SaveAsync();

        var response = new ResponseDTO<CommentDTO>
        {
            Status = "success",
            Data = new CommentDTO
            {
                Id = comment.Id,
                PostId = comment.PostId,
                UserId = comment.UserId,
                Body = comment.Body,
                CreatedAt = comment.CreatedAt,
            }
        };

        return TypedResults.Ok(response);
    }

    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public static async Task<IResult> DeleteComment(
        IRepository<Comment> commentRepository,
        int id,
        ClaimsPrincipal claimsPrincipal)
    {

        var userIdClaim = claimsPrincipal.UserRealId();
        if (userIdClaim == null)
        {
            return Results.Unauthorized();
        }

        var comment = await commentRepository.GetByIdAsync(id);

        if (comment == null)
        {
            return TypedResults.NotFound();
        }

        if (comment.UserId != userIdClaim)
        {
            return Results.Unauthorized();
        }

        commentRepository.Delete(comment);
        await commentRepository.SaveAsync();

        var response = new ResponseDTO<CommentDTO>
        {
            Status = "success",
            Data = new CommentDTO
            {
                Id = comment.Id,
                PostId = comment.PostId,
                UserId = comment.UserId,
                Body = comment.Body,
                CreatedAt = comment.CreatedAt
            }
        };

        return TypedResults.Ok(response);
    }
}