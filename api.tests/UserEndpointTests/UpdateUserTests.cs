using exercise.wwwapi.DTOs;
using exercise.wwwapi.DTOs.Login;
using exercise.wwwapi.DTOs.Register;
using exercise.wwwapi.DTOs.UpdateUser;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace api.tests.UserEndpointTests;

public class UpdateUserTests
{
    Random random = new Random();

    [Test]
    public async Task UpdateUser401UnauthorizedTest()
    {
        var factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder => { });
        var client = factory.CreateClient();

        var updateUser = new UpdateUserRequestDTO
        {
            Email = "Test"
        };
        var content = new StringContent(JsonSerializer.Serialize(updateUser), System.Text.Encoding.UTF8, "application/json");
        var patchResponse = await client.PatchAsync("users/1", content);

        Assert.That(patchResponse.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.Unauthorized));

    }

    [Test]
    public async Task UpdateUserEmailValidationFailsTest()
    {
        var factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder => { });
        var client = factory.CreateClient();

        var email = "test1@test1";
        var password = "Test1test1%";

        var loginUser = new LoginRequestDTO()
        {
            email = email,
            password = password
        };
        var contentLogin = new StringContent(JsonSerializer.Serialize(loginUser), System.Text.Encoding.UTF8, "application/json");
        var loginResponse = await client.PostAsync("login", contentLogin);

        if (!loginResponse.IsSuccessStatusCode)
        {
            Assert.Fail();
        }

        string jsonResponse = await loginResponse.Content.ReadAsStringAsync();
        ResponseDTO<LoginSuccessDTO>? result = JsonSerializer.Deserialize<ResponseDTO<LoginSuccessDTO>>(jsonResponse);

        if (result is null)
        {
            Assert.Fail("Login failed");
        }

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.Data.token);
        var updateUser = new UpdateUserRequestDTO
        {
            Email = "Test"
        };
        var content = new StringContent(JsonSerializer.Serialize(updateUser), System.Text.Encoding.UTF8, "application/json");
        var patchResponse = await client.PatchAsync("users/1", content);

        Assert.That(patchResponse.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.BadRequest));

    }

    [Test]
    public async Task UpdateUserPasswordValidationFailsTest()
    {
        var factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder => { });
        var client = factory.CreateClient();

        var email = "test1@test1";
        var password = "Test1test1%";

        var loginUser = new LoginRequestDTO()
        {
            email = email,
            password = password
        };
        var contentLogin = new StringContent(JsonSerializer.Serialize(loginUser), System.Text.Encoding.UTF8, "application/json");
        var loginResponse = await client.PostAsync("login", contentLogin);

        if (!loginResponse.IsSuccessStatusCode)
        {
            Assert.Fail();
        }

        string jsonResponse = await loginResponse.Content.ReadAsStringAsync();
        ResponseDTO<LoginSuccessDTO>? result = JsonSerializer.Deserialize<ResponseDTO<LoginSuccessDTO>>(jsonResponse);

        if (result is null)
        {
            Assert.Fail("Login failed");
        }

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.Data.token);
        var updateUser = new UpdateUserRequestDTO
        {
            Password = "pass",
        };
        var content = new StringContent(JsonSerializer.Serialize(updateUser), System.Text.Encoding.UTF8, "application/json");
        var patchResponse = await client.PatchAsync("users/1", content);

        Assert.That(patchResponse.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.BadRequest));

    }

    [Test]
    public async Task UpdateUserMobileNumberValidationFailsTest()
    {
        var factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder => { });
        var client = factory.CreateClient();

        var email = "test1@test1";
        var password = "Test1test1%";

        var loginUser = new LoginRequestDTO()
        {
            email = email,
            password = password
        };
        var contentLogin = new StringContent(JsonSerializer.Serialize(loginUser), System.Text.Encoding.UTF8, "application/json");
        var loginResponse = await client.PostAsync("login", contentLogin);

        if (!loginResponse.IsSuccessStatusCode)
        {
            Assert.Fail();
        }

        string jsonResponse = await loginResponse.Content.ReadAsStringAsync();
        ResponseDTO<LoginSuccessDTO>? result = JsonSerializer.Deserialize<ResponseDTO<LoginSuccessDTO>>(jsonResponse);

        if (result is null)
        {
            Assert.Fail("Login failed");
        }

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.Data.token);
        var updateUser = new UpdateUserRequestDTO
        {
            MobileNumber = "test"
        };
        var content = new StringContent(JsonSerializer.Serialize(updateUser), System.Text.Encoding.UTF8, "application/json");
        var patchResponse = await client.PatchAsync("users/1", content);

        Assert.That(patchResponse.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.BadRequest));

    }    

    
    [Test]
    public async Task UpdateUserPassesTest()
    {
        var factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder => { });
        var client = factory.CreateClient();

        var email = "test1@test1";
        var password = "Test1test1%";
        string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        int length = 8; 
        string randomUsername = new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());

        var loginUser = new LoginRequestDTO()
        {
            email = email,
            password = password
        };
        var contentLogin = new StringContent(JsonSerializer.Serialize(loginUser), System.Text.Encoding.UTF8, "application/json");
        var loginResponse = await client.PostAsync("login", contentLogin);

        if (!loginResponse.IsSuccessStatusCode)
        {
            Assert.Fail();
        }

        string jsonResponse = await loginResponse.Content.ReadAsStringAsync();
        ResponseDTO<LoginSuccessDTO>? result = JsonSerializer.Deserialize<ResponseDTO<LoginSuccessDTO>>(jsonResponse);

        if (result is null)
        {
            Assert.Fail("Login failed");
        }

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.Data.token);
        var updateUser = new UpdateUserRequestDTO
        {
            Username = randomUsername
        };
        var content = new StringContent(JsonSerializer.Serialize(updateUser), System.Text.Encoding.UTF8, "application/json");
        var patchResponse = await client.PatchAsync("users/1", content);
        string patchResponseContent = await patchResponse.Content.ReadAsStringAsync();

        ResponseDTO<UpdateUserSuccessDTO>? updatedResult = JsonSerializer.Deserialize<ResponseDTO<UpdateUserSuccessDTO>>(
            patchResponseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        if (updatedResult is null)
        {
            Assert.Fail("Update failed");
        }
        Assert.That(patchResponse.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));
        Assert.That(updatedResult.Data.user.Username, Is.EqualTo(randomUsername));

    }

    [Test]
    public async Task UpdateUserEmailValidationPassesTest()
    {
        var factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder => { });
        var client = factory.CreateClient();

        var email = "test1@test1";
        var password = "Test1test1%";

        var loginUser = new LoginRequestDTO()
        {
            email = email,
            password = password
        };
        var contentLogin = new StringContent(JsonSerializer.Serialize(loginUser), System.Text.Encoding.UTF8, "application/json");
        var loginResponse = await client.PostAsync("login", contentLogin);

        if (!loginResponse.IsSuccessStatusCode)
        {
            Assert.Fail();
        }

        string jsonResponse = await loginResponse.Content.ReadAsStringAsync();
        ResponseDTO<LoginSuccessDTO>? result = JsonSerializer.Deserialize<ResponseDTO<LoginSuccessDTO>>(jsonResponse);

        if (result is null)
        {
            Assert.Fail("Login failed");
        }

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.Data.token);
        var updateUser = new UpdateUserRequestDTO
        {
            Email = email,
        };
        var content = new StringContent(JsonSerializer.Serialize(updateUser), System.Text.Encoding.UTF8, "application/json");
        var patchResponse = await client.PatchAsync("users/1", content);

        Assert.That(patchResponse.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));

    }

    
    [Test]
    public async Task UpdateUserPasswordValidationPassesTest()
    {
        var factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder => { });
        var client = factory.CreateClient();

        var email = "test1@test1";
        var password = "Test1test1%";

        var loginUser = new LoginRequestDTO()
        {
            email = email,
            password = password
        };
        var contentLogin = new StringContent(JsonSerializer.Serialize(loginUser), System.Text.Encoding.UTF8, "application/json");
        var loginResponse = await client.PostAsync("login", contentLogin);

        if (!loginResponse.IsSuccessStatusCode)
        {
            Assert.Fail();
        }

        string jsonResponse = await loginResponse.Content.ReadAsStringAsync();
        ResponseDTO<LoginSuccessDTO>? result = JsonSerializer.Deserialize<ResponseDTO<LoginSuccessDTO>>(jsonResponse);

        if (result is null)
        {
            Assert.Fail("Login failed");
        }

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.Data.token);
        var updateUser = new UpdateUserRequestDTO
        {
            Password = password,
        };
        var content = new StringContent(JsonSerializer.Serialize(updateUser), System.Text.Encoding.UTF8, "application/json");
        var patchResponse = await client.PatchAsync("users/1", content);

        Assert.That(patchResponse.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));
    }
    

    
    [Test]
    public async Task UpdateUserMobileValidationPassesTest()
    {
        var factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder => { });
        var client = factory.CreateClient();

        var email = "test1@test1";
        var password = "Test1test1%";

        var loginUser = new LoginRequestDTO()
        {
            email = email,
            password = password
        };
        var contentLogin = new StringContent(JsonSerializer.Serialize(loginUser), System.Text.Encoding.UTF8, "application/json");
        var loginResponse = await client.PostAsync("login", contentLogin);

        if (!loginResponse.IsSuccessStatusCode)
        {
            Assert.Fail();
        }

        string jsonResponse = await loginResponse.Content.ReadAsStringAsync();
        ResponseDTO<LoginSuccessDTO>? result = JsonSerializer.Deserialize<ResponseDTO<LoginSuccessDTO>>(jsonResponse);

        if (result is null)
        {
            Assert.Fail("Login failed");
        }

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.Data.token);
        var updateUser = new UpdateUserRequestDTO
        {
            MobileNumber = "99911555",
        };
        var content = new StringContent(JsonSerializer.Serialize(updateUser), System.Text.Encoding.UTF8, "application/json");
        var patchResponse = await client.PatchAsync("users/1", content);

        Assert.That(patchResponse.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));
    }
}
