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

namespace api.tests.CommentEndpointTests
{
    public class GetCommentsPerPostTests
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
        public async Task GetCommentsForPostPassesTest()
        {
            var response = await _client.GetAsync("/posts/1/comments");
            Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));

            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);

            var comments = doc.RootElement
                .GetProperty("data")
                .GetProperty("comments");

            Assert.That(comments.ValueKind, Is.EqualTo(JsonValueKind.Array));
            Assert.That(comments.GetArrayLength(), Is.GreaterThan(0));
        }

        [Test]
        public async Task GetCommentsForUnknownPostTest()
        {
            var response = await _client.GetAsync("/posts/999999/comments");

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var json = await response.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(json);

                var comments = doc.RootElement
                    .GetProperty("data")
                    .GetProperty("comments");

                Assert.That(comments.ValueKind, Is.EqualTo(JsonValueKind.Array));
                Assert.That(comments.GetArrayLength(), Is.EqualTo(0));
            }
            else
            {
                Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.NotFound));
            }
        }
    }
}
