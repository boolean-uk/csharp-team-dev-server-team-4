using exercise.wwwapi.DTOs;
using exercise.wwwapi.DTOs.Login;
using exercise.wwwapi.Endpoints;
using System.Net.Http.Headers;
using System.Text.Json;

namespace api.tests.CommentEndpointTests
{
    public class UpdateCommentTests
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
        public async Task UpdateCommentUnauthorizedTest()
        {
            var payload = new { Body = "No auth should fail" };
            var content = new StringContent(
                JsonSerializer.Serialize(payload),
                System.Text.Encoding.UTF8,
                "application/json"
            );

            var response = await _client.PatchAsync("/comments/1", content);
            Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.Unauthorized));
        }

        [Test]
        public async Task UpdateCommentForbiddenWhenNotOwnerTest()
        {
            var loginUser = new LoginRequestDTO
            {
                Email = "test2@test2",
                Password = "Test2test2%"
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

            var payload = new { Body = "Trying to update someone else's comment" };
            var content = new StringContent(
                JsonSerializer.Serialize(payload),
                System.Text.Encoding.UTF8,
                "application/json"
            );

            var response = await _client.PatchAsync("/comments/1", content);
            Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.Unauthorized));
        }

        [Test]
        public async Task UpdateCommentNotFoundTest()
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

            var payload = new { Body = "Won't find this id" };
            var content = new StringContent(
                JsonSerializer.Serialize(payload),
                System.Text.Encoding.UTF8,
                "application/json"
            );
            var response = await _client.PatchAsync("/comments/99999999", content);

            Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.NotFound)
                .Or.EqualTo(System.Net.HttpStatusCode.Unauthorized));
        }
    }
}
