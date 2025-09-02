using exercise.wwwapi.Configuration;
using exercise.wwwapi.DTOs;
using exercise.wwwapi.DTOs.GetUsers;
using exercise.wwwapi.DTOs.Login;
using exercise.wwwapi.DTOs.Register;
using exercise.wwwapi.DTOs.UpdateUser;
using exercise.wwwapi.Models;
using exercise.wwwapi.Repository;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using exercise.wwwapi.DTOs.GetUser;
using exercise.wwwapi.Models.UserInfo;

namespace exercise.wwwapi.EndPoints
{
    public static class UserEndpoints
    {
        private const string GITHUB_URL = "github.com/";

        public static void ConfigureAuthApi(this WebApplication app)
        {
            var users = app.MapGroup("users");
            users.MapPost("/", Register).WithSummary("Create user");
            users.MapGet("/", GetUsers).WithSummary("Get all users by first name if provided");
            users.MapGet("/{id}", GetUserById).WithSummary("Get user by user id");
            app.MapPost("/login", Login).WithSummary("Localhost Login");
            users.MapPatch("/{id}", UpdateUser).WithSummary("Update a user");
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> GetUsers(IUserRepository userRepository, string? firstName,
            ClaimsPrincipal user)
        {
            var results = (await userRepository.GetAllUsers()).ToList();

            var userData = new UsersSuccessDTO
            {
                Users = string.IsNullOrEmpty(firstName)
                    ? results : results
                        .Where(u => u.Profile.FirstName
                            .Equals(firstName, StringComparison.OrdinalIgnoreCase))
                        .ToList(),
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
        private static async Task<IResult> Register(RegisterRequestDTO request, IUserRepository userRepository,
            IValidator<RegisterRequestDTO> validator)
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
            var users = await userRepository.GetAllUsers();
            if (users.Any(u => u.Credential.Email == request.Email))
            {
                var failureDto = new RegisterFailureDTO();
                failureDto.EmailErrors.Add("Email already exists");

                var failResponse = new ResponseDTO<RegisterFailureDTO> { Status = "fail", Data = failureDto };
                return Results.Conflict(failResponse);
            }

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var user = new User();
            user.Credential.Username = request.Username;
            user.Credential.PasswordHash = passwordHash;
            user.Credential.Email = request.Email;
            user.Credential.Role = Role.Student;
            user.Profile.FirstName = string.IsNullOrEmpty(request.FirstName) ? string.Empty : request.FirstName;
            user.Profile.LastName = string.IsNullOrEmpty(request.LastName) ? string.Empty : request.LastName;
            user.Profile.Bio = string.IsNullOrEmpty(request.Bio) ? string.Empty : request.Bio;
            user.Profile.Github = string.IsNullOrEmpty(request.GithubUrl) ? string.Empty : request.GithubUrl;

            user = await userRepository.CreateUser(user);

            var response = new ResponseDTO<RegisterSuccessDTO>();
            response.Status = "success";
            response.Data.User.Id = user.Id;
            response.Data.User.FirstName = user.Profile.FirstName;
            response.Data.User.LastName = user.Profile.LastName;
            response.Data.User.Bio = user.Profile.Bio;
            response.Data.User.Github = user.Profile.Github;
            response.Data.User.Username = user.Credential.Username;
            response.Data.User.Email = user.Credential.Email;
            response.Data.User.Phone = user.Profile.Phone;

            return Results.Ok(response);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        private static async Task<IResult> Login(LoginRequestDTO request, IUserRepository userRepository,
            IConfigurationSettings config)
        {
            var allUsers = await userRepository.GetAllUsers();
            var user = allUsers.FirstOrDefault(u => u.Credential.Email == request.Email);
            if (user == null)
            {
                return Results.BadRequest(new Payload<object>()
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

            var token = CreateToken(user, config);

            var response = new ResponseDTO<LoginSuccessDTO>();
            response.Data.User.Id = user.Id;
            response.Data.User.Credential.Email = user.Credential.Email;
            response.Data.User.Profile.FirstName = user.Profile.FirstName;
            response.Data.User.Profile.LastName = user.Profile.LastName;
            response.Data.User.Profile.Bio = user.Profile.Bio;
            response.Data.User.Profile.Github = user.Profile.Github;
            response.Data.User.Profile.Phone = user.Profile.Phone;

            response.Data.Token = token;

            return Results.Ok(response);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        public static async Task<IResult> GetUserById(IUserRepository userRepository, int id)
        {
            var user = await userRepository.GetUserById(id);
            if (user == null)
            {
                return TypedResults.NotFound();
            }

            var response = new ResponseDTO<GetUserSuccessDTO>();
            response.Status = "success";
            response.Data.Id = user.Id;
            response.Data.FirstName = user.Profile.FirstName;
            response.Data.LastName = user.Profile.LastName;
            response.Data.Bio = user.Profile.Bio;
            response.Data.Github = user.Profile.Github;
            response.Data.Username = user.Credential.Username;
            response.Data.Email = user.Credential.Email;
            response.Data.Phone = user.Profile.Phone;
            return TypedResults.Ok(response);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        public static async Task<IResult> UpdateUser(IUserRepository userRepository, int id,
            UpdateUserRequestDTO request,
            IValidator<UpdateUserRequestDTO> validator)
        {
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

            var user = await userRepository.GetUserById(id);
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

            await userRepository.UpdateUser(user);

            var response = new ResponseDTO<UpdateUserSuccessDTO>();
            response.Status = "success";
            response.Data.Id = user.Id;
            response.Data.Email = user.Credential.Email;
            response.Data.FirstName = user.Profile.FirstName;
            response.Data.LastName = user.Profile.LastName;
            response.Data.Bio = user.Profile.Bio;
            response.Data.Github = user.Profile.Github;
            response.Data.Username = user.Credential.Username;
            response.Data.Phone = user.Profile.Phone;

            return TypedResults.Ok(response);
        }

        private static string CreateToken(User user, IConfigurationSettings config)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.Sid, user.Id.ToString()),
                new(ClaimTypes.Name, user.Credential.Username),
                new(ClaimTypes.Email, user.Credential.Email)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetValue("AppSettings:Token")));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials
            );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }
    }
}