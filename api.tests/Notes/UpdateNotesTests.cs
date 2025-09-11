using exercise.wwwapi.DTOs;
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

public class UpdateNotesTests
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
    public async Task UpdateNoteTestNoteNotFound()
    {
        await AuthenticateAsTeacherAsync();

        int noteId = 15476587;

        var updateNote = new UpdateNoteRequestDTO
        {
            Title = "changed title",
            Content = "changed content"
        };

        var updateJson = new StringContent(
            JsonSerializer.Serialize(updateNote),
            Encoding.UTF8,
            "application/json"
        );

        var response = await _client.PatchAsync($"/notes/{noteId}", updateJson);
        Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.NotFound));
    }

    [Test]
    public async Task UpdateNoteTestTitleValidationFails()
    {
        await AuthenticateAsTeacherAsync();

        int noteId = 1;

        var updateNote = new UpdateNoteRequestDTO
        {
            Title = "",
            Content = "changed content"
        };

        var updateJson = new StringContent(
            JsonSerializer.Serialize(updateNote),
            Encoding.UTF8,
            "application/json"
        );

        var response = await _client.PatchAsync($"/notes/{noteId}", updateJson);
        Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.BadRequest));
    }

    [Test]
    public async Task UpdateNoteTestContentValidationFails()
    {
        await AuthenticateAsTeacherAsync();

        int noteId = 1;

        var updateNote = new UpdateNoteRequestDTO
        {
            Title = "changed title",
            Content = ""
        };

        var updateJson = new StringContent(
            JsonSerializer.Serialize(updateNote),
            Encoding.UTF8,
            "application/json"
        );

        var response = await _client.PatchAsync($"/notes/{noteId}", updateJson);
        Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.BadRequest));
    }

    [Test]
    public async Task UpdateNoteTestTitleTooLongValidationFails()
    {
        await AuthenticateAsTeacherAsync();

        int noteId = 1;

        var updateNote = new UpdateNoteRequestDTO
        {
            Title = MANY_LETTERS,
            Content = "test"
        };

        var updateJson = new StringContent(
            JsonSerializer.Serialize(updateNote),
            Encoding.UTF8,
            "application/json"
        );

        var response = await _client.PatchAsync($"/notes/{noteId}", updateJson);
        Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.BadRequest));
    }

    [Test]
    public async Task UpdateNoteTestContentTooLongValidationFails()
    {
        await AuthenticateAsTeacherAsync();

        int noteId = 1;

        var updateNote = new UpdateNoteRequestDTO
        {
            Content = MANY_LETTERS,
            Title = "test"
        };

        var updateJson = new StringContent(
            JsonSerializer.Serialize(updateNote),
            Encoding.UTF8,
            "application/json"
        );

        var response = await _client.PatchAsync($"/notes/{noteId}", updateJson);
        Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.BadRequest));
    }

    [Test]
    public async Task UpdateNoteTestSuccess()
    {
        await AuthenticateAsTeacherAsync();

        int noteId = 1;

        var updateNote = new UpdateNoteRequestDTO
        {
            Title = "changed title",
            Content = "changed content"
        };

        var updateJson = new StringContent(
            JsonSerializer.Serialize(updateNote),
            Encoding.UTF8,
            "application/json"
        );

        var response = await _client.PatchAsync($"/notes/{noteId}", updateJson);
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
