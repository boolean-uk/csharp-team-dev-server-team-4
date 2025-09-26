using exercise.wwwapi.DTOs;
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
            courses.MapGet("/info", GetAllCoursesInfo).WithSummary("returns id and name for all courses");
            courses.MapGet("/", GetCourses).WithSummary("Returns all courses");
            courses.MapGet("/{id}", GetCourseById).WithSummary("Returns course with provided id");
            courses.MapPost("/", CreateCourse).WithSummary("Create a new course");
            courses.MapDelete("/{id}", DeleteCourseById).WithSummary("Delete a course");
            courses.MapPut("/{id}", UpdateCourse).WithSummary("Update a course name");
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    public static async Task<IResult> GetAllCoursesInfo(IRepository<Course> courseRepository)
    {

        // Use GetWithIncludes to include CohortCourses and their Course
        var response = await courseRepository.GetWithIncludes(null);

        var courses = response.Select(c => new GetCourseInfoDTO(c));

  
        return TypedResults.Ok(courses);
    }


    [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        private static async Task<IResult> GetCourses(IRepository<Course> repository, ClaimsPrincipal claimsPrincipal)
        {
            var response = await repository.GetWithIncludes(c => c.Include(a => a.CourseModules).ThenInclude(b => b.Module).ThenInclude(d => d.Units).ThenInclude(u => u.Exercises));
            if (response == null || response.Count == 0)
            {
                return TypedResults.NotFound("No courses found");
            }
            var result = response.Select(c => new GetCourseDTO(c));
            return TypedResults.Ok(result);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        private static async Task<IResult> GetCourseById(IRepository<Course> repository, int id, ClaimsPrincipal claimsPrincipal)
        {
            var response = await repository.GetByIdWithIncludes(c => c.Include(a => a.CourseModules).ThenInclude(b => b.Module).ThenInclude(d => d.Units).ThenInclude(u => u.Exercises), id);
            if (response == null)
            {
                return TypedResults.NotFound("No course with the given id was found");
            }
            var result = new GetCourseDTO(response);
            return TypedResults.Ok(result);
        }

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> CreateCourse(IRepository<Course> repository, CoursePostDTO postedCourse, ClaimsPrincipal claimsPrincipal)
        {

            if (claimsPrincipal.IsInRole("Teacher") == false)
            {
                return TypedResults.Unauthorized();
            }
            if (postedCourse == null || postedCourse.Name == null || postedCourse.Name == "")
            {
                    return TypedResults.BadRequest("Course data missing in request");
            }

            Course newCourse = new Course(postedCourse);
            repository.Insert(newCourse);
            await repository.SaveAsync();
            GetCourseDTO response = new GetCourseDTO(newCourse);

            return TypedResults.Created($"/courses/{newCourse.Id}", response);
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> DeleteCourseById(IRepository<Course> repository, int id, ClaimsPrincipal claimsPrincipal)
        {
            if (claimsPrincipal.IsInRole("Teacher") == false)
            {
                return TypedResults.Unauthorized();
            }

            Course? course = await repository.GetByIdAsync(id);
            if(course == null)
            {
                return TypedResults.NotFound($"No course with the given id: {id} was found");
            }
            repository.Delete(course);
            await repository.SaveAsync();

            return TypedResults.NoContent();
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> UpdateCourse(IRepository<Course> repository, int id, CoursePostDTO updatedCourse, ClaimsPrincipal claimsPrincipal)
        {
            if (claimsPrincipal.IsInRole("Teacher") == false)
            {
                return TypedResults.Unauthorized();
            }

            Course? course = await repository.GetByIdAsync(id);
            if (course == null)
            {
                return TypedResults.NotFound($"No course with the given id: {id} was found");
            }
            if(updatedCourse == null || updatedCourse.Name == null || updatedCourse.Name == "")
            {
                return TypedResults.BadRequest("Missing update data in request");
            }
            course.Name = updatedCourse.Name;
            course.SpecialismName = updatedCourse.SpecialismName;
            repository.Update(course);
            await repository.SaveAsync();

            GetCourseDTO response = new GetCourseDTO(course);

            return TypedResults.Ok(response);
        }

}
