﻿using exercise.wwwapi.Configuration;
using exercise.wwwapi.DTOs;
using exercise.wwwapi.DTOs.GetUsers;
using exercise.wwwapi.DTOs.Login;
using exercise.wwwapi.DTOs.Register;
using exercise.wwwapi.Helpers;
using exercise.wwwapi.Models;
using exercise.wwwapi.Repository;
using FluentValidation;
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
            UsersSuccessDTO userData = new UsersSuccessDTO() { Users = !string.IsNullOrEmpty(firstName) ? results.Where(i => i.Email.Contains(firstName)).ToList() : results.ToList() };
            ResponseDTO<UsersSuccessDTO> response = new ResponseDTO<UsersSuccessDTO>() { Status = "success", Data = userData };
            return TypedResults.Ok(response);
        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        private static async Task<IResult> Register(RegisterRequestDTO request, IRepository<User> service, IValidator<RegisterRequestDTO> validator)
        {
            //user exists
            if (service.GetAll().Where(u => u.Email == request.email)
                .Any()) return Results.Conflict(new ResponseDTO<RegisterFailureDTO>() { Status = "Fail" });
            
            // validate email
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

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.password);

            var user = new User();

            user.Username = !string.IsNullOrEmpty(request.username) ? request.username : request.email;
            user.PasswordHash = passwordHash;
            user.Email = request.email;
            user.FirstName = !string.IsNullOrEmpty(request.firstName) ? request.firstName : string.Empty;
            user.LastName = !string.IsNullOrEmpty(request.lastName) ? request.lastName : string.Empty;
            user.Bio = !string.IsNullOrEmpty(request.bio) ? request.bio : string.Empty;
            user.GithubUrl = !string.IsNullOrEmpty(request.githubUrl) ? request.githubUrl : string.Empty;
            user.Role = Role.Student;

            service.Insert(user);
            service.Save();

            ResponseDTO<RegisterSuccessDTO> response = new ResponseDTO<RegisterSuccessDTO>();
            response.Status = "success";
            response.Data.user.firstName = user.FirstName;
            response.Data.user.lastName = user.LastName;
            response.Data.user.bio = user.Bio;
            response.Data.user.githubUrl = user.GithubUrl;
            response.Data.user.username = user.Username;
            response.Data.user.email = user.Email;


            return Results.Ok(response);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        private static async Task<IResult> Login(LoginRequestDTO request, IRepository<User> service, IConfigurationSettings config)
        {
            //if (string.IsNullOrEmpty(request.username)) request.username = request.email;

            //user doesn't exist
            if (!service.GetAll().Where(u => u.Email == request.email).Any()) return Results.BadRequest(new Payload<Object>() { status = "User does not exist", data = new { email="Invalid email and/or password provided"} });

            User user = service.GetAll().FirstOrDefault(u => u.Email == request.email)!;
           

            if (!BCrypt.Net.BCrypt.Verify(request.password, user.PasswordHash))
            {
                return Results.BadRequest(new Payload<Object>() { status = "fail", data = new LoginFailureDTO() });
            }

            string token = CreateToken(user, config);

            ResponseDTO<LoginSuccessDTO> response = new ResponseDTO<LoginSuccessDTO>();
            response.Data.user.Id = user.Id;
            response.Data.user.Email = user.Email;
            response.Data.user.FirstName = user.FirstName;
            response.Data.user.LastName = user.LastName;
            response.Data.user.Bio = user.Bio;
            response.Data.user.GithubUrl = user.GithubUrl;


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
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email)
                
                
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

