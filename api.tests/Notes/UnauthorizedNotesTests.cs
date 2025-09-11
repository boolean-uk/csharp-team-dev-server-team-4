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
    public async Task GetNoteByIdByNonUserReceiveUnauthorizedTest()
    {
        var response = await _client.GetAsync("/notes/1");
        Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.Unauthorized));
    }

    [Test]
    public async Task GetNoteByIdByStudentReceiveUnauthorizedTest()
    {
        await AuthenticateAsStudentAsync();

        var response = await _client.GetAsync("/notes/1");
        Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.Unauthorized));
    }

    [Test]
    public async Task GetAllStudentNotesByNonUserReceiveUnauthorizedTest()
    {
        var response = await _client.GetAsync("/users/1/notes/");
        Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.Unauthorized));
    }

    [Test]
    public async Task GetAllStudentNotesByStudentReceiveUnauthorizedTest()
    {
        await AuthenticateAsStudentAsync();

        var response = await _client.GetAsync("/users/1/notes");
        Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.Unauthorized));
    }

    [Test]
    public async Task CreateStudentNoteByNonUserReceiveUnauthorizedTest()
    {
        var createNote = new CreateNoteRequestDTO
        {
            Title = "test",
            Content = "test"
        };
        var contentCreate = new StringContent(
            JsonSerializer.Serialize(createNote),
            Encoding.UTF8,
            "application/json"
        );

        var response = await _client.PostAsync("/users/1/notes", contentCreate);
        Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.Unauthorized));
    }

    [Test]
    public async Task CreateStudentNoteByStudentReceiveUnauthorizedTest()
    {
        await AuthenticateAsStudentAsync();

        var createNote = new CreateNoteRequestDTO
        {
            Title = "test",
            Content = "test"
        };
        var contentCreate = new StringContent(
            JsonSerializer.Serialize(createNote),
            Encoding.UTF8,
            "application/json"
        );

        var response = await _client.PostAsync("/users/1/notes", contentCreate);
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
        await AuthenticateAsStudentAsync();

        var response = await _client.DeleteAsync("/notes/1");
        Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.Unauthorized));
    }

    [Test]
    public async Task UpdateStudentNoteByNonUserReceiveUnauthorizedTest()
    {
        var updateNote = new UpdateNoteRequestDTO
        {
            Title = "test2",
            Content = "test2"
        };

        var contentUpdate = new StringContent(
            JsonSerializer.Serialize(updateNote),
            Encoding.UTF8,
            "application/json"
        );

        var response = await _client.PatchAsync("/notes/1", contentUpdate);
        Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.Unauthorized));
    }

    [Test]
    public async Task UpdateStudentNoteByStudentReceiveUnauthorizedTest()
    {
        await AuthenticateAsStudentAsync();

        var updateNote = new UpdateNoteRequestDTO
        {
            Title = "test2",
            Content = "test2"
        };

        var contentUpdate = new StringContent(
            JsonSerializer.Serialize(updateNote),
            Encoding.UTF8,
            "application/json"
        );

        var response = await _client.PatchAsync("/notes/1", contentUpdate);
        Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.Unauthorized));
    }

    private async Task AuthenticateAsStudentAsync()
    {
        var loginUser = new LoginRequestDTO
        {
            Email = "test1@test1",
            Password = "Test1test1%"
        };

        var loginContent = new StringContent(
            JsonSerializer.Serialize(loginUser),
            Encoding.UTF8,
            "application/json"
        );

        var loginResponse = await _client.PostAsync("login", loginContent);
        Assert.That(loginResponse.IsSuccessStatusCode, Is.True);

        var loginJson = await loginResponse.Content.ReadAsStringAsync();
        var login = JsonSerializer.Deserialize<ResponseDTO<LoginSuccessDTO>>(loginJson);
        Assert.That(login, Is.Not.Null);

        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", login!.Data.Token);
    }
}
