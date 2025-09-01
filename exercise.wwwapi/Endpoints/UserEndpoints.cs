using exercise.wwwapi.Configuration;
using exercise.wwwapi.DTOs;
using exercise.wwwapi.DTOs.GetUsers;
using exercise.wwwapi.DTOs.Login;
using exercise.wwwapi.DTOs.Register;
using exercise.wwwapi.Helpers;
using exercise.wwwapi.Models;
using exercise.wwwapi.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace exercise.wwwapi.EndPoints
{
    public static class UserEndpoints
    {
        public static void ConfigureAuthApi(this WebApplication app)
        {
            var users = app.MapGroup("users");
            users.MapPost("/", Register).WithSummary("Create user");
            users.MapGet("/", GetUsers).WithSummary("Get all users by first name if provided");
            users.MapGet("/{id}", GetUserById).WithSummary("Get user by user id");
            app.MapPost("/login", Login).WithSummary("Localhost Login");
            app.MapPatch("/{id}", UpdateUser).WithSummary("Update a user");
        }
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> GetUsers(IRepository<User> service, string? firstName, ClaimsPrincipal user)
        {
            IEnumerable<User> results = await service.Get();
            UsersSuccessDTO userData = new UsersSuccessDTO() { Users = !string.IsNullOrEmpty(firstName) ? results.Where(i => i.Credential.Email.Contains(firstName)).ToList() : results.ToList() };
            ResponseDTO<UsersSuccessDTO> response = new ResponseDTO<UsersSuccessDTO>() { Status = "success", Data = userData };
            return TypedResults.Ok(response);
        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        private static async Task<IResult> Register(RegisterRequestDTO request, IRepository<User> service)
        {
            //user exists
            if (service.GetAll().Where(u => u.Credential.Email == request.email).Any()) return Results.Conflict(new ResponseDTO<RegisterFailureDTO>() { Status = "Fail" });
            


            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.password);

            var user = new User();

            user.Credential.Username = !string.IsNullOrEmpty(request.username) ? request.username : request.email;
            user.Credential.PasswordHash = passwordHash;
            user.Credential.Email = request.email;
            user.Profile.FirstName = !string.IsNullOrEmpty(request.firstName) ? request.firstName : string.Empty;
            user.Profile.LastName = !string.IsNullOrEmpty(request.lastName) ? request.lastName : string.Empty;
            user.Profile.Bio = !string.IsNullOrEmpty(request.bio) ? request.bio : string.Empty;
            user.Profile.Github = !string.IsNullOrEmpty(request.githubUrl) ? request.githubUrl : string.Empty;

            service.Insert(user);
            service.Save();

            ResponseDTO<RegisterSuccessDTO> response = new ResponseDTO<RegisterSuccessDTO>();
            response.Status = "success";
            response.Data.user.firstName = user.Profile.FirstName;
            response.Data.user.lastName = user.Profile.LastName;
            response.Data.user.bio = user.Profile.Bio;
            response.Data.user.githubUrl = user.Profile.Github;
            response.Data.user.username = user.Credential.Username;
            response.Data.user.email = user.Credential.Email;


            return Results.Ok(response);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        private static async Task<IResult> Login(LoginRequestDTO request, IRepository<User> service, IConfigurationSettings config)
        {
            var email = request.email?.Trim().ToLowerInvariant();
            var password = request.password;

            //if (string.IsNullOrEmpty(request.username)) request.username = request.email;

            //user doesn't exist
            if (!service.GetAll().Where(u => u.Credential.Email == request.email).Any()) return Results.BadRequest(new Payload<Object>() { status = "User does not exist", data = new { email="Invalid email and/or password provided"} });

            var user = service.GetAll().FirstOrDefault(u => u.Credential.Email.ToLower() == email);

            if (user is null)
            {
                return Results.BadRequest(new Payload<object> { status = "User does not exist", data = new { email = "Invalid email and/or password provided" }});
            }

            if (!BCrypt.Net.BCrypt.Verify(request.password, user.Credential.PasswordHash))
            {
                return Results.BadRequest(new Payload<Object>() { status = "fail", data = new LoginFailureDTO() });
            }

            string token = CreateToken(user, config);

            var response = new ResponseDTO<LoginSuccessDTO>();
            response.Data.user.Id = user.Id;
            response.Data.user.Credential.Email = user.Credential.Email;
            response.Data.user.Profile.FirstName = user.Profile.FirstName;
            response.Data.user.Profile.LastName = user.Profile.LastName;
            response.Data.user.Profile.Bio = user.Profile.Bio;
            response.Data.user.Profile.Github = user.Profile.Github;


            response.Data.token = token;

            return Results.Ok(response) ;
           
        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        public static async Task<IResult> GetUserById(int id)
        {
            return TypedResults.Ok();
        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        public static async Task<IResult> UpdateUser(int id)
        {
            return TypedResults.Ok();
        }
        private static string CreateToken(User user, IConfigurationSettings config)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Sid, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Credential.Username),
                new Claim(ClaimTypes.Email, user.Credential.Email)
                
                
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

