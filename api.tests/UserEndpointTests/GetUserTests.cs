using exercise.wwwapi.DTOs;
using System.Text.Json;
using exercise.wwwapi.Endpoints;

namespace api.tests.UserEndpointTests;

public class GetUserTests
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
    public async Task GetUserPassesTest()
    {
        const string email = "test1@test1";
        const int userId = 1;
            
        var getUserResponse = await _client.GetAsync($"users/{userId}");
        Assert.That(getUserResponse.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));

        var getUserJson = await getUserResponse.Content.ReadAsStringAsync();
        var responseDto = JsonSerializer.Deserialize<ResponseDTO<UserDTO>>(getUserJson);
        Assert.That(responseDto, Is.Not.Null);
        Assert.That(responseDto.Data.Email, Is.EqualTo(email));
    }

    [Test]
    public async Task GetUserNotFoundTest()
    {
        const int userId = 1353275;
            
        await _client.GetAsync($"users/{userId}");
        var getUserResponse = await _client.GetAsync($"users/{userId}");
        Assert.That(getUserResponse.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.NotFound));
    }

    [Test]
    public async Task GetAllUsersTest()
    {
        var getUsersResponse = await _client.GetAsync($"users");
        Assert.That(getUsersResponse.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));
    }

}