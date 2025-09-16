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
using exercise.wwwapi.Models.UserInfo;
using User = exercise.wwwapi.Models.UserInfo.User;
using exercise.wwwapi.DTOs.Notes;
using System.Diagnostics;
using exercise.wwwapi.Models;
using exercise.wwwapi.Utils;
using exercise.wwwapi.Factories;

namespace exercise.wwwapi.EndPoints;

public static class UserEndpoints
{
    private const string GITHUB_URL = "github.com/";

    public static void ConfigureAuthApi(this WebApplication app)
    {
        var users = app.MapGroup("users");
        users.MapPost("/", Register).WithSummary("Create user");
        users.MapGet("/by_cohort/{id}", GetUsersByCohort).WithSummary("Get all users from a cohort");
        users.MapGet("/", GetUsers).WithSummary("Get all users or filter by first name, last name or full name");
        users.MapGet("/{id}", GetUserById).WithSummary("Get user by user id");
        app.MapPost("/login", Login).WithSummary("Localhost Login");
        users.MapPatch("/{id}", UpdateUser).RequireAuthorization().WithSummary("Update a user");
        users.MapDelete("/{id}", DeleteUser).RequireAuthorization().WithSummary("Delete a user");
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    private static async Task<IResult> GetUsers(IRepository<User> userRepository, string? searchTerm,
        ClaimsPrincipal claimPrincipal)
    {
        var results = (await userRepository.GetAllAsync(u => u.Profile, u => u.Credential, u => u.Notes)).ToList();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            results = results.Where(u =>
                    u.Profile.GetFullName().Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
        var userRole = claimPrincipal.Role();
        var authorizedAsTeacher = AuthorizeTeacher(claimPrincipal);

        var userData = new UsersSuccessDTO
        {
            Users = results.Select(user => authorizedAsTeacher 
            ? UserFactory.GetUserDTO(user, PrivilegeLevel.Teacher) 
            : UserFactory.GetUserDTO(user, PrivilegeLevel.Student))
            .ToList()
        };

        var response = new ResponseDTO<UsersSuccessDTO>
        {
            Status = "success",
            Data = userData
        };
        return TypedResults.Ok(response);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    private static async Task<IResult> GetUsersByCohort(IRepository<User> userRepository, int id, ClaimsPrincipal claimsPrincipal)
    {
        var all = await userRepository.GetAllAsync(u => u.Profile, u => u.Credential, u => u.Notes);
        var results = all.Where(u => u.CohortId == id).ToList();

        var userRole = claimsPrincipal.Role();
        var authorizedAsTeacher = AuthorizeTeacher(claimsPrincipal);

        var userData = new UsersSuccessDTO
        {
            Users = results.Select(user => authorizedAsTeacher
           ? UserFactory.GetUserDTO(user, PrivilegeLevel.Teacher)
           : UserFactory.GetUserDTO(user, PrivilegeLevel.Student))
            .ToList()
        };
        var response = new ResponseDTO<UsersSuccessDTO>
        {
            Status = "success",
            Data = userData
        };
        return TypedResults.Ok(response);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    private static async Task<IResult> Register(RegisterRequestDTO request, IRepository<User> userRepository,
        IRepository<Cohort> cohortRepository, IValidator<RegisterRequestDTO> validator)
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

            var failResponse = new ResponseDTO<RegisterFailureDTO> { Status = "fail", Data = failureDto };
            return Results.BadRequest(failResponse);
        }

        // Check if email already exists
        var users = await userRepository.GetAllAsync(user => user.Credential
        );
        if (users.Any(u => u.Credential.Email == request.Email))
        {
            var failureDto = new RegisterFailureDTO();
            failureDto.EmailErrors.Add("Email already exists");

            var failResponse = new ResponseDTO<RegisterFailureDTO> { Status = "fail", Data = failureDto };
            return Results.Conflict(failResponse);
        }

        // Check if CohortId exists
        if (request.CohortId != null)
        {
            var cohort = await cohortRepository.GetByIdAsync(request.CohortId.Value);
            if (cohort == null)
            {
                var failureDto = new RegisterFailureDTO();
                failureDto.EmailErrors.Add("Invalid CohortId");

                var failResponse = new ResponseDTO<RegisterFailureDTO> { Status = "fail", Data = failureDto };
                return Results.BadRequest(failResponse);
            }
        }

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

        var user = new User
        {
            Credential = new Credential
            {
                Username = string.IsNullOrEmpty(request.Username) ? request.Email : request.Username,
                PasswordHash = passwordHash,
                Email = request.Email,
                Role = Role.Student,
            },
            Profile = new Profile
            {
                FirstName = string.IsNullOrEmpty(request.FirstName) ? string.Empty : request.FirstName,
                LastName = string.IsNullOrEmpty(request.LastName) ? string.Empty : request.LastName,
                Bio = string.IsNullOrEmpty(request.Bio) ? string.Empty : request.Bio,
                Github = string.IsNullOrEmpty(request.Github) ? string.Empty : request.Github,
                StartDate = DateTime.MinValue,
                EndDate = DateTime.MinValue,
                Specialism = Specialism.None,
            },
            CohortId = request.CohortId
        };

        userRepository.Insert(user);
        await userRepository.SaveAsync();

        var response = new ResponseDTO<RegisterSuccessDTO>
        {
            Status = "success",
            Data = new RegisterSuccessDTO
            {
                User =
                {
                    Id = user.Id,
                    FirstName = user.Profile.FirstName,
                    LastName = user.Profile.LastName,
                    Bio = user.Profile.Bio,
                    Github = user.Profile.Github,
                    Username = user.Credential.Username,
                    Email = user.Credential.Email,
                    Phone = user.Profile.Phone,
                    StartDate = user.Profile.StartDate,
                    EndDate = user.Profile.EndDate,
                    Specialism = user.Profile.Specialism,
                    CohortId = user.CohortId
                }
            }
        };

        return Results.Ok(response);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    private static async Task<IResult> Login(LoginRequestDTO request, IRepository<User> userRepository,
        IConfigurationSettings configurationSettings)
    {
        var allUsers = await userRepository.GetAllAsync(
            user => user.Credential,
            user => user.Profile
        );
        var user = allUsers.FirstOrDefault(u => u.Credential.Email == request.Email);
        if (user == null)
        {
            return Results.BadRequest(new Payload<object>
            {
                Status = "User does not exist", Data = new { email = "Invalid email and/or password provided" }
            });
        }

        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.Credential.PasswordHash))
        {
            return Results.BadRequest(new Payload<object>
            {
                Status = "fail",
                Data = new LoginFailureDTO()
            });
        }

        var token = CreateToken(user, configurationSettings);

        var response = new ResponseDTO<LoginSuccessDTO>
        {
            Status = "Success",
            Data = new LoginSuccessDTO
            {
                Token = token,
                User = UserFactory.GetUserDTO(user, PrivilegeLevel.Student)
            }
        };
        return Results.Ok(response);
    }

    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public static async Task<IResult> GetUserById(IRepository<User> userRepository, int id, ClaimsPrincipal claimsPrincipal)
    {
        var user = await userRepository.GetByIdAsync(
            id,
            user => user.Credential,
            user => user.Profile,
            user => user.Notes
        );
        if (user == null)
        {
            return TypedResults.NotFound();
        }

        var response = new ResponseDTO<UserDTO>
        {
            Status = "success",
            Data = UserFactory.GetUserDTO(user, PrivilegeLevel.Student)
        };

        var userRole = claimsPrincipal.Role();

        if (userRole == "Teacher")
        {
            response.Data.Notes = user.Notes.Select(note => new NoteResponseDTO
            {
                Id = note.Id,
                Title = note.Title,
                Content = note.Content,
                CreatedAt = note.CreatedAt,
                UpdatedAt = note.UpdatedAt
            }).ToList();
        }

        return TypedResults.Ok(response);
    }

    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public static async Task<IResult> UpdateUser(IRepository<User> userRepository, int id,
        UpdateUserRequestDTO request,
        IValidator<UpdateUserRequestDTO> validator, ClaimsPrincipal claimsPrinciple
    )
    {
        // Only teacher can edit protected fields
        var authorized = AuthorizeTeacher(claimsPrinciple);
        if (!authorized && (request.StartDate is not null 
            || request.EndDate is not null 
            || request.CohortId is not null
            || request.Specialism is not null
            || request.Role is not null))
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

        var user = await userRepository.GetByIdAsync(
            id,
            user => user.Credential,
            user => user.Profile
        );
        if (user == null)
        {
            return TypedResults.NotFound();
        }

        if (request.Username != null) user.Credential.Username = request.Username;
        if (request.Email != null) user.Credential.Email = request.Email;
        if (request.Password != null)
            user.Credential.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        if (request.Phone != null) user.Profile.Phone = request.Phone;
        if (request.Bio != null) user.Profile.Bio = request.Bio;
        if (request.Github != null) user.Profile.Github = GITHUB_URL + request.Github;
        if (request.FirstName != null) user.Profile.FirstName = request.FirstName;
        if (request.LastName != null) user.Profile.LastName = request.LastName;
        if (request.CohortId  != null) user.CohortId = request.CohortId;
        if (request.Specialism != null)
            user.Profile.Specialism = (Specialism)request.Specialism;
        if (request.Role != null)
            user.Credential.Role = (Role)request.Role;

        userRepository.Update(user);
        await userRepository.SaveAsync();

        var response = new ResponseDTO<UpdateUserSuccessDTO>()
        {
            Status = "success",
            Data = new UpdateUserSuccessDTO()
            {
                Id = user.Id,
                Email = user.Credential.Email,
                FirstName = user.Profile.FirstName,
                LastName = user.Profile.LastName,
                Bio = user.Profile.Bio,
                Github = user.Profile.Github,
                Username = user.Credential.Username,
                Phone = user.Profile.Phone,
                CohortId = user.CohortId,
                Specialism = user.Profile.Specialism,
                Role = user.Credential.Role,
                StartDate = user.Profile.StartDate,
                EndDate = user.Profile.EndDate
            }
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
        var userIdClaim = claimsPrincipal.UserRealId();
        if (userIdClaim == null || userIdClaim != id)
        {
            return Results.Unauthorized();
        }

        var user = await userRepository.GetByIdAsync(
            id,
            user => user.Credential,
            user => user.Profile
        );
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
            new(ClaimTypes.Name, user.Credential.Username),
            new(ClaimTypes.Email, user.Credential.Email),
            new(ClaimTypes.Role, user.Credential.Role.ToString())
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