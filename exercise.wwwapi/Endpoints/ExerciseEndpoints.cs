using exercise.wwwapi.Configuration;
using exercise.wwwapi.DTOs;
using exercise.wwwapi.DTOs.GetUsers;
using exercise.wwwapi.DTOs.Login;
using exercise.wwwapi.DTOs.Register;
using exercise.wwwapi.DTOs.UpdateUser;
using exercise.wwwapi.Repository;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using exercise.wwwapi.Enums;
using exercise.wwwapi.Helpers;
using User = exercise.wwwapi.Models.User;
using exercise.wwwapi.DTOs.Notes;
using System.Diagnostics;
using exercise.wwwapi.Models;
using exercise.wwwapi.Factories;
using Microsoft.EntityFrameworkCore;
using exercise.wwwapi.Models.Exercises;
using exercise.wwwapi.DTOs.Exercises;
using System.Linq;

namespace exercise.wwwapi.EndPoints;

public static class ExerciseEndpoints
{
    private const string GITHUB_URL = "github.com/";

    public static void ConfigureExerciseEndpoints(this WebApplication app)
    {
        var exercises = app.MapGroup("exercises");
        exercises.MapGet("/", GetExercises).WithSummary("Returns all exercises");
        exercises.MapGet("/{id}", GetExerciseById).WithSummary("Returns exercise with provided id");
        exercises.MapDelete("/{id}", DeleteExerciseById).WithSummary("Deletes exercise with provided id");
        exercises.MapPut("/{id}", UpdateExerciseById).WithSummary("Update exercise with provided id");

        var units = app.MapGroup("units");
        units.MapGet("/", GetUnits).WithSummary("Returns all units");
        units.MapGet("/{id}", GetUnitById).WithSummary("Returns unit with provided id");
        units.MapPost("/{id}", CreateExerciseInUnit).WithSummary("Create an exercise in the given unit");
        units.MapDelete("/{id}", DeleteUnit).WithSummary("Deletes unit with provided id");
        units.MapPut("/{id}", UpdateUnit).WithSummary("Update unit with provided id");

        var modules = app.MapGroup("modules");
        modules.MapGet("/by_user/{user_id}", GetModulesByUserId).WithSummary("Returns all modules for a given user");
        modules.MapGet("/", GetModules).WithSummary("Returns all modules");
        modules.MapGet("/{id}", GetModuleById).WithSummary("Returns module with provided id");
        modules.MapPost("/", CreateModule).WithSummary("Create a new module");
        modules.MapPut("/{id}", UpdateModule).WithSummary("Update a module with provided id");
        modules.MapDelete("/{id}", DeleteModule).WithSummary("Delete a module with provided id");
        modules.MapPost("/{id}", CreateUnitInModule).WithSummary("Create a unit in the given module");
        //TODO: Add MapPost to modules which creates a new Unit in the module


    }
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)] // will implement if some users should not have access
    private static async Task<IResult> GetModulesByUserId(IRepository<User> userRepository, ClaimsPrincipal claimsPrincipal, int user_id)
    {
        var response = await userRepository.GetByIdWithIncludes(a => a
                                                                        .Include(b => b.User_CC)
                                                                            .ThenInclude(c => c.CohortCourse)
                                                                            .ThenInclude(d => d.Course)
                                                                            .ThenInclude(e => e.CourseModules)
                                                                            .ThenInclude(f => f.Module)
                                                                            .ThenInclude(g => g.Units)
                                                                            .ThenInclude(h => h.Exercises)
                                                                        .Include(i => i.User_Exercises), user_id);

        if (response == null)
        {
            return TypedResults.NotFound("user does not exist");
        }



        var result = response.User_CC.LastOrDefault().CohortCourse.Course.CourseModules.Select(a => new GetModuleForUserDTO(a.Module, response.User_Exercises)).ToList();
        return TypedResults.Ok(result);
    }


    [ProducesResponseType(StatusCodes.Status200OK)]
    private static async Task<IResult> GetModules(IRepository<Module> moduleRepository, ClaimsPrincipal claimsPrincipal)
    {
        var response = await moduleRepository.GetWithIncludes(a => a.Include(u => u.Units).ThenInclude(e => e.Exercises));
        var result = response.Select(u => new GetModuleDTO(u));
        return TypedResults.Ok(result);
    }

    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    private static async Task<IResult> GetModuleById(IRepository<Module> moduleRepository, int id)
    {
        var response = await moduleRepository.GetByIdWithIncludes(a => a.Include(u => u.Units).ThenInclude(u => u.Exercises), id);
        if (response == null)
        {
            return TypedResults.NotFound();
        }

        var result = new GetModuleDTO(response);
        return TypedResults.Ok(result);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    private static async Task<IResult> CreateModule(IRepository<Module> moduleRepository, ClaimsPrincipal claimsPrincipal, string moduleTitle)
    {
        var authorized = claimsPrincipal.IsInRole("Teacher");
        if (!authorized)
        {
            return TypedResults.Unauthorized();
        }

        var newModule = new Module { Title = moduleTitle };

        moduleRepository.Insert(newModule);
        await moduleRepository.SaveAsync();

        var result = new GetModuleDTO(newModule);
        return TypedResults.Ok(result);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    private static async Task<IResult> UpdateModule(IRepository<Module> moduleRepository, ClaimsPrincipal claimsPrincipal, int id, string moduleTitle)
    {
        var authorized = claimsPrincipal.IsInRole("Teacher");
        if (!authorized)
        {
            return TypedResults.Unauthorized();
        }

        var response = await moduleRepository.GetByIdWithIncludes(a => a.Include(u => u.Units).ThenInclude(u => u.Exercises), id);
        if (response == null)
        {
            return TypedResults.NotFound();
        }

        response.Title = moduleTitle;
        moduleRepository.Update(response);
        await moduleRepository.SaveAsync();

        var result = new GetModuleDTO(response);
        return TypedResults.Ok(result);
    }
    // TODO: Create a DTO to make it easier to later change what module might hold/need when creating/editing a module

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    private static async Task<IResult> DeleteModule(IRepository<Module> moduleRepository, ClaimsPrincipal claimsPrincipal, int id)
    {
        var authorized = claimsPrincipal.IsInRole("Teacher");
        if (!authorized)
        {
            return TypedResults.Unauthorized();
        }

        var response = await moduleRepository.GetByIdWithIncludes(a => a.Include(u => u.Units).ThenInclude(u => u.Exercises), id);
        if (response == null)
        {
            return TypedResults.NotFound();
        }

        moduleRepository.Delete(response);
        await moduleRepository.SaveAsync();

        var result = new GetModuleDTO(response);
        return TypedResults.Ok(result);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    private static async Task<IResult> CreateUnitInModule(IRepository<Module> moduleRepository, IRepository<Unit> unitRepository, ClaimsPrincipal claimsPrincipal,int id, string name)
    {
        var authorized = claimsPrincipal.IsInRole("Teacher");
        if (!authorized)
        {
            return TypedResults.Unauthorized();
        }

        var module = await moduleRepository.GetByIdWithIncludes(a => a.Include(u => u.Units).ThenInclude(u => u.Exercises), id);
        if (module == null)
        {
            return TypedResults.NotFound();
        }

        var newUnit = new Unit { ModuleId = id, Name = name };

        unitRepository.Insert(newUnit);
        await unitRepository.SaveAsync();

        var result = new GetUnitDTO(newUnit);
        return TypedResults.Ok(result);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    private static async Task<IResult> GetUnits(IRepository<Unit> unitRepository, ClaimsPrincipal claimsPrincipal)
    {
        var response = await unitRepository.GetWithIncludes(a => a.Include(u => u.Exercises));
        var result = response.Select(u => new GetUnitDTO(u));
        return TypedResults.Ok(result);
    }

    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    private static async Task<IResult> GetUnitById(IRepository<Unit> unitRepository, int id)
    {
        var response = await unitRepository.GetByIdWithIncludes(a => a.Include(u => u.Exercises), id);
        if (response == null)
        {
            return TypedResults.NotFound();
        }
        var result = new GetUnitDTO(response);
        return TypedResults.Ok(result);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    private static async Task<IResult> DeleteUnit(IRepository<Unit> unitRepository, ClaimsPrincipal claimsPrincipal, int id)
    {
        var authorized = claimsPrincipal.IsInRole("Teacher");
        if (!authorized)
        {
            return TypedResults.Unauthorized();
        }

        var response = await unitRepository.GetByIdWithIncludes(a => a.Include(u => u.Exercises), id);
        if (response == null)
        {
            return TypedResults.NotFound();
        }

        unitRepository.Delete(response);
        await unitRepository.SaveAsync();

        var result = new GetUnitDTO(response);
        return TypedResults.Ok(result);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    private static async Task<IResult> UpdateUnit(IRepository<Unit> unitRepository, ClaimsPrincipal claimsPrincipal,int id, string name)
    {
        var authorized = claimsPrincipal.IsInRole("Teacher");
        if (!authorized)
        {
            return TypedResults.Unauthorized();
        }

        var response = await unitRepository.GetByIdWithIncludes(a => a.Include(u => u.Exercises), id);
        if (response == null)
        {
            return TypedResults.NotFound();
        }
        response.Name = name;
        unitRepository.Update(response);
        await unitRepository.SaveAsync();

        var result = new GetUnitDTO(response);
        return TypedResults.Ok(result);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    private static async Task<IResult> CreateExerciseInUnit(
    IRepository<Exercise> exerciseRepository,
    IRepository<Unit> unitRepository,
    ClaimsPrincipal claimsPrincipal,
    int id,
    UpdateCreateExerciseDTO exerciseDTO)
    {
        var authorized = claimsPrincipal.IsInRole("Teacher");
        if (!authorized)
        {
            return TypedResults.Unauthorized();
        }

        var unit = await unitRepository.GetByIdWithIncludes(null, id);
        if (unit == null)
        {
            return TypedResults.NotFound();
        }

        var newExercise = new Exercise { UnitId = id, Name = exerciseDTO.Name, GitHubLink = exerciseDTO.GitHubLink, Description = exerciseDTO.Description };
        exerciseRepository.Insert(newExercise);
        await exerciseRepository.SaveAsync();

        var result = new Exercise_noUnit(newExercise);
        return TypedResults.Ok(result);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    private static async Task<IResult> GetExercises(IRepository<Exercise> exerciseRepository, ClaimsPrincipal claimsPrincipal)
    {
        var response = await exerciseRepository.GetWithIncludes(null);
        var result = response.Select(e => new Exercise_noUnit(e));
        return TypedResults.Ok(result);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    private static async Task<IResult> GetExerciseById(IRepository<Exercise> exerciseRepository, int id)
    {
        var response = await exerciseRepository.GetByIdWithIncludes(null, id);
        if (response == null)
        {
            return TypedResults.NotFound();
        }
        var result = new Exercise_noUnit(response);
        return TypedResults.Ok(result);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    private static async Task<IResult> DeleteExerciseById(IRepository<Exercise> exerciseRepository, ClaimsPrincipal claimsPrincipal, int id)
    {
        var authorized = claimsPrincipal.IsInRole("Teacher");
        if (!authorized)
        {
            return TypedResults.Unauthorized();
        }

        var response = await exerciseRepository.GetByIdWithIncludes(null, id);
        if (response == null)
        {
            return TypedResults.NotFound();
        }

        exerciseRepository.Delete(response);
        await exerciseRepository.SaveAsync();

        var result = new Exercise_noUnit(response);
        return TypedResults.Ok(result);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    private static async Task<IResult> UpdateExerciseById(IRepository<Exercise> exerciseRepository, IRepository<Unit> unitRepository, ClaimsPrincipal claimsPrincipal, int id, UpdateWithNewUnitExerciseDTO exerciseDTO)
    {
        var authorized = claimsPrincipal.IsInRole("Teacher");
        if (!authorized)
        {
            return TypedResults.Unauthorized();
        }

        var unit = await unitRepository.GetByIdWithIncludes(null, exerciseDTO.UnitId);
        if ( unit == null)
        {
            return TypedResults.NotFound("Couldn't fint Unit");
        }
        var response = await exerciseRepository.GetByIdWithIncludes(null, id);
        if (response == null)
        {
            return TypedResults.NotFound();
        }

        response.Name = exerciseDTO.Name;
        response.Description = exerciseDTO.Description;
        response.GitHubLink = exerciseDTO.GitHubLink;
        response.UnitId = exerciseDTO.UnitId;

        exerciseRepository.Update(response);
        await exerciseRepository.SaveAsync();

        var result = new Exercise_noUnit(response);
        return TypedResults.Ok(result);

    }

}