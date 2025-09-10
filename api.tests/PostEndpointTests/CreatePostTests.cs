using exercise.wwwapi.DTOs;
using exercise.wwwapi.DTOs.Login;
using exercise.wwwapi.DTOs.Posts;
using exercise.wwwapi.Endpoints;
using System.Net.Http.Headers;
using System.Text.Json;

namespace api.tests.PostEndpointTests
{
    public class CreatePostTests
    {
        private HttpClient _client;

        [SetUp]
        public void Setup()
        {
            _client = TestUtils.CreateClient();
        }

        [TearDown]
        public void TearDown()
        {
            _client.Dispose();
        }

        [Test]
        public async Task CreatePostValidationFailsTest()
        {
            var loginUser = new LoginRequestDTO
            {
                Email = "test1@test1",
                Password = "Test1test1%"
            };

            var loginContent = new StringContent(
                JsonSerializer.Serialize(loginUser),
                System.Text.Encoding.UTF8,
                "application/json"
            );

            var loginResponse = await _client.PostAsync("/login", loginContent);
            Assert.That(loginResponse.IsSuccessStatusCode, Is.True, "Login failed");

            var loginJson = await loginResponse.Content.ReadAsStringAsync();
            var loginResult = JsonSerializer.Deserialize<ResponseDTO<LoginSuccessDTO>>(loginJson);
            Assert.That(loginResult, Is.Not.Null, "Login parse failed");

            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", loginResult!.Data.Token);

            var newPost = new CreatePostRequestDTO
            {
                Body = ""
            };

            var content = new StringContent(
                JsonSerializer.Serialize(newPost),
                System.Text.Encoding.UTF8,
                "application/json"
            );

            var response = await _client.PostAsync("/posts", content);

            Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task CreatePostTooLongValidationFailsTest()
        {
            var loginUser = new LoginRequestDTO
            {
                Email = "test1@test1",
                Password = "Test1test1%"
            };

            var loginContent = new StringContent(
                JsonSerializer.Serialize(loginUser),
                System.Text.Encoding.UTF8,
                "application/json"
            );

            var loginResponse = await _client.PostAsync("/login", loginContent);
            Assert.That(loginResponse.IsSuccessStatusCode, Is.True);

            var loginJson = await loginResponse.Content.ReadAsStringAsync();
            var loginResult = JsonSerializer.Deserialize<ResponseDTO<LoginSuccessDTO>>(loginJson);
            Assert.That(loginResult, Is.Not.Null);

            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", loginResult!.Data.Token);

            var tooLong = new string('x', 1001);

            var newPost = new CreatePostRequestDTO
            {
                Body = tooLong
            };

            var content = new StringContent(
                JsonSerializer.Serialize(newPost),
                System.Text.Encoding.UTF8,
                "application/json"
            );

            var response = await _client.PostAsync("/posts", content);

            Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.BadRequest));
        }
    }
}
