using exercise.wwwapi.DTOs;
using exercise.wwwapi.DTOs.Login;
using exercise.wwwapi.DTOs.Register;
using exercise.wwwapi.DTOs.UpdateUser;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace api.tests.UserEndpointTests
{
    public class DeleteUserTests
    {

        [Test]
        public async Task DeleteUserPassesTest()
        {
            var factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder => { });
            var client = factory.CreateClient();

            var email = "TemporaryTestuser@test";
            var password = "TemporaryTestuser1test%";

            var newUser = new RegisterRequestDTO
            {
                email = email,
                password = password
            };

            var contentRegister = new StringContent(JsonSerializer.Serialize(newUser), Encoding.UTF8, "application/json");
            var registerResponse = await client.PostAsync("/users", contentRegister);
            string registerJsonResponse = await registerResponse.Content.ReadAsStringAsync();
            var registerResult = JsonSerializer.Deserialize<ResponseDTO<RegisterSuccessDTO>>(
                registerJsonResponse, 
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                );

            Assert.That(registerResponse.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));
            if (!registerResponse.IsSuccessStatusCode)
            {
                Assert.Fail();
            }

            var loginUser = new LoginRequestDTO()
            {
                email = email,
                password = password
            };
            var contentLogin = new StringContent(JsonSerializer.Serialize(loginUser), Encoding.UTF8, "application/json");
            var loginResponse = await client.PostAsync("login", contentLogin);

            if (!loginResponse.IsSuccessStatusCode)
            {
                Assert.Fail();
            }

            string jsonResponse = await loginResponse.Content.ReadAsStringAsync();
            ResponseDTO<LoginSuccessDTO>? result = JsonSerializer.Deserialize<ResponseDTO<LoginSuccessDTO>>(jsonResponse);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.Data.token);
            
            Assert.That(loginResponse.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));

            // Now delete the user we created
            var userId = registerResult.Data.user.Id;
            await client.DeleteAsync($"users/{userId}");
            var deletedUser = await client.GetAsync($"users/{userId}");
            Assert.That(deletedUser.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.NotFound));

        }

        [Test]
        public async Task DeleteUserUnauthorizedTest()
        {
            var factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder => { });
            var client = factory.CreateClient();

            var userId = 1;
            var deleteResponse = await client.DeleteAsync($"users/{userId}");
            Assert.That(deleteResponse.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.Unauthorized));
        }

    }
}
