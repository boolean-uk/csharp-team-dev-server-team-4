using exercise.wwwapi.DTOs;
using exercise.wwwapi.DTOs.Login;
using exercise.wwwapi.DTOs.Notes;
using exercise.wwwapi.Endpoints;
using exercise.wwwapi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace api.tests.Notes
{
    public class CreateNoteTests
    {
        private HttpClient _client;
        private const string MANY_LETTERS = "hwfafwfwktyseqiklipfnxcmjrssaxcifgnixotgxrbwgnxksjdzpjrncphdeblhluetheclxjkixejuuhojmpmtkkmapetqogavojolqopxghpszcmnjyzzhimhkagarppesnmwovcwjlkajuzytzxbildggqnkwfpickwumymachzorlbddihdiyvxmfgivittwwprsrxagzhewywfoygheozougtbysbjuxwhltmvdoxermzaeepfkpphteiylumtjccalnolnyodlawqrvftxdyxxfhdrelmacpgohhascldwqjgalcppvlqxhfpjzufwsxdyxisgbihhvvhsgqoqdtfxgidetdduceknvznmmurcoxqqdkzlxrmrjlmzkattyshyljiwetfrhkyqpynfigrixocbzmjrbahkceaqdczsnlfbktzfqyotvpyqirxdktkekeklfnarqwuonwywxflvimmldeoeyyzqcrefclwlfthnofhthbksopebtlclyulwpnqyrcyyhajfhxwlytermugspbehowtsqdzrneqihsunxwjjapvnmpnztkygkxlqiairfdhqbtokeamnlctovwjbzaimosbttlcqnmfffmmlhgpxquunqjdsiniieoacjlltaoqlligkczfgoecmetdgiiafqpkyziusqjdmvdmsmgrhxqjspwogicuegcwqjrklavqqkyzgcpkaijeecxujfbqsamqgjkazxvcopasytqugmdohovynvhozxsposdjwzyzxkntjqlokilukvuhndtjfmjmwjjtahgppfistvkylnsardbaccvqmanccfzbexcbvzwojjobywfgdzjgigqshwwwgkkyoyjskpvwsmjwwourolfbxcehguncehdqvfkzhatkpqulbxrkpoqzdzbgxjzqlztjlugxndrnyuvbxzlbijqlrdzcuiigtyiiabdsaeylffijifhkfaxqfnbifgtxtlynnyc";

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
        public async Task CreateNoteTestTitleValidationFails()
        {
            await AuthenticateAsTeacherAsync();

            int userId = 1;
            int noteId = 1;

            var createNote = new CreateNoteRequestDTO
            {
                Title = "",
                Content = "new content"
            };

            var createJson = new StringContent(
                JsonSerializer.Serialize(createNote),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _client.PostAsync($"/users/{userId}/notes", createJson);
            Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task CreateNoteTestContentValidationFails()
        {
            await AuthenticateAsTeacherAsync();

            int userId = 1;
            int noteId = 1;

            var createNote = new CreateNoteRequestDTO
            {
                Title = "new title",
                Content = ""
            };

            var createJson = new StringContent(
                JsonSerializer.Serialize(createNote),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _client.PostAsync($"/users/{userId}/notes", createJson);
            Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task CreateNoteTestTitleTooLongValidationFails()
        {
            await AuthenticateAsTeacherAsync();

            int userId = 1;
            int noteId = 1;

            var createNote = new CreateNoteRequestDTO
            {
                Title = MANY_LETTERS,
                Content = "test"
            };

            var createJson = new StringContent(
                JsonSerializer.Serialize(createNote),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _client.PostAsync($"/users/{userId}/notes", createJson);
            Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task CreateNoteTestContentTooLongValidationFails()
        {
            await AuthenticateAsTeacherAsync();

            int userId = 1;
            int noteId = 1;

            var createNote = new CreateNoteRequestDTO
            {
                Content = MANY_LETTERS,
                Title = "test"
            };

            var createJson = new StringContent(
                JsonSerializer.Serialize(createNote),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _client.PostAsync($"/users/{userId}/notes", createJson);
            Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task CreateNoteTestSuccess()
        {
            await AuthenticateAsTeacherAsync();

            int userId = 1;
            int noteId = 1;

            var createNote = new CreateNoteRequestDTO
            {
                Title = "new title",
                Content = "new content"
            };

            var createJson = new StringContent(
                JsonSerializer.Serialize(createNote),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _client.PostAsync($"/users/{userId}/notes", createJson);
            Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));
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
