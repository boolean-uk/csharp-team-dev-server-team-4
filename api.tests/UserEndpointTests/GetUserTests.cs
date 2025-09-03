using exercise.wwwapi.DTOs;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Text.Json;

namespace api.tests.UserEndpointTests
{
    public class GetUserTests
    {
        private WebApplicationFactory<Program> _factory;
        private HttpClient _client;
        
        [SetUp]
        public void Setup()
        {
            _factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
            {
                builder.UseSetting("environment", "staging");
            });
            
            _client = _factory.CreateClient();
        }

        [TearDown]
        public void TearDown()
        {
            _client.Dispose();
            _factory.Dispose();
        }
        
        [Test]
        public async Task GetUserPassesTest()
        {
            const string email = "test1@test1";
            const int userId = 1;
            
            var getUserResponse = await _client.GetAsync($"users/{userId}");
            Assert.That(getUserResponse.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));

            var getUserJson = await getUserResponse.Content.ReadAsStringAsync();
            var responseDto = JsonSerializer.Deserialize<ResponseDTO<UserDTO>>(getUserJson);
            Assert.That(responseDto, Is.Not.Null);
            Assert.That(responseDto.Data.Email, Is.EqualTo(email));
        }

        [Test]
        public async Task GetUserNotFoundTest()
        {
            const int userId = 1353275;
            
            await _client.DeleteAsync($"users/{userId}");
            var getUserResponse = await _client.GetAsync($"users/{userId}");
            Assert.That(getUserResponse.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.NotFound));
        }

    }
}

