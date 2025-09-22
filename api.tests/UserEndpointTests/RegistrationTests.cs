using exercise.wwwapi.DTOs.Register;
using System.Text.Json;
using exercise.wwwapi.Endpoints;

namespace api.tests.UserEndpointTests;

public class CreateUserTests
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
    public async Task RegisterUserExistsTest()
    {
        const string email = "test2@test2";
        const string password = "Test2test2%";
        const string username = "test2@test2";

        var newUser = new RegisterRequestDTO
        {
            Email = email,
            Password = password,
            Username = username
        };

        var content = new StringContent(
            JsonSerializer.Serialize(newUser),
            System.Text.Encoding.UTF8,
            "application/json"
        );
        var response = await _client.PostAsync("/users", content);
        Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.Conflict));
    }

    [Test]
    public async Task RegisterUserEmailValidationFailsTest()
    {
        var newUser = new RegisterRequestDTO
        {
            Email = "test",
            Password = "Mattimatti7&",
            Username = "Matti"
        };

        var content = new StringContent(
            JsonSerializer.Serialize(newUser),
            System.Text.Encoding.UTF8,
            "application/json"
        );
        var response = await _client.PostAsync("/users", content);
        Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.BadRequest));
    }

    [Test]
    public async Task RegisterUserPasswordValidationFailsTest()
    {
        const string email = "testtesttest@test.test";
        const string password = "test";
        const string username = "testtest";
        
        var newUser = new RegisterRequestDTO
        {
            Email = email,
            Password = password,
            Username = username
        };

        var content = new StringContent(
            JsonSerializer.Serialize(newUser),
            System.Text.Encoding.UTF8,
            "application/json"
        );
        var response = await _client.PostAsync("/users", content);
        Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.BadRequest));
    }
}