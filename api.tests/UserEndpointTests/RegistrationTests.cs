using exercise.wwwapi.DTOs;
using exercise.wwwapi.DTOs.Register;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Json;
using System.Text.Json;

namespace api.tests.UserEndpointTests;

public class CreateUserTests
{

    [Test]
    public async Task GetUserByIdTestFails()
    {
        var factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder => { });
        var client = factory.CreateClient();

        var response = await client.GetAsync("users/10050");

        Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.NotFound));
    }

    // TODO: add test "RegisterUser" that adds a user, check if the response is 201 Created and then delete the user again so we don't fill up the database

    [Test]
    public async Task RegisterUserExistsTest()
    {
        var factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder => { });
        var client = factory.CreateClient();

        var newUser = new RegisterRequestDTO
        {
            email = "test1@test1",
            password = "Test1test1%"
        };

        var content = new StringContent(JsonSerializer.Serialize(newUser), System.Text.Encoding.UTF8, "application/json");
        var response = await client.PostAsync("/users", content);

        Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.Conflict));
    }


    [Test]
    public async Task RegisterUserEmailValidationFailsTest()
    {
        var factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder => { });
        var client = factory.CreateClient();

        var newUser = new RegisterRequestDTO
        {
            email = "test",
            password = "Mattimatti7&"
        };

        var content = new StringContent(JsonSerializer.Serialize(newUser), System.Text.Encoding.UTF8, "application/json");
        var response = await client.PostAsync("/users", content);

        Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.BadRequest));

    }

    [Test]
    public async Task RegisterUserPasswordValidationFailsTest()
    {
        // Arrange: prepare request data
        var factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder => { });
        var client = factory.CreateClient();

        var newUser = new RegisterRequestDTO
        {
            email = "testtesttest@test.test",
            password = "test"
        };

        var content = new StringContent(JsonSerializer.Serialize(newUser), System.Text.Encoding.UTF8, "application/json");
        var response = await client.PostAsync("/users", content);

        Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.BadRequest));

    }
}
