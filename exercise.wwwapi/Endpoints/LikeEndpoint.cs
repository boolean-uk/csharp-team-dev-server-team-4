using exercise.wwwapi.DTOs;
using exercise.wwwapi.DTOs.Likes;
using exercise.wwwapi.Helpers;
using exercise.wwwapi.Models;
using exercise.wwwapi.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace exercise.wwwapi.Endpoints
{
    public static class LikeEndpoint
    {
        public static async Task ConfigureLikeEndpoints(this WebApplication app)
        {
            var likes = app.MapGroup("likes");
            likes.MapPost("/{postId}", toggleLike).RequireAuthorization().WithSummary("toggle between liked and unliked");

        }
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public static async Task<IResult> toggleLike(
            IRepository<Like> likeRepository,
            IRepository<Post> postRepository,
            ClaimsPrincipal claimsPrincipal,
            int postId)
        {
            var userIdClaim = claimsPrincipal.UserRealId();
            if (userIdClaim == null) return Results.Unauthorized();

            var post = await postRepository.GetByIdWithIncludes(p => p.Include(l => l.Likes), postId);
            if (post == null) return TypedResults.NotFound();
            var isLiked = post.Likes.FirstOrDefault(l => l.UserId == userIdClaim);

            if (isLiked != null)
            {
                var deleteResponse = new ResponseDTO<LikeDTO>
                {
                    Status = "Success",
                    Data = new LikeDTO()
                    {
                       Id = isLiked.Id,
                       PostId = isLiked.PostId,
                       UserId = isLiked.UserId
                    }
                };
                likeRepository.Delete(isLiked);
                await likeRepository.SaveAsync();
                return Results.Ok(deleteResponse);
            }

            var like = new Like
            {
                PostId = postId,
                UserId = userIdClaim.Value,
            };
            var response = new ResponseDTO<LikeDTO>
            {
                Status = "success",
                Data = new LikeDTO
                {
                    Id = like.Id,
                    PostId = like.PostId,
                    UserId = like.UserId
                }
            };

            likeRepository.Insert(like);
            await likeRepository.SaveAsync();

            return Results.Created($"/likes/{postId}", response);
        }
    }
}
