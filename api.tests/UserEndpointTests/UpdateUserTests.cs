using exercise.wwwapi.DTOs;
using exercise.wwwapi.DTOs.Login;
using exercise.wwwapi.DTOs.UpdateUser;
using System.Net.Http.Headers;
using System.Text.Json;
using exercise.wwwapi.Endpoints;

namespace api.tests.UserEndpointTests;

public class UpdateUserTests
{
    private Random _random;
    private HttpClient _client;
        
    [SetUp]
    public void Setup()
    {
        _client = TestUtils.CreateClient();
        _random = new Random();
    }

    [TearDown]
    public void TearDown()
    {
        _client.Dispose();
    }

    [Test]
    public async Task UpdateUser401UnauthorizedTest()
    {
        const string email = "Test";

        var updateUser = new UpdateUserRequestDTO
        {
            Email = email,
        };
        var content = new StringContent(
            JsonSerializer.Serialize(updateUser), 
            System.Text.Encoding.UTF8,
            "application/json"
            );
        var patchResponse = await _client.PatchAsync("users/1", content);

        Assert.That(patchResponse.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.Unauthorized));
    }

    [Test]
    public async Task UpdateUserEmailValidationFailsTest()
    {
        const string email = "test1@test1";
        const string password = "Test1test1%";

        var loginUser = new LoginRequestDTO
        {
            Email = email,
            Password = password
        };

        var contentLogin = new StringContent(
            JsonSerializer.Serialize(loginUser),
            System.Text.Encoding.UTF8,
            "application/json"
        );
        var loginResponse = await _client.PostAsync("login", contentLogin);
        Assert.That(loginResponse.IsSuccessStatusCode, Is.True);

        var jsonResponse = await loginResponse.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<ResponseDTO<LoginSuccessDTO>>(jsonResponse);
        Assert.That(result, Is.Not.Null);

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.Data.Token);
        var updateUser = new UpdateUserRequestDTO
        {
            Email = "Test"
        };

        var content = new StringContent(
            JsonSerializer.Serialize(updateUser),
            System.Text.Encoding.UTF8,
            "application/json"
        );

        var patchResponse = await _client.PatchAsync("users/1", content);

        Assert.That(patchResponse.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.BadRequest));
    }

    [Test]
    public async Task UpdateUserPasswordValidationFailsTest()
    {
        const string email = "test1@test1";
        const string password = "Test1test1%";

        var loginUser = new LoginRequestDTO
        {
            Email = email,
            Password = password
        };

        var contentLogin = new StringContent(
            JsonSerializer.Serialize(loginUser),
            System.Text.Encoding.UTF8,
            "application/json"
        );
        var loginResponse = await _client.PostAsync("login", contentLogin);
        Assert.That(loginResponse.IsSuccessStatusCode, Is.True);

        var jsonResponse = await loginResponse.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<ResponseDTO<LoginSuccessDTO>>(jsonResponse);
        Assert.That(result, Is.Not.Null);

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.Data.Token);
        var updateUser = new UpdateUserRequestDTO
        {
            Password = "pass",
        };

        var content = new StringContent(
            JsonSerializer.Serialize(updateUser),
            System.Text.Encoding.UTF8,
            "application/json"
        );
        var patchResponse = await _client.PatchAsync("users/1", content);

        Assert.That(patchResponse.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.BadRequest));
    }

    [Test]
    public async Task UpdateUserMobileNumberValidationFailsTest()
    {
        const string email = "test1@test1";
        const string password = "Test1test1%";
        const string phone = "test";

        var loginUser = new LoginRequestDTO
        {
            Email = email,
            Password = password
        };
        var contentLogin = new StringContent(
            JsonSerializer.Serialize(loginUser),
            System.Text.Encoding.UTF8,
            "application/json"
        );
        var loginResponse = await _client.PostAsync("login", contentLogin);
        Assert.That(loginResponse.IsSuccessStatusCode, Is.True);

        var jsonResponse = await loginResponse.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<ResponseDTO<LoginSuccessDTO>>(jsonResponse);
        Assert.That(result, Is.Not.Null, "Login Failed");

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.Data.Token);
        var updateUser = new UpdateUserRequestDTO
        {
            Phone = phone,
        };
        var content = new StringContent(
            JsonSerializer.Serialize(updateUser),
            System.Text.Encoding.UTF8,
            "application/json"
        );
        var patchResponse = await _client.PatchAsync("users/1", content);

        Assert.That(patchResponse.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.BadRequest));
    }

    [Test]
    public async Task UpdateUserPassesTest()
    {
        const string email = "test1@test1";
        const string password = "Test1test1%";
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        const int length = 8;
        var randomUsername = new string(Enumerable.Repeat(chars, length)
            .Select(s => s[_random.Next(s.Length)]).ToArray());

        var loginUser = new LoginRequestDTO
        {
            Email = email,
            Password = password
        };
        var contentLogin = new StringContent(JsonSerializer.Serialize(loginUser), System.Text.Encoding.UTF8,
            "application/json");
        var loginResponse = await _client.PostAsync("login", contentLogin);
        Assert.That(loginResponse.IsSuccessStatusCode, Is.True);

        var jsonResponse = await loginResponse.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<ResponseDTO<LoginSuccessDTO>>(jsonResponse);
        Assert.That(result, Is.Not.Null);

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.Data.Token);
        var updateUser = new UpdateUserRequestDTO
        {
            Username = randomUsername
        };
        var content = new StringContent(JsonSerializer.Serialize(updateUser), System.Text.Encoding.UTF8,
            "application/json");
        var patchResponse = await _client.PatchAsync("users/1", content);
        var patchResponseContent = await patchResponse.Content.ReadAsStringAsync();
        var updatedResult = JsonSerializer.Deserialize<ResponseDTO<UpdateUserSuccessDTO>>(patchResponseContent);

        Assert.That(updatedResult, Is.Not.Null, "Update Failed");
        Assert.That(patchResponse.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));
        Assert.That(updatedResult!.Data.Username, Is.EqualTo(randomUsername));
    }

    [Test]
    public async Task UpdateUserEmailValidationPassesTest()
    {
        const string email = "test1@test1";
        const string password = "Test1test1%";
        const string username = "username111";

        var loginUser = new LoginRequestDTO
        {
            Email = email,
            Password = password
        };
        var contentLogin = new StringContent(
            JsonSerializer.Serialize(loginUser),
            System.Text.Encoding.UTF8,
            "application/json"
        );
        var loginResponse = await _client.PostAsync("login", contentLogin);

        if (!loginResponse.IsSuccessStatusCode)
        {
            Assert.Fail();
        }

        var jsonResponse = await loginResponse.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<ResponseDTO<LoginSuccessDTO>>(jsonResponse);
        Assert.That(result, Is.Not.Null, "Login failed");

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.Data.Token);
        var updateUser = new UpdateUserRequestDTO
        {
            Email = email,
            Username = username,
        };
        var content = new StringContent(
            JsonSerializer.Serialize(updateUser),
            System.Text.Encoding.UTF8,
            "application/json"
        );
        var patchResponse = await _client.PatchAsync("users/1", content);

        Assert.That(patchResponse.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));

        var patchResponseContent = await patchResponse.Content.ReadAsStringAsync();
        var updatedResult = JsonSerializer.Deserialize<ResponseDTO<UpdateUserSuccessDTO>>(patchResponseContent);
        Assert.That(updatedResult, Is.Not.Null, "Update Failed");
        Assert.That(updatedResult!.Data.Username, Is.EqualTo(username));
    }


    [Test]
    public async Task UpdateUserPasswordValidationPassesTest()
    {
        const string email = "test1@test1";
        const string password = "Test1test1%";
        const string username = "username222";

        var loginUser = new LoginRequestDTO()
        {
            Email = email,
            Password = password
        };
        var contentLogin = new StringContent(
            JsonSerializer.Serialize(loginUser),
            System.Text.Encoding.UTF8,
            "application/json"
        );
        var loginResponse = await _client.PostAsync("login", contentLogin);
        Assert.That(loginResponse.IsSuccessStatusCode, Is.True);

        var jsonResponse = await loginResponse.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<ResponseDTO<LoginSuccessDTO>>(jsonResponse);
        Assert.That(result, Is.Not.Null, "Login failed");

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.Data.Token);
        var updateUser = new UpdateUserRequestDTO
        {
            Password = password,
            Username = username
        };
        var content = new StringContent(
            JsonSerializer.Serialize(updateUser),
            System.Text.Encoding.UTF8,
            "application/json"
        );
        var patchResponse = await _client.PatchAsync("users/1", content);
        Assert.That(patchResponse.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));

        var patchResponseContent = await patchResponse.Content.ReadAsStringAsync();
        var updatedResult = JsonSerializer.Deserialize<ResponseDTO<UpdateUserSuccessDTO>>(patchResponseContent);
        Assert.That(updatedResult, Is.Not.Null, "Update Failed");
        Assert.That(updatedResult!.Data.Username, Is.EqualTo(username));
    }


    [Test]
    public async Task UpdateUserMobileValidationPassesTest()
    {
        const string email = "test1@test1";
        const string password = "Test1test1%";
        const string username = "username333";
        const string phone = "99911555";

        var loginUser = new LoginRequestDTO
        {
            Email = email,
            Password = password
        };
        var contentLogin = new StringContent(
            JsonSerializer.Serialize(loginUser),
            System.Text.Encoding.UTF8,
            "application/json"
        );
        var loginResponse = await _client.PostAsync("login", contentLogin);
        Assert.That(loginResponse.IsSuccessStatusCode, Is.True);

        var jsonResponse = await loginResponse.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<ResponseDTO<LoginSuccessDTO>>(jsonResponse);
        Assert.That(result, Is.Not.Null, "Login failed");

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.Data.Token);
        var updateUser = new UpdateUserRequestDTO
        {
            Phone = phone,
            Username = username,
        };
        var content = new StringContent(
            JsonSerializer.Serialize(updateUser),
            System.Text.Encoding.UTF8,
            "application/json"
        );

        var patchResponse = await _client.PatchAsync("users/1", content);
        Assert.That(patchResponse.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));

        var patchResponseContent = await patchResponse.Content.ReadAsStringAsync();
        var updatedResult = JsonSerializer.Deserialize<ResponseDTO<UpdateUserSuccessDTO>>(patchResponseContent);
        Assert.That(updatedResult, Is.Not.Null, "Update Failed");
        Assert.That(updatedResult!.Data.Username, Is.EqualTo(username));
    }
}