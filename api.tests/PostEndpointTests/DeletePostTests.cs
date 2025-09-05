using exercise.wwwapi.DTOs;
using exercise.wwwapi.DTOs.Login;
using exercise.wwwapi.Endpoints;
using NUnit.Framework;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace api.tests.PostEndpointTests
{
    public class DeletePostTests
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
        public async Task DeletePostPassesTest()
        {
            const string email = "test1@test1";
            const string password = "Test1test1%";

            var loginUser = new LoginRequestDTO
            {
                Email = email,
                Password = password
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

            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", result!.Data.Token);

            var deleteResponse = await _client.DeleteAsync("posts/1");
            Assert.That(deleteResponse.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));
        }

        [Test]
        public async Task DeletePostUnauthorizedTest()
        {
            var deleteResponse = await _client.DeleteAsync("posts/1");
            Assert.That(deleteResponse.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.Unauthorized));
        }

        [Test]
        public async Task DeletePostForbiddenWhenNotOwnerTest()
        {
            var loginUser = new LoginRequestDTO
            {
                Email = "test2@test2",
                Password = "Test2test2%"
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

            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", result!.Data.Token);

            var deleteResponse = await _client.DeleteAsync("posts/1");
            Assert.That(deleteResponse.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.Unauthorized));
        }
    }
}
