using exercise.wwwapi.DTOs;
using exercise.wwwapi.DTOs.CohortCourse;
using exercise.wwwapi.DTOs.Courses;
using exercise.wwwapi.DTOs.Exercises;
using exercise.wwwapi.Enums;
using exercise.wwwapi.Models;
using exercise.wwwapi.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace exercise.wwwapi.Endpoints;

public static class CohortCourseEndpoints
{
    public static void ConfigureCohortCourseEndpoints(this WebApplication app)
    {
        var cohortcourses = app.MapGroup("cohortcourses");
        cohortcourses.MapGet("/", GetAllCohortCourses).WithSummary("Get all cohort_courses");
        cohortcourses.MapGet("/{id}", GetCohortCourseById).WithSummary("Get cohort_course by id");
    }
    
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public static async Task<IResult> GetAllCohortCourses(IRepository<CohortCourse> cohortCourseRepository)
    {
        var response = await cohortCourseRepository.GetWithIncludes(a => a
                                                                            .Include(b => b.Cohort)
                                                                            .Include(c => c.Course)
                                                                            .Include(d => d.UserCCs)
                                                                                .ThenInclude(e => e.User));

        var result = response.Select(cc => new GetCohortCourseDTO(cc)).ToList();

        return TypedResults.Ok(result);

      
    }
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public static async Task<IResult> GetCohortCourseById(IRepository<CohortCourse> cohortCourseRepository, int id)
    {
        var response = await cohortCourseRepository.GetByIdWithIncludes(a => a
                                                                            .Include(b => b.Cohort)
                                                                            .Include(c => c.Course)
                                                                            .Include(d => d.UserCCs)
                                                                                .ThenInclude(e => e.User), id);

        if (response == null) return TypedResults.NotFound("No cohort_course with that id exists");

        var result = new GetCohortCourseDTO(response);

        return TypedResults.Ok(result);
    }
   

}