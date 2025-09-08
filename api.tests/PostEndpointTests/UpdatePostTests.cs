using exercise.wwwapi.DTOs;
using exercise.wwwapi.DTOs.Login;
using exercise.wwwapi.DTOs.Posts.UpdatePost;
using exercise.wwwapi.Endpoints;
using NUnit.Framework;
using System.Net.Http.Headers;
using System.Text.Json;
// If needed: using exercise.wwwapi.DTOs.Posts;

namespace api.tests.PostEndpointTests
{
    public class UpdatePostTests
    {
        private Random _random;
        private HttpClient _client;

        [SetUp]
        public void Setup()
        {
            _client = TestUtils.CreateClient();
            _random = new Random();
        }

        [TearDown]
        public void TearDown()
        {
            _client.Dispose();
        }

        [Test]
        public async Task UpdatePost401UnauthorizedTest()
        {
            var updatePost = new UpdatePostRequestDTO { Body = "Unauthorized change" };
            var content = new StringContent(JsonSerializer.Serialize(updatePost), System.Text.Encoding.UTF8, "application/json");

            var patchResponse = await _client.PatchAsync("posts/1", content);

            Assert.That(patchResponse.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.Unauthorized));
        }

        [Test]
        public async Task UpdatePostAuthorizationReturns401()
        {
            var loginUser = new LoginRequestDTO { Email = "test2@test2", Password = "Test2test2%" };
            var contentLogin = new StringContent(JsonSerializer.Serialize(loginUser), System.Text.Encoding.UTF8, "application/json");
            var loginResponse = await _client.PostAsync("login", contentLogin);
            Assert.That(loginResponse.IsSuccessStatusCode, Is.True);

            var json = await loginResponse.Content.ReadAsStringAsync();
            var login = JsonSerializer.Deserialize<ResponseDTO<LoginSuccessDTO>>(json);
            Assert.That(login, Is.Not.Null);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", login!.Data.Token);

            var updatePost = new UpdatePostRequestDTO { Body = "Should not be allowed" };
            var content = new StringContent(JsonSerializer.Serialize(updatePost), System.Text.Encoding.UTF8, "application/json");

            var patchResponse = await _client.PatchAsync("posts/1", content);

            Assert.That(patchResponse.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.Unauthorized));
        }

        [Test]
        public async Task UpdatePostPassesTest()
        {
            var loginUser = new LoginRequestDTO { Email = "test1@test1", Password = "Test1test1%" };
            var contentLogin = new StringContent(JsonSerializer.Serialize(loginUser), System.Text.Encoding.UTF8, "application/json");
            var loginResponse = await _client.PostAsync("login", contentLogin);
            Assert.That(loginResponse.IsSuccessStatusCode, Is.True);

            var json = await loginResponse.Content.ReadAsStringAsync();
            var login = JsonSerializer.Deserialize<ResponseDTO<LoginSuccessDTO>>(json);
            Assert.That(login, Is.Not.Null);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", login!.Data.Token);

            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789 ";
            var newBody = new string(Enumerable.Repeat(chars, 60).Select(s => s[_random.Next(s.Length)]).ToArray());

            var updatePost = new UpdatePostRequestDTO { Body = newBody };
            var content = new StringContent(JsonSerializer.Serialize(updatePost), System.Text.Encoding.UTF8, "application/json");

            var patchResponse = await _client.PatchAsync("posts/1", content);
            var patchContent = await patchResponse.Content.ReadAsStringAsync();
            var updatedResult = JsonSerializer.Deserialize<ResponseDTO<UpdatePostSuccessDTO>>(patchContent);

            Assert.That(updatedResult, Is.Not.Null, "Update Failed");
            Assert.That(patchResponse.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));
            Assert.That(updatedResult!.Data.Body, Is.EqualTo(newBody));
            Assert.That(updatedResult!.Data.Id, Is.EqualTo(1));
            Assert.That(updatedResult!.Data.AuthorId, Is.EqualTo(1));
        }
    }
}
