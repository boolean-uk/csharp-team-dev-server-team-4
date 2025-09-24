using exercise.wwwapi.Endpoints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace api.tests.CohortEndpointTests
{
    public class UpdateCohortTests
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
        // Checks that updating an existing cohort returns OK status
        public async Task UpdateCohortTest()
        {
            var updatedCohort = new
            {
                CohortNumber = 1,
                CohortName = "Updated Cohort Name",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddMonths(4)
            };
            var content = new StringContent(
                System.Text.Json.JsonSerializer.Serialize(updatedCohort),
                System.Text.Encoding.UTF8,
                "application/json"
            );
            var response = await _client.PatchAsync("/cohorts/1", content);
            Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));
        }

        [Test]
        // Checks that updating a non-existent cohort returns NotFound status
        public async Task UpdateCohortNotFoundTest()
        {
            var updatedCohort = new
            {
                CohortNumber = 99999,
                CohortName = "Non-existent Cohort",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddMonths(4)
            };
            var content = new StringContent(
                System.Text.Json.JsonSerializer.Serialize(updatedCohort),
                System.Text.Encoding.UTF8,
                "application/json"
            );
            var response = await _client.PatchAsync("/cohorts/99999", content);
            Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.NotFound));
        }

        [Test]
        // Checks that updating a cohort with invalid data returns BadRequest status
        public async Task UpdateCohortValidationFailsTest()
        {
            var updatedCohort = new
            {
                CohortNumber = 0,
                CohortName = "",
                StartDate = DateTime.MinValue,
                EndDate = DateTime.MinValue
            };
            var content = new StringContent(
                System.Text.Json.JsonSerializer.Serialize(updatedCohort),
                System.Text.Encoding.UTF8,
                "application/json"
            );
            var response = await _client.PatchAsync("/cohorts/1", content);
            Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.BadRequest));
        }
    }
}
