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
    }
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public static async Task<IResult> CreateCohort(IRepository<Cohort> cohortRepo, CohortPostDTO? postCohort)
    {
        if (postCohort == null || postCohort.StartDate == DateTime.MinValue || postCohort.EndDate == DateTime.MinValue)
        {
            return TypedResults.BadRequest("Missing cohort data");
        }

        bool success = false;
        int attempts = 0;
        int newCohortNumber = 0;

        while (!success && attempts < 5)
        {
            try
            {
                var maxCohortNumber = (int?) await cohortRepo.GetMaxValueAsync(c => c.CohortNumber);
                if (maxCohortNumber == null)
                {
                    maxCohortNumber = 0;
                }
                if (postCohort.CohortName == null)
                {
                    postCohort.CohortName = $"Cohort {maxCohortNumber + 1}";
                }

                newCohortNumber = (int)(maxCohortNumber + 1);
                Cohort newCohort = new Cohort { CohortNumber = newCohortNumber, CohortName = postCohort.CohortName, StartDate = postCohort.StartDate, EndDate = postCohort.EndDate };

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
                else
                {
                    Console.WriteLine($"DB update error: {ex.StackTrace}");
                    return TypedResults.InternalServerError($"Error while updating the database: {ex.Message}");
                }
            }
        }

        return TypedResults.Created($"/cohorts/{newCohortNumber}");
    }

}