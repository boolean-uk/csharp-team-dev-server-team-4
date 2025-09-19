using exercise.wwwapi.DTOs.Courses;
using exercise.wwwapi.DTOs.Exercises;
using exercise.wwwapi.Models;
using exercise.wwwapi.Models.Exercises;
using exercise.wwwapi.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace exercise.wwwapi.Endpoints;
    public static class CourseEndpoints
    {
        private const string GITHUB_URL = "github.com/";

        public static void ConfigureCourseEndpoints(this WebApplication app)
        {
            var courses = app.MapGroup("courses");
            courses.MapGet("/", GetCourses).WithSummary("Returns all courses");
            courses.MapGet("/{id}", GetCourseById).WithSummary("Returns course with provided id");
        }


        [ProducesResponseType(StatusCodes.Status200OK)]
        private static async Task<IResult> GetCourses(IRepository<Course> repository, ClaimsPrincipal claimsPrincipal)
        {
            var response = await repository.GetWithIncludes(c => c.Include(a => a.CourseModules).ThenInclude(b => b.Module).ThenInclude(d => d.Units).ThenInclude(u => u.Exercises));
            var result = response.Select(c => new GetCourseDTO(c));
            return TypedResults.Ok(result);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        private static async Task<IResult> GetCourseById(IRepository<Course> repository, int id)
        {
            var response = await repository.GetByIdWithIncludes(c => c.Include(a => a.CourseModules).ThenInclude(b => b.Module).ThenInclude(d => d.Units).ThenInclude(u => u.Exercises), id);
            var result = new GetCourseDTO(response);
            return TypedResults.Ok(result);
        }
    }
