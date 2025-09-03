using exercise.wwwapi.DTOs;
using exercise.wwwapi.DTOs.Login;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace api.tests.UserEndpointTests
{
    public class DeleteUserTests
    {
        private WebApplicationFactory<Program> _factory;
        private HttpClient _client;
        
        [SetUp]
        public void Setup()
        {
            _factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
            {
                builder.UseSetting("testing", "true");
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
        public async Task DeleteUserPassesTest()
        {
            const string email = "test1@test1";
            const string password = "Test1test1%";
            
            var loginUser = new LoginRequestDTO()
            {
                Email = email,
                Password = password,
            };

            var contentLogin = new StringContent(
                JsonSerializer.Serialize(loginUser),
                Encoding.UTF8,
                "application/json"
            );
            
            var loginResponse = await _client.PostAsync("login", contentLogin);
            Assert.That(loginResponse.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));

            var jsonResponse = await loginResponse.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ResponseDTO<LoginSuccessDTO>>(jsonResponse);
            Assert.That(result, Is.Not.Null);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.Data.Token);
            
            var userId = result.Data.User.Id;
            var deleteResponse = await _client.DeleteAsync($"users/{userId}");
            Assert.That(deleteResponse.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));
        }

        [Test]
        public async Task DeleteUserUnauthorizedTest()
        {
            const int userId = 1;
            var deleteResponse = await _client.DeleteAsync($"users/{userId}");
            Assert.That(deleteResponse.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.Unauthorized));
        }
    }
}