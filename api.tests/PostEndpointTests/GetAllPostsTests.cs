using exercise.wwwapi.DTOs;
using exercise.wwwapi.DTOs.Login;
using exercise.wwwapi.Endpoints;
using NUnit.Framework;
using System.Net.Http.Headers;
using System.Text.Json;

namespace api.tests.PostEndpointTests
{
    public class GetAllPostsTests
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
        public async Task GetAllPostsPassesTest()
        {
            var loginUser = new LoginRequestDTO { Email = "test1@test1", Password = "Test1test1%" };
            var loginContent = new StringContent(JsonSerializer.Serialize(loginUser), System.Text.Encoding.UTF8, "application/json");
            var loginResponse = await _client.PostAsync("login", loginContent);
            Assert.That(loginResponse.IsSuccessStatusCode, Is.True);

            var loginJson = await loginResponse.Content.ReadAsStringAsync();
            var login = JsonSerializer.Deserialize<ResponseDTO<LoginSuccessDTO>>(loginJson);
            Assert.That(login, Is.Not.Null);

            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", login!.Data.Token);

            var response = await _client.GetAsync("posts");
            Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));

            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);

            var posts = doc.RootElement
                .GetProperty("data")
                .GetProperty("posts");

            Assert.That(posts.ValueKind, Is.EqualTo(JsonValueKind.Array));
            Assert.That(posts.GetArrayLength(), Is.GreaterThanOrEqualTo(5));
        }

        [Test]
        public async Task GetAllPostsContainsExpectedPostTest()
        {
            var loginUser = new LoginRequestDTO { Email = "test1@test1", Password = "Test1test1%" };
            var loginContent = new StringContent(JsonSerializer.Serialize(loginUser), System.Text.Encoding.UTF8, "application/json");
            var loginResponse = await _client.PostAsync("login", loginContent);
            Assert.That(loginResponse.IsSuccessStatusCode, Is.True);

            var loginJson = await loginResponse.Content.ReadAsStringAsync();
            var login = JsonSerializer.Deserialize<ResponseDTO<LoginSuccessDTO>>(loginJson);
            Assert.That(login, Is.Not.Null);

            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", login!.Data.Token);

            var response = await _client.GetAsync("posts");
            Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));

            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);

            var posts = doc.RootElement
                .GetProperty("data")
                .GetProperty("posts");

            var found = posts.EnumerateArray().Any(p =>
                p.GetProperty("id").GetInt32() == 1 &&
                p.GetProperty("authorId").GetInt32() == 1
            );

            Assert.That(found, Is.True, "Expected to find seeded post with id=1 and authorId=1.");
        }
    }
}