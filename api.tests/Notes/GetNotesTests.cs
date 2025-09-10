using exercise.wwwapi.DTOs;
using exercise.wwwapi.DTOs.Login;
using exercise.wwwapi.DTOs.Notes;
using exercise.wwwapi.Endpoints;
using exercise.wwwapi.Models.UserInfo;
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
        public async Task TeacherGetNotesOnAStudentSuccess()
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
        }

        [Test]
        public async Task TeacherGetNotesOnStudentWithNoNotesSuccess()
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
        }
    }
}
