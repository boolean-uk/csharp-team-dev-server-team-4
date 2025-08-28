using exercise.wwwapi.DTOs;
using exercise.wwwapi.DTOs.Register;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Json;
using System.Text.Json;

namespace api.tests.UserEndpointTests;

public class Tests
{
    [Test]
    public async Task GetUserByIdTest()
    {
        // Arrange: prepare request data
        var factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder => { });
        var client = factory.CreateClient();

        var response = await client.GetAsync("users/1");

        Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));

    }

    [Test]
    public async Task GetUserByIdTestFails()
    {
        // Arrange: prepare request data
        var factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder => { });
        var client = factory.CreateClient();

        var response = await client.GetAsync("users/1000");

        Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.NotFound));

    }

    [Test]
    public async Task RegisterUserTestFails()
    {
        // Arrange: prepare request data
        var factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder => { });
        var client = factory.CreateClient();

        var newUser = new RegisterRequestDTO
        {
            email = "matti@matti.matti",
            password = "Mattimatti7&"
        };

        var content = new StringContent(JsonSerializer.Serialize(newUser), System.Text.Encoding.UTF8, "application/json");
        var response1 = await client.PostAsync("/users", content);
        var response2 = await client.PostAsync("/users", content);

        Assert.That(response2.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.Conflict));

    }

    [Test]
    public async Task RegisterUserTestEmailValidation()
    {
        // Arrange: prepare request data
        var factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder => { });
        var client = factory.CreateClient();

        var newUser = new RegisterRequestDTO
        {
            email = "matti",
            password = "Mattimatti7&"
        };

        var content = new StringContent(JsonSerializer.Serialize(newUser), System.Text.Encoding.UTF8, "application/json");
        var response = await client.PostAsync("/users", content);

        Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.BadRequest));

    }

    [Test]
    public async Task RegisterUserTestPasswordValidation()
    {
        // Arrange: prepare request data
        var factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder => { });
        var client = factory.CreateClient();

        var newUser = new RegisterRequestDTO
        {
            email = "jakub@test.test",
            password = "test"
        };

        var content = new StringContent(JsonSerializer.Serialize(newUser), System.Text.Encoding.UTF8, "application/json");
        var response = await client.PostAsync("/users", content);

        Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.BadRequest));

    }
}
