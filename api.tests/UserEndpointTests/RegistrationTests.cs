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
        // Arrange: prepare request data
        var factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder => { });
        var client = factory.CreateClient();

        var response = await client.GetAsync("users/10050");

        Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.NotFound));

    }

    [Test]
    public async Task RegisterUserExistsTest()
    {
        // Arrange: prepare request data
        var factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder => { });
        var client = factory.CreateClient();

        var newUser = new RegisterRequestDTO
        {
            email = "test@test.test",
            password = "Mattimatti7&"
        };

        var content = new StringContent(JsonSerializer.Serialize(newUser), System.Text.Encoding.UTF8, "application/json");
        var response1 = await client.PostAsync("/users", content);
        var response2 = await client.PostAsync("/users", content);

        Assert.That(response2.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.Conflict));

    }

    [Test]
    public async Task RegisterUserEmailValidationFailsTest()
    {
        // Arrange: prepare request data
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
