using exercise.wwwapi.DTOs;
using exercise.wwwapi.DTOs.Courses;
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
        cohorts.MapGet("/", GetAllCohorts).WithSummary("Get all cohorts");
        cohorts.MapGet("/{id}", GetCohortById).WithSummary("Get cohort by id");
        cohorts.MapPatch("/{id}", UpdateCohortById).WithSummary("Update cohort");
        cohorts.MapDelete("/{id}", DeleteCohortById).WithSummary("Delete cohort");
    }

    


    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public static async Task<IResult> CreateCohort(IRepository<Cohort> cohortRepo, CohortPostDTO? postCohort)
    {
        // Check for missing or invalid data
        if (postCohort == null ||
            string.IsNullOrWhiteSpace(postCohort.CohortName) ||
            postCohort.StartDate == DateTime.MinValue ||
            postCohort.EndDate == DateTime.MinValue)
        {
            return TypedResults.BadRequest("Missing or invalid cohort data");
        }
        // Check if end date is before start date
        if (postCohort.EndDate < postCohort.StartDate)
        {
            return TypedResults.BadRequest("End date cannot be before start date");
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
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public static async Task<IResult> GetAllCohorts(IRepository<Cohort> cohortRepo)
    {
        // Use GetWithIncludes to include CohortCourses and their Course
        var cohorts = await cohortRepo.GetWithIncludes(q =>
            q.Include(c => c.CohortCourses)
             .ThenInclude(cc => cc.Course)
        );

        var cohortDTOs = cohorts.Select(c => new CohortDTO(c)).ToList();

        var response = new ResponseDTO<List<CohortDTO>>()
        {
            Status = "success",
            Data = cohortDTOs
        };
        return TypedResults.Ok(response);
    }
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public static async Task<IResult> GetCohortById(IRepository<Cohort> cohortRepo, int id)
    {
        // uses GetByIdWithIncludes for nested includes
        var cohort = await cohortRepo.GetByIdWithIncludes(q =>
            q.Include(c => c.CohortCourses)
             .ThenInclude(cc => cc.Course), id);

        if (cohort == null)
        {
            return TypedResults.NotFound();
        }

        var cohortDTO = new CohortDTO(cohort);

        var response = new ResponseDTO<CohortDTO>
        {
            Status = "success",
            Data = cohortDTO
        };

        return TypedResults.Ok(response);
    }
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public static async Task<IResult> UpdateCohortById(IRepository<Cohort> cohortRepo, int id, CohortPostDTO updateDto)
    {
        var cohort = await cohortRepo.GetByIdAsync(id);
        if (cohort == null)
        {
            return TypedResults.NotFound();
        }

        // Check for missing or invalid data
        if (string.IsNullOrWhiteSpace(updateDto.CohortName) ||
            updateDto.StartDate == DateTime.MinValue ||
            updateDto.EndDate == DateTime.MinValue)
        {
            return TypedResults.BadRequest("Missing or invalid cohort data");
        }
        // Check if end date is before start date
        if (updateDto.EndDate < updateDto.StartDate)
        {
            return TypedResults.BadRequest("End date cannot be before start date");
        }

        cohort.CohortName = updateDto.CohortName;
        cohort.StartDate = updateDto.StartDate;
        cohort.EndDate = updateDto.EndDate;

        cohortRepo.Update(cohort);
        await cohortRepo.SaveAsync();

        var cohortDTO = new CohortDTO(cohort);

        var response = new ResponseDTO<CohortDTO>
        {
            Status = "success",
            Data = cohortDTO
        };

        return TypedResults.Ok(response);
    }
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public static async Task<IResult> DeleteCohortById(IRepository<Cohort> cohortRepo, int id)
    {
        var cohort = await cohortRepo.GetByIdAsync(id);
        if (cohort == null)
        {
            return TypedResults.NotFound();
        }

        cohortRepo.Delete(cohort);
        await cohortRepo.SaveAsync();

        return TypedResults.Ok(new { Status = "success", Data = $"Cohort with id {id} deleted" });
    }

}