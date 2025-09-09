using exercise.wwwapi.DTOs;
using exercise.wwwapi.DTOs.GetUsers;
using exercise.wwwapi.DTOs.Login;
using exercise.wwwapi.Endpoints;
using System.Text.Json;

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
            
        var getUserResponse = await _client.GetAsync($"users/{userId}");
        Assert.That(getUserResponse.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.NotFound));
    }

    [Test]
    public async Task GetAllUsersTest()
    {
        var getUsersResponse = await _client.GetAsync($"users");
        Assert.That(getUsersResponse.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));
    }

    [Test]
    public async Task GetFilteredUsersByFirstNameTest()
    {
        var getUsersResponse = await _client.GetAsync($"users?searchTerm=Michael");
        var jsonResponse = await getUsersResponse.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<ResponseDTO<UsersSuccessDTO>>(jsonResponse);

        Assert.That(result.Data.Users.Count, Is.EqualTo(2));
    }

    [Test]
    public async Task GetFilteredUsersByLastNameTest()
    {
        var getUsersResponse = await _client.GetAsync($"users?searchTerm=Jackson");
        var jsonResponse = await getUsersResponse.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<ResponseDTO<UsersSuccessDTO>>(jsonResponse);

        Assert.That(result.Data.Users.Count, Is.EqualTo(1));
    }

    [Test]
    public async Task GetFilteredUsersTestFails()
    {
        var getUsersResponse = await _client.GetAsync($"users?searchTerm=test%20test");
        var jsonResponse = await getUsersResponse.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<ResponseDTO<UsersSuccessDTO>>(jsonResponse);

        Assert.That(result.Data.Users.Count, Is.EqualTo(0));
    }

    [Test]
    public async Task GetFilteredUsersByFullNameTest()
    {
        var getUsersResponse = await _client.GetAsync($"users?searchTerm=Michael%20Jackson");
        var jsonResponse = await getUsersResponse.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<ResponseDTO<UsersSuccessDTO>>(jsonResponse);

        Assert.That(result.Data.Users.Count, Is.EqualTo(1));
    }

    [Test]
    public async Task GetFilteredUsersByLetterTest()
    {
        var getUsersResponse = await _client.GetAsync($"users?searchTerm=m");
        var jsonResponse = await getUsersResponse.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<ResponseDTO<UsersSuccessDTO>>(jsonResponse);

        Assert.That(result.Data.Users.Count, Is.EqualTo(3));
    }

}