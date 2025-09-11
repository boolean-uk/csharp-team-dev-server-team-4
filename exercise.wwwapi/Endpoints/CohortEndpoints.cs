using exercise.wwwapi.DTOs;
using exercise.wwwapi.DTOs.Cohorts;
using exercise.wwwapi.Models;
using exercise.wwwapi.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace exercise.wwwapi.Endpoints;

public static class CohortEndpoints
{
    public static void ConfigureCohortEndpoints(this WebApplication app)
    {
        var cohorts = app.MapGroup("cohorts");
        cohorts.MapPost("/", GetCohorts).WithSummary("List all cohorts");
    }

    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public static async Task<IResult> GetCohorts(IRepository<Cohort> cohortRepository, ClaimsPrincipal user)
    {
        var results = (await cohortRepository.GetAllAsync(
            c => c.Course
        )).ToList();

        var cohortData = new CohortsSuccessDTO
        {
            Cohorts = results.Select(c => new CohortDTO
            {
                Id = c.Id,
                CourseId = c.CourseId
            }).ToList()
        };

        var response = new ResponseDTO<CohortsSuccessDTO>
        {
            Status = "success",
            Data = cohortData
        };

        return TypedResults.Ok(response);
    }
}