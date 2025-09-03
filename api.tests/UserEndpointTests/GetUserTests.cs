using exercise.wwwapi.DTOs;
using exercise.wwwapi.DTOs.Login;
using exercise.wwwapi.DTOs.Register;
using exercise.wwwapi.DTOs.UpdateUser;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace api.tests.UserEndpointTests
{
    public class GetUserTests
    {

        [Test]
        public async Task GetUserPassesTest()
        {
            var factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder => { });
            var client = factory.CreateClient();
            var email = "test1@test1";

            var userId = 1;
            var getUserResponse = await client.GetAsync($"users/{userId}");
            Assert.That(getUserResponse.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));

            string getUserJson = await getUserResponse.Content.ReadAsStringAsync();
            var responseDto = JsonSerializer.Deserialize<ResponseDTO<UserDTO>>(getUserJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.That(responseDto.Data.Email, Is.EqualTo(email));

        }

        [Test]
        public async Task GetUserNotFoundTest()
        {
            var factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder => { });
            var client = factory.CreateClient();

            var userId = 1353275;
            await client.DeleteAsync($"users/{userId}");
            var getUserResponse = await client.GetAsync($"users/{userId}");
            Assert.That(getUserResponse.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.NotFound));
        }

    }
}

