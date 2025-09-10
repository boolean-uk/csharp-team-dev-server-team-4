using exercise.wwwapi.DTOs;
using exercise.wwwapi.DTOs.GetUsers;
using exercise.wwwapi.DTOs.Login;
using exercise.wwwapi.DTOs.Notes;
using exercise.wwwapi.Endpoints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace api.tests.Notes;

public class UnauthorizedNotesTests
{
    private HttpClient _client;
    private StringContent? _contentLogin;
    private StringContent? _contentCreate;
    private StringContent? _contentUpdate;

    [SetUp]
    public void Setup()
    {
        _client = TestUtils.CreateClient();

        // login
        var loginUser = new LoginRequestDTO
        {
            Email = "test1@test1",
            Password = "Test1test1%"
        };

        var _contentLogin = new StringContent(
            JsonSerializer.Serialize(loginUser),
            Encoding.UTF8,
            "application/json"
        );

        // POST note
        var createNote = new CreateNoteRequestDTO
        {
            Title = "test",
            Content = "test"
        };
        var _contentCreate = new StringContent(
            JsonSerializer.Serialize(createNote),
            Encoding.UTF8,
            "application/json"
        );

        // PATCH note
        var updateNote = new UpdateNoteRequestDTO 
        { 
            Title = "test2",
            Content = "test2" 
        };

        var _contentUpdate = new StringContent(
            JsonSerializer.Serialize(updateNote),
            Encoding.UTF8,
            "application/json"
        );
    }

    [TearDown]
    public void TearDown()
    {
        _client.Dispose();

        _contentLogin?.Dispose();        
        _contentCreate?.Dispose();
        _contentUpdate?.Dispose();
    }

    [Test]
    public async Task GetNoteByIdByNonUserReceiveUnauthorizedTest()
    {
        var response = await _client.GetAsync("/notes/1");
        Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.Unauthorized));
    }

    [Test]
    public async Task GetNoteByIdByStudentReceiveUnauthorizedTest()
    {
        var loginResponse = await _client.PostAsync("login", _contentLogin);
        Assert.That(loginResponse.IsSuccessStatusCode, Is.True);

        var jsonResponse = await loginResponse.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<ResponseDTO<LoginSuccessDTO>>(jsonResponse);
        Assert.That(result, Is.Not.Null);

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.Data.Token);

        var response = await _client.GetAsync("/notes/1");
        Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.Unauthorized));
    }

    [Test]
    public async Task GetStudentNotesByNonUserReceiveUnauthorizedTest()
    {
        var response = await _client.GetAsync("/users/1/notes");
        Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.Unauthorized));
    }

    [Test]
    public async Task GetStudentNotesByStudentReceiveUnauthorizedTest()
    {
        var loginResponse = await _client.PostAsync("login", _contentLogin);
        Assert.That(loginResponse.IsSuccessStatusCode, Is.True);

        var jsonResponse = await loginResponse.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<ResponseDTO<LoginSuccessDTO>>(jsonResponse);
        Assert.That(result, Is.Not.Null);

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.Data.Token);

        var response = await _client.GetAsync("/users/1/notes");
        Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.Unauthorized));
    }

    [Test]
    public async Task CreateStudentNoteByNonUserReceiveUnauthorizedTest()
    {
        var response = await _client.PostAsync("/users/1/notes", _contentCreate);
        Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.Unauthorized));
    }

    [Test]
    public async Task CreateStudentNoteByStudentReceiveUnauthorizedTest()
    {
        var loginResponse = await _client.PostAsync("login", _contentLogin);
        Assert.That(loginResponse.IsSuccessStatusCode, Is.True);

        var jsonResponse = await loginResponse.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<ResponseDTO<LoginSuccessDTO>>(jsonResponse);
        Assert.That(result, Is.Not.Null);

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.Data.Token);

        var response = await _client.PostAsync("/users/1/notes", _contentCreate);
        Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.Unauthorized));
    }

    [Test]
    public async Task DeleteStudentNoteByNonUserReceiveUnauthorizedTest()
    {
        var response = await _client.DeleteAsync("/notes/1");
        Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.Unauthorized));
    }

    [Test]
    public async Task DeleteStudentNoteByStudentReceiveUnauthorizedTest()
    {
        var loginResponse = await _client.PostAsync("login", _contentLogin);
        Assert.That(loginResponse.IsSuccessStatusCode, Is.True);

        var jsonResponse = await loginResponse.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<ResponseDTO<LoginSuccessDTO>>(jsonResponse);
        Assert.That(result, Is.Not.Null);

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.Data.Token);

        var response = await _client.GetAsync("/users/1/notes");
        Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.Unauthorized));
    }

    [Test]
    public async Task UpdateStudentNoteByNonUserReceiveUnauthorizedTest()
    {
        var response = await _client.PatchAsync("/notes/1", _contentUpdate);
        Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.Unauthorized));
    }

    [Test]
    public async Task UpdateStudentNoteByStudentReceiveUnauthorizedTest()
    {
        var loginResponse = await _client.PostAsync("login", _contentLogin);
        Assert.That(loginResponse.IsSuccessStatusCode, Is.True);

        var jsonResponse = await loginResponse.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<ResponseDTO<LoginSuccessDTO>>(jsonResponse);
        Assert.That(result, Is.Not.Null);

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.Data.Token);

        var response = await _client.PatchAsync("/notes/1", _contentUpdate);
        Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.Unauthorized));
    }
}
