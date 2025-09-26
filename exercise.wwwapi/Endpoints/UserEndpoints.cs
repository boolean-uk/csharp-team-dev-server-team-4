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
using exercise.wwwapi.DTOs.Users;

namespace exercise.wwwapi.EndPoints;

public static class UserEndpoints
{
    private const string GITHUB_URL = "github.com/";

    public static void ConfigureAuthApi(this WebApplication app)
    {
        var users = app.MapGroup("users");
        users.MapPost("/", Register).WithSummary("Create user"); //OKOKOK
        users.MapGet("/by_cohortcourse/{cc_id}", GetUsersByCohortCourse).WithSummary("Get all users from a cohortCourse"); //OKOKOK
        users.MapGet("/by_cohort/{cohort_id}", GetUsersByCohort).WithSummary("Get all users from a cohort"); //OKOKOK
        users.MapGet("/", GetUsers).WithSummary("Get all users or filter by first name, last name or full name");//OKOKOK
        users.MapGet("/{id}", GetUserById).WithSummary("Get user by user id"); //OKOKOK
        app.MapPost("/login", Login).WithSummary("Localhost Login"); //OKOKOK
        users.MapPatch("/{id}", UpdateUser).RequireAuthorization().WithSummary("Update a user");//OKOKOK
        users.MapDelete("/{id}", DeleteUser).RequireAuthorization().WithSummary("Delete a user");
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    private static async Task<IResult> GetUsers(IRepository<User> userRepository, string? searchTerm,
        ClaimsPrincipal claimPrincipal)
    {
        var results = await userRepository.GetWithIncludes(x => x
                                                                .Include(u => u.User_CC)
                                                                    .ThenInclude(c => c.CohortCourse)
                                                                    .ThenInclude(d => d.Cohort)
                                                                .Include(u => u.User_CC)
                                                                    .ThenInclude(c => c.CohortCourse)
                                                                    .ThenInclude(d => d.Course)
                                                                .Include(p => p.Notes));

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var terms = searchTerm
            .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(t => t.ToLowerInvariant())
            .ToArray();

            results = results.Where(u =>
            {
                var first = u.FirstName?.ToLowerInvariant() ?? "";
                var last = u.LastName?.ToLowerInvariant() ?? "";
                var full = (first + " " + last).Trim();

                // All search terms must be present in either first, last, or full name (order-insensitive)
                return terms.All(term =>
                    first.Contains(term) ||
                    last.Contains(term) ||
                    full.Contains(term)
                );
            }).ToList();
        }
        var userRole = claimPrincipal.Role();
        var authorizedAsTeacher = AuthorizeTeacher(claimPrincipal);



        var userData = new UsersSuccessDTO
        {
            Users = results.Select(user => authorizedAsTeacher
            ? new UserDTO(user, PrivilegeLevel.Teacher) //if teacher loads students, also load notes for students.
            : new UserDTO(user, PrivilegeLevel.Student)).ToList() //if teacher loads students, also load notes for students.
        };

        var response = new ResponseDTO<UsersSuccessDTO>
        {
            Status = "success",
            Data = userData
        };

        return TypedResults.Ok(response);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    private static async Task<IResult> GetUsersByCohort(IRepository<Cohort> repository, int cohort_id, ClaimsPrincipal claimsPrincipal)
    {
        var response = await repository.GetWithIncludes(a => a
                                                                .Include(p => p.CohortCourses)
                                                                    .ThenInclude(b => b.Course)
                                                                .Include(p => p.CohortCourses)
                                                                    .ThenInclude(b => b.UserCCs)
                                                                    .ThenInclude(a => a.User)
                                                                    .ThenInclude(u => u.Notes));

        var cohortCourses = response.SelectMany(c => c.CohortCourses).ToList();
        var userCohortCourses = cohortCourses.SelectMany(cc => cc.UserCCs).ToList();
        var groupedUserCohortCourses = userCohortCourses.GroupBy(uc => uc.UserId).ToList();
        var currentUserCohortCourses = groupedUserCohortCourses.Select(g => g.OrderBy(f => f.Id).LastOrDefault()).ToList();
        var userCohortCoursesInCohort = currentUserCohortCourses.Where(f => f.CohortCourse.CohortId == cohort_id).ToList();
        var usersInCohort = userCohortCoursesInCohort.Select(uc => uc.User).ToList();




        var userRole = claimsPrincipal.Role();
        var authorizedAsTeacher = AuthorizeTeacher(claimsPrincipal);

        var userData = new UsersSuccessDTO
        {
            Users = usersInCohort.Select(user => authorizedAsTeacher
            ? new UserDTO(user, PrivilegeLevel.Teacher) //if teacher loads students, also load notes for students.
            : new UserDTO(user, PrivilegeLevel.Student)).ToList() //if teacher loads students, also load notes for students.
        };

        var responseObject = new ResponseDTO<UsersSuccessDTO>
        {
            Status = "success",
            Data = userData
        };

        return TypedResults.Ok(responseObject);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    private static async Task<IResult> GetUsersByCohortCourse(IRepository<CohortCourse> ccRepository, int cc_id, ClaimsPrincipal claimsPrincipal)
    {
        var response = await ccRepository.GetWithIncludes(a => a
                                                                .Include(z => z.Cohort)
                                                                .Include(z => z.Course)
                                                                .Include(b => b.UserCCs)
                                                                    .ThenInclude(a => a.User)                                                                   
                                                                    .ThenInclude(u => u.Notes));

        var userCohortCourses = response.SelectMany(cc => cc.UserCCs).ToList();
        var groupedUserCohortCourses = userCohortCourses.GroupBy(uc => uc.UserId).ToList();
        var currentUserCohortCourses = groupedUserCohortCourses.Select(g => g.OrderBy(f => f.Id).LastOrDefault()).ToList();
        var userCohortCoursesInCohortCourse = currentUserCohortCourses.Where(f => f.CohortCourse.Id == cc_id).ToList();
        var usersInCohortCourse = userCohortCoursesInCohortCourse.Select(uc => uc.User).ToList();



        var userRole = claimsPrincipal.Role();
        var authorizedAsTeacher = AuthorizeTeacher(claimsPrincipal);

        var userData = new UsersSuccessDTO
        {
            Users = usersInCohortCourse.Select(user => authorizedAsTeacher
            ? new UserDTO(user, PrivilegeLevel.Teacher) //if teacher loads students, also load notes for students.
            : new UserDTO(user, PrivilegeLevel.Student)).ToList() //if teacher loads students, also load notes for students.
        };

        var responseObject = new ResponseDTO<UsersSuccessDTO>
        {
            Status = "success",
            Data = userData
        };

        return TypedResults.Ok(responseObject);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    private static async Task<IResult> Register(PostUserDTO request, IRepository<User> userRepository,
        IRepository<Cohort> cohortRepository, IValidator<PostUserDTO> validator)
    {
        // Validate
        var validation = await validator.ValidateAsync(request);
        if (!validation.IsValid)
        {
            var failureDto = new RegisterFailureDTO();

            foreach (var error in validation.Errors)
            {
                if (error.PropertyName.Equals("email", StringComparison.OrdinalIgnoreCase))
                    failureDto.EmailErrors.Add(error.ErrorMessage);
                else if (error.PropertyName.Equals("password", StringComparison.OrdinalIgnoreCase))
                    failureDto.PasswordErrors.Add(error.ErrorMessage);
            }

            var failResponse = new ResponseDTO<RegisterFailureDTO> { Status = "conflict", Data = failureDto };
            return Results.BadRequest(failResponse);
        }

        var response = await userRepository.GetWithIncludes(x => x.Where(u => u.Email == request.Email)); // uses where-statement to filter data before fetching
        if (response.Count == 1)
        {
            return Results.Conflict(new Payload<object>
            {
                Status = "fail",
                Data = "User already exists"
            });
        }





        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

        var user = new User
        {
            Username = string.IsNullOrEmpty(request.Username) ? request.Email : request.Username,
            PasswordHash = passwordHash,
            Email = request.Email,
            Role = Role.Student,
            FirstName = string.IsNullOrEmpty(request.FirstName) ? string.Empty : request.FirstName,
            LastName = string.IsNullOrEmpty(request.LastName) ? string.Empty : request.LastName,
            Mobile = string.IsNullOrEmpty(request.Mobile) ? string.Empty : request.Mobile,
            Bio = string.IsNullOrEmpty(request.Bio) ? string.Empty : request.Bio,
            Github = string.IsNullOrEmpty(request.Github) ? string.Empty : request.Github,
            PhotoUrl = ""
        };

        userRepository.Insert(user);
        await userRepository.SaveAsync();

        var responseObject = new ResponseDTO<RegisterSuccessDTO>
        {
            Status = "success",
            Data = new RegisterSuccessDTO
            {
                User =
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Bio = user.Bio,
                    Github = user.Github,
                    Username = user.Username,
                    Email = user.Email,
                    Mobile = user.Mobile,
                }
            }
        };


        return Results.Ok(responseObject);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    private static async Task<IResult> Login(LoginRequestDTO request, IRepository<User> userRepository,
        IConfigurationSettings configurationSettings)
    {
        var response = await userRepository.GetWithIncludes(x => x.Where(u => u.Email == request.Email)); // uses where-statement to filter data before fetching
        if (response.Count == 0)
        {
            return Results.BadRequest(new Payload<object>
            {
                Status = "User does not exist",
                Data = new { email = "Invalid email and/or password provided" }
            });
        }
        var user = response.FirstOrDefault();


        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            return Results.BadRequest(new Payload<object>
            {
                Status = "fail",
                Data = new LoginFailureDTO()
            });
        }

        var token = CreateToken(user, configurationSettings);

        var result = new ResponseDTO<LoginSuccessDTO>
        {
            Status = "Success",
            Data = new LoginSuccessDTO
            {
                Token = token,
                User = UserFactory.GetUserDTO(user, PrivilegeLevel.Student)
            }
        };
        return Results.Ok(result);
    }

    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public static async Task<IResult> GetUserById(IRepository<User> userRepository, int id, ClaimsPrincipal claimsPrincipal)
    {
        var response = await userRepository.GetByIdWithIncludes(x => x
                                                                .Include(u => u.User_CC)
                                                                    .ThenInclude(c => c.CohortCourse)
                                                                    .ThenInclude(d => d.Cohort)
                                                                .Include(u => u.User_CC)
                                                                    .ThenInclude(c => c.CohortCourse)
                                                                    .ThenInclude(d => d.Course)
                                                                .Include(p => p.Notes), id);

        if (response == null)
        {
            return TypedResults.NotFound();
        }

        var userRole = claimsPrincipal.Role();
        var authorizedAsTeacher = AuthorizeTeacher(claimsPrincipal);

        var userData = authorizedAsTeacher
            ? new UserDTO(response, PrivilegeLevel.Teacher) //if teacher loads students, also load notes for students.
            : new UserDTO(response, PrivilegeLevel.Student); //if teacher loads students, also load notes for students.
        

        var responseObject = new ResponseDTO<UserDTO>
        {
            Status = "success",
            Data = userData
        };

        return TypedResults.Ok(responseObject);
    }



    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public static async Task<IResult> UpdateUser(IRepository<User> userRepository, int id,
        PatchUserDTO request,
        IValidator<PatchUserDTO> validator, ClaimsPrincipal claimsPrinciple
    )
    {
        // Only teacher can edit protected fields
        var authorized = AuthorizeTeacher(claimsPrinciple);
        if (!authorized && (request.Role is not null))
        {
            return Results.Unauthorized();
        }

        // Student can edit only own profile
        var userIdClaim = claimsPrinciple.UserRealId();
        if (AuthorizeStudent(claimsPrinciple) && (userIdClaim is null || userIdClaim != id))
        {
            return Results.Unauthorized();
        }

        // Only user can edit its own  password
        if ((userIdClaim is null || userIdClaim != id) && request.Password is not null)
        {
            return Results.Unauthorized();
        }

        var validation = await validator.ValidateAsync(request);
        if (!validation.IsValid)
        {
            var failureDto = new UpdateUserFailureDTO();
            foreach (var error in validation.Errors)
            {
                if (error.PropertyName.Equals("Email", StringComparison.OrdinalIgnoreCase))
                    failureDto.EmailErrors.Add(error.ErrorMessage);
                else if (error.PropertyName.Equals("Password", StringComparison.OrdinalIgnoreCase))
                    failureDto.PasswordErrors.Add(error.ErrorMessage);
                else if (error.PropertyName.Equals("MobileNumber", StringComparison.OrdinalIgnoreCase))
                    failureDto.MobileNumberErrors.Add(error.ErrorMessage);
            }

            var failResponse = new ResponseDTO<UpdateUserFailureDTO>
            {
                Status = "fail",
                Data = failureDto
            };
            return Results.BadRequest(failResponse);
        }


        var user = await userRepository.GetByIdAsync(id);
        if (user == null)
        {
            return TypedResults.NotFound();
        }

        if (request.Username != null) user.Username = request.Username;
        if (request.Email != null) user.Email = request.Email;
        if (request.Password != null)
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        if (request.Mobile != null) user.Mobile = request.Mobile;
        if (request.Bio != null) user.Bio = request.Bio;
        if (request.Github != null) user.Github = GITHUB_URL + request.Github;
        if (request.FirstName != null) user.FirstName = request.FirstName;
        if (request.LastName != null) user.LastName = request.LastName;
        if (request.Role != null)
            user.Role = (Role)request.Role;

        userRepository.Update(user);
        await userRepository.SaveAsync();

        var result = await userRepository.GetByIdWithIncludes(x => x
                                                                .Include(u => u.User_CC)
                                                                    .ThenInclude(c => c.CohortCourse)
                                                                    .ThenInclude(d => d.Cohort)
                                                                .Include(u => u.User_CC)
                                                                    .ThenInclude(c => c.CohortCourse)
                                                                    .ThenInclude(d => d.Course)
                                                                .Include(p => p.Notes), id);

        var response = new ResponseDTO<UserDTO>()
        {
            Status = "success",
            Data = new UserDTO(result)
        };

        return TypedResults.Ok(response);
    }

    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public static async Task<IResult> DeleteUser(IRepository<User> userRepository, int id,
        ClaimsPrincipal claimsPrincipal)
    {
        var userRole = claimsPrincipal.Role();
        var authorizedAsTeacher = AuthorizeTeacher(claimsPrincipal);

        var userIdClaim = claimsPrincipal.UserRealId();
        if (!authorizedAsTeacher && (userIdClaim == null || userIdClaim != id))
        {
            return Results.Unauthorized();
        }

        var user = await userRepository.GetByIdWithIncludes(x => x
                                                                .Include(u => u.User_CC)
                                                                    .ThenInclude(c => c.CohortCourse)
                                                                    .ThenInclude(d => d.Cohort)
                                                                .Include(u => u.User_CC)
                                                                    .ThenInclude(c => c.CohortCourse)
                                                                    .ThenInclude(d => d.Course)
                                                                .Include(p => p.Notes), id);
        if (user == null)
        {
            return TypedResults.NotFound();
        }

        userRepository.Delete(user);
        await userRepository.SaveAsync();

        var response = new ResponseDTO<UserDTO>
        {
            Status = "success",
            Data = UserFactory.GetUserDTO(user, PrivilegeLevel.Student)
        };

        return TypedResults.Ok(response);
    }

    private static string CreateToken(User user, IConfigurationSettings configurationSettings)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Sid, user.Id.ToString()),
            new(ClaimTypes.Name, user.Username),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Role, user.Role.ToString()),
            new("FirstName", user.FirstName),
            new("LastName", user.LastName)
        };

        var tokenKey = Environment.GetEnvironmentVariable(Globals.EnvironmentEnvVariable) == "Staging"
            ? Globals.TestTokenKey
            : Globals.TokenKey;
        var rawToken = configurationSettings.GetValue(tokenKey);
        if (rawToken == null)
        {
            throw new Exception($"TokenKey: {tokenKey} could not be found.");
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(rawToken));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: credentials
        );
        var jwt = new JwtSecurityTokenHandler().WriteToken(token);
        return jwt;
    }
    private static bool AuthorizeTeacher(ClaimsPrincipal claims)
    {
        if (claims.IsInRole("Teacher"))
        {
            return true;
        }

        return false;
    }

    private static bool AuthorizeStudent(ClaimsPrincipal claims)
    {
        if (claims.IsInRole("Student"))
        {
            return true;
        }

        return false;
    }
}
