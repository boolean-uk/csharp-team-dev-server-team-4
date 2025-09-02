using exercise.wwwapi.DTOs.UpdateUser;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Text.Json;

namespace api.tests.UserEndpointTests;

public class UpdateUserTests
{
    // some test commented out until github actions db is setup
    [Test]
    public async Task UpdateUserEmailValidationFailsTest()
    {
        var factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder => { });
        var client = factory.CreateClient();

        var updateUser = new UpdateUserRequestDTO
        {
            Email = "Test"
        };
        var content = new StringContent(JsonSerializer.Serialize(updateUser), System.Text.Encoding.UTF8,
            "application/json");
        var patchResponse = await client.PatchAsync("users/1", content);

        Assert.That(patchResponse.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.BadRequest));
    }

    [Test]
    public async Task UpdateUserPasswordValidationFailsTest()
    {
        var factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder => { });
        var client = factory.CreateClient();

        var updateUser = new UpdateUserRequestDTO
        {
            Password = "Test"
        };
        var content = new StringContent(JsonSerializer.Serialize(updateUser), System.Text.Encoding.UTF8,
            "application/json");
        var patchResponse = await client.PatchAsync("/users/1", content);

        Assert.That(patchResponse.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.BadRequest));
    }

    [Test]
    public async Task UpdateUserMobileNumberValidationFailsTest()
    {
        var factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder => { });
        var client = factory.CreateClient();

        var updateUser = new UpdateUserRequestDTO
        {
            Phone = "Test"
        };
        var content = new StringContent(JsonSerializer.Serialize(updateUser), System.Text.Encoding.UTF8,
            "application/json");
        var patchResponse = await client.PatchAsync("/users/1", content);

        Assert.That(patchResponse.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.BadRequest));
    }

    /*
    [Test]
    public async Task UpdateUserNotFoundTest()
    {
        var factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder => { });
        var client = factory.CreateClient();

        var updateUser = new UpdateUserRequestDTO
        {
            Username = "Test"
        };
        var content = new StringContent(JsonSerializer.Serialize(updateUser), System.Text.Encoding.UTF8, "application/json");
        var patchResponse = await client.PatchAsync("/users/15221582", content);

        Assert.That(patchResponse.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.NotFound));
    }
    */

    /*
    [Test]
    public async Task UpdateUserPassesTest()
    {
        var factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder => { });
        var client = factory.CreateClient();

        var getResponse = await client.GetAsync("users/1");

        if (!getResponse.IsSuccessStatusCode)
        {
            Assert.Fail("No user on id=1");
        }

        var updateUser = new UpdateUserRequestDTO
        {
            Username = "Test"
        };
        var content = new StringContent(JsonSerializer.Serialize(updateUser), System.Text.Encoding.UTF8, "application/json");
        var patchResponse = await client.PatchAsync("/users/1", content);

        Assert.That(patchResponse.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));
    }
    */

    /*
    [Test]
    public async Task UpdateUserEmailValidationPassesTest()
    {
        var factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder => { });
        var client = factory.CreateClient();

        var getResponse = await client.GetAsync("users/1");

        if (!getResponse.IsSuccessStatusCode)
        {
            Assert.Fail("No user on id=1");
        }

        var updateUser = new UpdateUserRequestDTO
        {
            Email = "test@testupdateuser.com"
        };
        var content = new StringContent(JsonSerializer.Serialize(updateUser), System.Text.Encoding.UTF8, "application/json");
        var patchResponse = await client.PatchAsync("/users/1", content);

        Assert.That(patchResponse.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));
    }
    */

    /*
    [Test]
    public async Task UpdateUserPasswordValidationPassesTest()
    {
        var factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder => { });
        var client = factory.CreateClient();

        var getResponse = await client.GetAsync("users/1");

        if (!getResponse.IsSuccessStatusCode)
        {
            Assert.Fail("No user on id=1");
        }

        var updateUser = new UpdateUserRequestDTO
        {
            Password = "passwordD1$"
        };
        var content = new StringContent(JsonSerializer.Serialize(updateUser), System.Text.Encoding.UTF8, "application/json");
        var patchResponse = await client.PatchAsync("/users/1", content);

        Assert.That(patchResponse.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));
    }
    */

    /*
    [Test]
    public async Task UpdateUserMobileValidationPassesTest()
    {
        var factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder => { });
        var client = factory.CreateClient();

        var getResponse = await client.GetAsync("users/1");

        if (!getResponse.IsSuccessStatusCode)
        {
            Assert.Fail("No user on id=1");
        }

        var updateUser = new UpdateUserRequestDTO
        {
            MobileNumber = "99911888"
        };
        var content = new StringContent(JsonSerializer.Serialize(updateUser), System.Text.Encoding.UTF8, "application/json");
        var patchResponse = await client.PatchAsync("/users/1", content);

        Assert.That(patchResponse.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));
    }
    */
}