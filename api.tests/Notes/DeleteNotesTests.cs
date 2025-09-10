using exercise.wwwapi.DTOs;
using exercise.wwwapi.DTOs.Login;
using exercise.wwwapi.Endpoints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace api.tests.Notes
{
    public class DeleteNotesTests
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
        public async Task DeleteNoteTestNotFound()
        {
            int noteId = 523523;
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

            var response = await _client.DeleteAsync($"/notes/{noteId}");
            Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.NotFound));
        }

        [Test]
        public async Task DeleteNoteTestSuccess()
        {
            int noteId = 5;
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

            var response = await _client.DeleteAsync($"/notes/{noteId}");
            Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));
        }
    }
}
