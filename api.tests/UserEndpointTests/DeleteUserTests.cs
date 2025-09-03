using exercise.wwwapi.DTOs;
using exercise.wwwapi.DTOs.Login;
using exercise.wwwapi.DTOs.Register;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace api.tests.UserEndpointTests
{
    public class DeleteUserTests
    {
        [Test]
        public async Task DeleteUserPassesTest()
        {
            const string email = "TemporaryTestuser@test";
            const string password = "TemporaryTestuser1test%";
            const string username = "TemporaryTestUser";
            
            var factory = new WebApplicationFactory<Program>().WithWebHostBuilder(_ => { });
            var client = factory.CreateClient();

            var newUser = new RegisterRequestDTO
            {
                Email = email,
                Password = password,
                Username = username,
            };

            var contentRegister = new StringContent(
                JsonSerializer.Serialize(newUser),
                Encoding.UTF8,
                "application/json"
            );
            
            var registerResponse = await client.PostAsync("/users", contentRegister);
            Assert.That(registerResponse.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));
            
            var registerJsonResponse = await registerResponse.Content.ReadAsStringAsync();

            var registerResult = JsonSerializer.Deserialize<ResponseDTO<RegisterSuccessDTO>>(
                registerJsonResponse,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
            
            Assert.That(registerResult, Is.Not.Null);

            var loginUser = new LoginRequestDTO
            {
                Email = email,
                Password = password,
            };
            
            var contentLogin = new StringContent(
                JsonSerializer.Serialize(loginUser),
                Encoding.UTF8,
                "application/json"
            );
            
            var loginResponse = await client.PostAsync("login", contentLogin);
            Assert.That(loginResponse.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));

            var jsonResponse = await loginResponse.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ResponseDTO<LoginSuccessDTO>>(jsonResponse);
            Assert.That(result, Is.Not.Null);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.Data.Token);
            
            // Now delete the user we created
            var userId = registerResult.Data.User.Id;
            await client.DeleteAsync($"users/{userId}");
            var deletedUser = await client.GetAsync($"users/{userId}");
            Assert.That(deletedUser.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.NotFound));
        }

        [Test]
        public async Task DeleteUserUnauthorizedTest()
        {
            var factory = new WebApplicationFactory<Program>().WithWebHostBuilder(_ => { });
            var client = factory.CreateClient();

            const int userId = 1;
            var deleteResponse = await client.DeleteAsync($"users/{userId}");
            Assert.That(deleteResponse.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.Unauthorized));
        }
    }
}