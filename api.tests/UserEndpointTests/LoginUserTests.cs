using exercise.wwwapi.DTOs.Login;
using exercise.wwwapi.DTOs.Register;
using exercise.wwwapi.DTOs.UpdateUser;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace api.tests.UserEndpointTests
{
    public class LoginUserTests
    {
        [Test]
        public async Task UserLoginSucceds()
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

            Assert.That(loginResponse.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));
        }

        [Test]
        public async Task UserLoginFails()
        {
            var factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder => { });
            var client = factory.CreateClient();

            var email = "test1@test1";
            var password = "TESTtest7&aaaaaaaaaaaa";

            var newUser = new RegisterRequestDTO
            {
                email = email,
                password = password
            };

            var contentRegister = new StringContent(JsonSerializer.Serialize(newUser), System.Text.Encoding.UTF8, "application/json");
            var registerResponse = await client.PostAsync("/users", contentRegister);

            var loginUser = new LoginRequestDTO()
            {
                email = email,
                password = "dwawdfwawfawfw"
            };
            var contentLogin = new StringContent(JsonSerializer.Serialize(loginUser), System.Text.Encoding.UTF8, "application/json");
            var loginResponse = await client.PostAsync("login", contentLogin);

            Assert.That(loginResponse.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.BadRequest));
        }
    }
}
