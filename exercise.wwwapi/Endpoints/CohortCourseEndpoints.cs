using exercise.wwwapi.DTOs;
using exercise.wwwapi.DTOs.CohortCourse;
using exercise.wwwapi.DTOs.Courses;
using exercise.wwwapi.DTOs.Exercises;
using exercise.wwwapi.DTOs.Users;
using exercise.wwwapi.Enums;
using exercise.wwwapi.Helpers;
using exercise.wwwapi.Models;
using exercise.wwwapi.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Net.NetworkInformation;
using System.Security.Claims;

namespace exercise.wwwapi.Endpoints;

public static class CohortCourseEndpoints
{
    public static void ConfigureCohortCourseEndpoints(this WebApplication app)
    {
        var cohortcourses = app.MapGroup("cohortcourses");
        cohortcourses.MapPost("/cohortCourse", CreateCohortCourse).WithSummary("creates a new cohort course");
        cohortcourses.MapGet("/", GetAllCohortCourses).WithSummary("Get all cohort_courses");
        cohortcourses.MapGet("/{id}", GetCohortCourseById).WithSummary("Get cohort_course by id");
        cohortcourses.MapPost("/moveUser{user_id}", MoveUser).WithSummary("Creates a new user_cc to move student");
    }


    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public static async Task<IResult> CreateCohortCourse(IRepository<CohortCourse> cohortCourseRepository, PostCohortCourseDTO model, ClaimsPrincipal claimPrincipal)
    {
        var userRole = claimPrincipal.Role();
        var authorizedAsTeacher = claimPrincipal.IsInRole("Teacher");
        if (!authorizedAsTeacher)
        {
            return TypedResults.Unauthorized();
        }
        if (model == null)
        {
            return TypedResults.BadRequest("provide cohortid and courseid");
        }

        var response = await cohortCourseRepository.GetWithIncludes(a => a.Where(b => b.CohortId == model.CohortId && b.CourseId == model.CourseId));
        if (response != null && response?.Count != 0)
        {
            return TypedResults.BadRequest("CohortCourse already exists.");
        }
        //implement later
        //if usercc-combo already exists, delete old and create new

        
        cohortCourseRepository.Insert(new CohortCourse
        {
            CohortId = model.CohortId,
            CourseId = model.CourseId
        });
        await cohortCourseRepository.SaveAsync();
        return TypedResults.Ok("CohortCourse created");
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public static async Task<IResult> MoveUser(IRepository<UserCC> userCCRepository, IRepository<CohortCourse> cohortCourseRepository, IRepository<User> userRepository, int user_id, PostUserCohortCourseDTO userCC, ClaimsPrincipal claimPrincipal)
    {
        if (userCC == null)
        {
            return TypedResults.BadRequest("No user_id provided");
        }
        var cohortCourse = await cohortCourseRepository.GetWithIncludes(a => a.Where(b => b.CohortId == userCC.CohortId && b.CourseId == userCC.CourseId));
        if (cohortCourse == null || cohortCourse.Count == 0)
        {
            return TypedResults.NotFound("No cohort_course with that cohort_id and course_id exists");
        }
        //implement later
        //if usercc-combo already exists, delete old and create new
        
        var userRole = claimPrincipal.Role();
        var authorizedAsTeacher = claimPrincipal.IsInRole("Teacher");
        if(!authorizedAsTeacher)
        {
            return TypedResults.Unauthorized();
        }
        userCCRepository.Insert(new UserCC
        {
            UserId = user_id,
            CcId = cohortCourse.First().Id
        });
        await userCCRepository.SaveAsync();
        return TypedResults.Ok("User moved successfully");
    }



        [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public static async Task<IResult> GetAllCohortCourses(IRepository<CohortCourse> cohortCourseRepository)
    {
        var response = await cohortCourseRepository.GetWithIncludes(a => a
                                                                            .Include(b => b.Cohort)
                                                                            .Include(c => c.Course)
                                                                            .Include(d => d.UserCCs)
                                                                                .ThenInclude(e => e.User)
                                                                                .ThenInclude(f => f.Notes));

        var allUserCCs = response.SelectMany(cc => cc.UserCCs).ToList();

        var latestUserCCs = allUserCCs
        .GroupBy(uc => uc.UserId)
        .Select(g => g.OrderByDescending(uc => uc.Id).First())
        .ToList();

        var result = response.Select(cc =>
        {
            var users = latestUserCCs
                .Where(uc => uc.CcId == cc.Id)
                .Select(uc => new UserDTO(uc.User))
                .ToList();

            var dto = new GetCohortCourseDTO(cc);
            dto.Users = users;
            return dto;
        }).ToList();

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