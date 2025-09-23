using exercise.wwwapi.Endpoints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace api.tests.CohortEndpointTests
{
    public class GetCohortTests
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
        // Checks that getting all cohorts returns OK
        public async Task GetAllCohortsTest()
        {
            var getCohortsResponse = await _client.GetAsync($"cohorts");
            Assert.That(getCohortsResponse.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));
        }

        [Test]
        // Checks that getting a specific cohort by ID returns OK
        public async Task GetCohortByIdTest()
        {
            var getCohortResponse = await _client.GetAsync($"cohorts/1");
            Assert.That(getCohortResponse.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));
        }

        [Test]
        // Checks that getting a cohort with an invalid id returns BadRequest
        public async Task GetCohortByIdBadRequestTest()
        {
            var response = await _client.GetAsync("cohorts/notanumber");
            Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.BadRequest));
        }

        [Test]
        // Checks that getting a non-existent cohort returns NotFound
        public async Task GetCohortByIdNotFoundTest()
        {
            var getCohortResponse = await _client.GetAsync($"cohorts/999999999");
            Assert.That(getCohortResponse.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.NotFound));
        }
    }
}
