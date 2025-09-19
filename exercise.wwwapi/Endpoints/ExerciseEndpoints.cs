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

        var modules = app.MapGroup("modules");
        modules.MapGet("/", GetModules).WithSummary("Returns all modules");
        modules.MapGet("/{id}", GetModuleById).WithSummary("Returns module with provided id");


    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    private static async Task<IResult> GetModules(IRepository<Module> moduleRepository, ClaimsPrincipal claimsPrincipal)
    {
        var response = await moduleRepository.GetWithIncludes(a => a.Include(u => u.Units).ThenInclude(e => e.Exercises));
        var result = response.Select(u => new GetModuleDTO(u));
        return TypedResults.Ok(result);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    private static async Task<IResult> GetModuleById(IRepository<Module> moduleRepository, int id)
    {
        var response = await moduleRepository.GetByIdWithIncludes(a => a.Include(u => u.Units).ThenInclude(u => u.Exercises), id);
        var result = new GetModuleDTO(response);
        return TypedResults.Ok(result);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    private static async Task<IResult> GetUnits(IRepository<Unit> unitRepository, ClaimsPrincipal claimsPrincipal)
    {
        var response = await unitRepository.GetWithIncludes(a => a.Include(u => u.Exercises));
        var result = response.Select(u => new GetUnitDTO(u));
        return TypedResults.Ok(result);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    private static async Task<IResult> GetUnitById(IRepository<Unit> unitRepository, int id)
    {
        var response = await unitRepository.GetByIdWithIncludes(a => a.Include(u => u.Exercises), id);
        var result = new GetUnitDTO(response);
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
    private static async Task<IResult> GetExerciseById(IRepository<Exercise> exerciseRepository, int id)
    {
        var response = await exerciseRepository.GetByIdWithIncludes(null, id);
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
            return Results.Unauthorized();
        }

        var response = await exerciseRepository.GetByIdWithIncludes(null, id);
        if (response == null)
        {
            return TypedResults.NotFound();
        }

        var result = new Exercise_noUnit(response);
        return TypedResults.Ok(result);
    }

    private static async Task<IResult> UpdateExerciseById(IRepository<Exercise> exerciseRepository, ClaimsPrincipal claimsPrincipal, int id, UpdateCreateExerciseDTO exerciseDTO)
    {
        var authorized = claimsPrincipal.IsInRole("Teacher");
        if (!authorized)
        {
            return Results.Unauthorized();
        }

        var response = await exerciseRepository.GetByIdWithIncludes(null, id);
        if (response == null)
        {
            return TypedResults.NotFound();
        }

        response.Name = exerciseDTO.Name;
        response.Description = exerciseDTO.Description;
        response.GitHubLink = exerciseDTO.GitHubLink;

        exerciseRepository.Update(response);
        await exerciseRepository.SaveAsync();

        var result = new Exercise_noUnit(response);
        return TypedResults.Ok(result);

    }

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
            return Results.Unauthorized();
        }

        var unit = await unitRepository.GetByIdWithIncludes(null, id);
        if (unit == null)
        {
            return TypedResults.NotFound();
        }

        var newExercise = new Exercise { UnitId = id, Name = exerciseDTO.Name, GitHubLink = exerciseDTO.GitHubLink, Description = exerciseDTO.Description, Unit = unit };
        exerciseRepository.Insert(newExercise);
        await exerciseRepository.SaveAsync();

        var result = new Exercise_noUnit(newExercise);
        return TypedResults.Ok(result);
    }




}