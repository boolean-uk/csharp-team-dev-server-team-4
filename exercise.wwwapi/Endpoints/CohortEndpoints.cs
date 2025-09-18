using exercise.wwwapi.DTOs;
using exercise.wwwapi.Enums;
using exercise.wwwapi.Models;
using exercise.wwwapi.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace exercise.wwwapi.Endpoints;

public static class CohortEndpoints
{
    public static void ConfigureCohortEndpoints(this WebApplication app)
    {
        var cohorts = app.MapGroup("cohorts");
        cohorts.MapPost("/", CreateCohort).WithSummary("Create a cohort");
        cohorts.MapGet("/{id}", GetCohortById).WithSummary("Get a cohort including its course, but not including its users");
    }
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public static async Task<IResult> CreateCohort(IRepository<Cohort> cohortRepo, CohortPostDTO? postCohort)
    {
        if (postCohort == null || postCohort.CourseId == 0)
        {
            return TypedResults.BadRequest("Missing cohort data");
        }

        bool success = false;
        int attempts = 0;

        while (!success && attempts < 5)
        {
            try
            {
                var maxCohortNumber = (int?) await cohortRepo.GetMaxValueAsync(c => c.CohortNumber);
                if (maxCohortNumber == null)
                {
                    maxCohortNumber = 0;
                }

                Cohort newCohort = new Cohort { CohortNumber = (int)(maxCohortNumber + 1), CourseId = postCohort.CourseId };

                cohortRepo.Insert(newCohort);
                await cohortRepo.SaveAsync();
                success = true;
            }
            catch (DbUpdateException ex) 
            {
                if (ex.InnerException is PostgresException CohortNumberEx &&
                     CohortNumberEx.SqlState == "23505") //23505 = Cohort number value already exists
                {
                    attempts++;
                }
                else if (ex.InnerException is PostgresException CourseIdEx &&
                     CourseIdEx.SqlState == "23503") //23503 = No course with given id exists
                {
                    return TypedResults.BadRequest("No course with given id exists");
                }
                else
                {
                    Console.WriteLine($"DB update error: {ex.StackTrace}");
                    return TypedResults.InternalServerError($"Error while updating the database: {ex.Message}");
                }
            }
        }
        return TypedResults.Created($"/cohorts/{postCohort.CourseId}");
    }

    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public static async Task<IResult> GetCohortById(IRepository<Cohort> cohortRepo, int id)
    {

        Cohort? cohort = await cohortRepo.GetByIdAsync(id, c => c.Course);

        if (cohort == null)
        {
            return TypedResults.NotFound("No course with given id found");
        }

        CohortDTO response = new CohortDTO {
            CourseId = cohort.CourseId,
            CohortNumber = cohort.CohortNumber,
            Course = new CourseDTO { CourseName = cohort.Course.CourseName } };

        return TypedResults.Ok(response);
    }
}