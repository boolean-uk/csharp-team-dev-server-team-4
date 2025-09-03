using exercise.wwwapi.DTOs;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Text.Json;

namespace api.tests.UserEndpointTests
{
    public class GetUserTests
    {
        [Test]
        public async Task GetUserPassesTest()
        {
            const string email = "test1@test1";
            const int userId = 1;
            
            var factory = new WebApplicationFactory<Program>().WithWebHostBuilder(_ => { });
            var client = factory.CreateClient();

            var getUserResponse = await client.GetAsync($"users/{userId}");
            Assert.That(getUserResponse.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));

            var getUserJson = await getUserResponse.Content.ReadAsStringAsync();
            var responseDto = JsonSerializer.Deserialize<ResponseDTO<UserDTO>>(getUserJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            Assert.That(responseDto, Is.Not.Null);
            Assert.That(responseDto.Data.Email, Is.EqualTo(email));
        }

        [Test]
        public async Task GetUserNotFoundTest()
        {
            const int userId = 1353275;
            
            var factory = new WebApplicationFactory<Program>().WithWebHostBuilder(_ => { });
            var client = factory.CreateClient();

            await client.DeleteAsync($"users/{userId}");
            var getUserResponse = await client.GetAsync($"users/{userId}");
            Assert.That(getUserResponse.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.NotFound));
        }

    }
}

