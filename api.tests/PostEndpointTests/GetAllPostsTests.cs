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
        public async Task GetAllPostsTest()
        {
            var loginUser = new LoginRequestDTO { Email = "test1@test1", Password = "Test1test1%" };
            var loginContent = new StringContent(JsonSerializer.Serialize(loginUser), System.Text.Encoding.UTF8, "application/json");
            var loginResponse = await _client.PostAsync("login", loginContent);
            Assert.That(loginResponse.IsSuccessStatusCode, Is.True);

            var loginJson = await loginResponse.Content.ReadAsStringAsync();
            var login = JsonSerializer.Deserialize<ResponseDTO<LoginSuccessDTO>>(loginJson);
            Assert.That(login, Is.Not.Null);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", login!.Data.Token);

            var resp = await _client.GetAsync("posts");
            Assert.That(resp.IsSuccessStatusCode, Is.True);

            var postsJson = await resp.Content.ReadAsStringAsync();
            var responseDto = JsonSerializer.Deserialize<ResponseDTO<PostsSuccessDtoForTest>>(postsJson);

            Assert.That(responseDto, Is.Not.Null, "Deserialization failed");
            Assert.That(responseDto!.Data.Posts.Count, Is.GreaterThanOrEqualTo(5));
            Assert.That(responseDto.Data.Posts.Any(p => p.Id == 1 && p.AuthorId == 1), Is.True);
        }
    }
    internal class PostsSuccessDtoForTest
    {
        public List<PostDtoForTest> Posts { get; set; } = new();
    }

    internal class PostDtoForTest
    {
        public int Id { get; set; }
        public int AuthorId { get; set; }
        public string Body { get; set; } = "";
        public int Likes { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
