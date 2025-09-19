using exercise.wwwapi.DTOs;
using exercise.wwwapi.DTOs.Login;
using exercise.wwwapi.DTOs.Notes;
using exercise.wwwapi.Endpoints;
using exercise.wwwapi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace api.tests.Notes
{
    public class GetNotesTests
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
        public async Task GetNoteByIdSuccess()
        {
            // login to a teacher user
            var loginUser = new LoginRequestDTO
            {
                Email = "test2@test2",
                Password = "Test2test2%"
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

            var noteId = 1;
            var response = await _client.GetAsync($"/notes/{noteId}");

            Assert.That(response.IsSuccessStatusCode, Is.True);

            var responseJson = await response.Content.ReadAsStringAsync();
            var noteResult = JsonSerializer.Deserialize<ResponseDTO<NoteDTO>>(responseJson,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.That(noteResult, Is.Not.Null);
            Assert.That(noteResult.Status, Is.EqualTo("success"));
            Assert.That(noteResult.Data, Is.Not.Null);
            Assert.That(noteResult.Data.Id, Is.EqualTo(noteId));
            Assert.That(noteResult.Data.Title, Is.EqualTo("Title Note 1"));
            Assert.That(noteResult.Data.Content, Is.EqualTo("note1note1 note1 note1 content")); 
        }

        [Test]
        public async Task GetNoteByIdFails()
        {
            await AuthenticateAsTeacherAsync();

            var noteId = 999; // note does not exist
            var response = await _client.GetAsync($"/notes/{noteId}");

            Assert.That(response.IsSuccessStatusCode, Is.False);
        }

        [Test]
        public async Task TeacherGetNotesOnAStudentSuccess()
        {
            await AuthenticateAsTeacherAsync();

            var userId = 2; // student user id to get notes for
            var getNotesResponse = await _client.GetAsync($"/users/{userId}/notes");

            Assert.That(getNotesResponse.IsSuccessStatusCode, Is.True);

            var notesJson = await getNotesResponse.Content.ReadAsStringAsync();
            var notesResult = JsonSerializer.Deserialize<ResponseDTO<NotesResponseDTO>>(notesJson,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.That(notesResult, Is.Not.Null);
            Assert.That(notesResult.Status, Is.EqualTo("success"));
            Assert.That(notesResult.Data, Is.Not.Null);
            Assert.That(notesResult.Data.Notes, Is.Not.Empty);
            Assert.That(notesResult.Data.Notes.Count, Is.EqualTo(1));
        }

        [Test]
        public async Task TeacherGetNotesOnStudentWithNoNotesSuccess()
        {
            await AuthenticateAsTeacherAsync();

            var userId = 5; // student user id to get notes for but user has no notes
            var getNotesResponse = await _client.GetAsync($"/users/{userId}/notes");

            Assert.That(getNotesResponse.IsSuccessStatusCode, Is.True);

            var notesJson = await getNotesResponse.Content.ReadAsStringAsync();
            var notesResult = JsonSerializer.Deserialize<ResponseDTO<NotesResponseDTO>>(notesJson,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.That(notesResult, Is.Not.Null);
            Assert.That(notesResult.Status, Is.EqualTo("success"));
            Assert.That(notesResult.Data, Is.Not.Null);
            Assert.That(notesResult.Data.Notes, Is.Empty);
            Assert.That(notesResult.Data.Notes.Count, Is.EqualTo(0));
        }

        private async Task AuthenticateAsTeacherAsync()
        {
            var loginUser = new LoginRequestDTO
            {
                Email = "test2@test2",
                Password = "Test2test2%"
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
}
