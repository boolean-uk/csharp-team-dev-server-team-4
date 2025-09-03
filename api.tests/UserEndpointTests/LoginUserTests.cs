using exercise.wwwapi.DTOs.Login;
using exercise.wwwapi.DTOs.Register;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Text.Json;

namespace api.tests.UserEndpointTests
{
    public class LoginUserTests
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
        public async Task UserLoginSucceeds()
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
                System.Text.Encoding.UTF8,
                "application/json"
            );

            var loginResponse = await _client.PostAsync("login", contentLogin);

            Assert.That(loginResponse.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));
        }

        [Test]
        public async Task UserLoginFails()
        {
            const string email = "test1@test1";
            const string password = "TESTtest7&aaaaaaaaaaaa";
            const string username = "aTesTaa";

            var newUser = new RegisterRequestDTO
            {
                Email = email,
                Password = password,
                Username = username,
            };

            var contentRegister = new StringContent(
                JsonSerializer.Serialize(newUser),
                System.Text.Encoding.UTF8,
                "application/json"
            );

            await _client.PostAsync("/users", contentRegister);

            var loginUser = new LoginRequestDTO()
            {
                Email = email,
                Password = "dwawdfwawfawfw"
            };
            var contentLogin = new StringContent(
                JsonSerializer.Serialize(loginUser),
                System.Text.Encoding.UTF8,
                "application/json"
            );
            var loginResponse = await _client.PostAsync("login", contentLogin);

            Assert.That(loginResponse.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.BadRequest));
        }
    }
}