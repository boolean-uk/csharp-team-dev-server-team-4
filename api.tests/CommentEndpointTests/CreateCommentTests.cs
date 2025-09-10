using exercise.wwwapi.DTOs;
using exercise.wwwapi.DTOs.Login;
using exercise.wwwapi.Endpoints;
using System.Net.Http.Headers;
using System.Text.Json;

namespace api.tests.CommentEndpointTests
{
    public class CreateCommentTests
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
        public async Task CreateCommentPostNotFoundTest()
        {
            var loginUser = new LoginRequestDTO
            {
                Email = "test1@test1",
                Password = "Test1test1%"
            };

            var contentLogin = new StringContent(
                JsonSerializer.Serialize(loginUser),
                System.Text.Encoding.UTF8,
                "application/json"
            );

            var loginResponse = await _client.PostAsync("/login", contentLogin);
            Assert.That(loginResponse.IsSuccessStatusCode, Is.True);

            var loginJson = await loginResponse.Content.ReadAsStringAsync();
            var loginResult = JsonSerializer.Deserialize<ResponseDTO<LoginSuccessDTO>>(loginJson);
            Assert.That(loginResult, Is.Not.Null);
            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", loginResult!.Data.Token);

            var payload = new { Body = "Hello there" };
            var content = new StringContent(
                JsonSerializer.Serialize(payload),
                System.Text.Encoding.UTF8,
                "application/json"
            );

            var response = await _client.PostAsync("/posts/999999/comments", content);
            Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.NotFound));
        }

        [Test]
        public async Task CreateCommentEmptyFailsTest()
        {
            var loginUser = new LoginRequestDTO
            {
                Email = "test1@test1",
                Password = "Test1test1%"
            };

            var contentLogin = new StringContent(
            JsonSerializer.Serialize(loginUser),
            System.Text.Encoding.UTF8,
            "application/json"
            );

            var loginResponse = await _client.PostAsync("/login", contentLogin);
            Assert.That(loginResponse.IsSuccessStatusCode, Is.True);

            var loginJson = await loginResponse.Content.ReadAsStringAsync();
            var loginResult = JsonSerializer.Deserialize<ResponseDTO<LoginSuccessDTO>>(loginJson);
            Assert.That(loginResult, Is.Not.Null);
            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", loginResult!.Data.Token);

            var payload = new { Body = "" };
            var content = new StringContent(
                JsonSerializer.Serialize(payload),
                System.Text.Encoding.UTF8,
                "application/json"
            );

            var response = await _client.PostAsync("/posts/1/comments", content);
            Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.BadRequest));

        }

        [Test]
        public async Task CreateCommentUnauthorizedTest()
        {
            var payload = new { Body = "Should fail without auth" };
            var content = new StringContent(
                JsonSerializer.Serialize(payload),
                System.Text.Encoding.UTF8,
                "application/json"
            );

            var response = await _client.PostAsync("/posts/1/comments", content);
            Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.Unauthorized));
        }
    }
}
