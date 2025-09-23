using exercise.wwwapi.Endpoints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace api.tests.CohortEndpointTests
{
    public class DeleteCohortTests
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
        // Checks that deleting an existing cohort returns OK 
        public async Task DeleteCohortTest()
        {
            var response = await _client.DeleteAsync("/cohorts/1");
            Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));
        }

        [Test]
        // Checks that deleting a non-existent cohort returns NotFound
        public async Task DeleteCohortNotFoundTest()
        {
            var response = await _client.DeleteAsync("/cohorts/99999");
            Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.NotFound));
        }

        [Test]
        // Checks that deleting a cohort with an invalid id returns BadRequest
        public async Task DeleteCohortInvalidIdTest()
        {
            var response = await _client.DeleteAsync("/cohorts/invalid");
            Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.BadRequest));
        }
    }
}
