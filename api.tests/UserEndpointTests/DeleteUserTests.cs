using exercise.wwwapi.DTOs;
using exercise.wwwapi.DTOs.Login;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using exercise.wwwapi.Endpoints;

namespace api.tests.UserEndpointTests;

public class DeleteUserTests
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
    public async Task UPDATEME___DeleteUserPassesTest()
    {
        
        Assert.That(true);
    }

    [Test]
    public async Task DeleteUserUnauthorizedTest()
    {
        const int userId = 1;
        var deleteResponse = await _client.DeleteAsync($"users/{userId}");
        Assert.That(deleteResponse.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.Unauthorized));
    }
}