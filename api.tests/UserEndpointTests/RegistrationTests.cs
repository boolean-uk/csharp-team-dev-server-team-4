using exercise.wwwapi.DTOs.Register;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Text.Json;

namespace api.tests.UserEndpointTests;

public class CreateUserTests
{
    [Test]
    public async Task GetUserByIdTestFails()
    {
        var factory = new WebApplicationFactory<Program>().WithWebHostBuilder(_ => { });
        var client = factory.CreateClient();

        var response = await client.GetAsync("users/10050");

        Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.NotFound));
    }

    // TODO: add test "RegisterUser" that adds a user, check if the response is 201 Created and then delete the user again so we don't fill up the database

    [Test]
    public async Task RegisterUserExistsTest()
    {
        const string email = "test1@test1";
        const string password = "Test1test1%";
        const string username = "TestTestTest";

        var factory = new WebApplicationFactory<Program>().WithWebHostBuilder(_ => { });
        var client = factory.CreateClient();

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
        var response = await client.PostAsync("/users", content);

        Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.Conflict));
    }

    [Test]
    public async Task RegisterUserEmailValidationFailsTest()
    {
        var factory = new WebApplicationFactory<Program>().WithWebHostBuilder(_ => { });
        var client = factory.CreateClient();

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
        var response = await client.PostAsync("/users", content);

        Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.BadRequest));
    }

    [Test]
    public async Task RegisterUserPasswordValidationFailsTest()
    {
        const string email = "testtesttest@test.test";
        const string password = "test";
        const string username = "testtest";
        
        // Arrange: prepare request data
        var factory = new WebApplicationFactory<Program>().WithWebHostBuilder(_ => { });
        var client = factory.CreateClient();

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
        var response = await client.PostAsync("/users", content);

        Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.BadRequest));
    }
}