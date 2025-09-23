using exercise.wwwapi.DTOs;
using exercise.wwwapi.DTOs.Cohorts;
using exercise.wwwapi.Endpoints;
using exercise.wwwapi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace api.tests.CohortEndpointTests
{
    public class CreateCohortTests
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
        // Checks that creating a new cohort with valid values returns Created 
        public async Task CreateCohortTest()
        {
            var newCohort = new CreateCohortRequestDTO
            {
                CohortNumber = 2,
                CohortName = "New Cohort",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddMonths(3)
            };
            var content = new StringContent(
                JsonSerializer.Serialize(newCohort),
                System.Text.Encoding.UTF8,
                "application/json"
            );
            var response = await _client.PostAsync("/cohorts", content);
            Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.Created));
        }

        [Test]
        // Checks that creating a new cohort with invalid values returns BadRequest
        public async Task CreateCohortValidationFailsTest()
        {
            var newCohort = new CreateCohortRequestDTO
            {
                CohortNumber = -2,
                CohortName = "New Cohort",
                StartDate = DateTime.MinValue,
                EndDate = DateTime.MinValue
            };
            var content = new StringContent(
                JsonSerializer.Serialize(newCohort),
                System.Text.Encoding.UTF8,
                "application/json"
            );
            var response = await _client.PostAsync("/cohorts", content);
            Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.BadRequest));
        }

        // Checks that creating a new cohort with missing required fields returns BadRequest
        [Test]
        public async Task CreateCohortMissingFieldsTest()
        {
            var newCohort = new
            {
                CohortNumber = 3,
                // CohortName is missing
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddMonths(3)
            };
            var content = new StringContent(
                JsonSerializer.Serialize(newCohort),
                System.Text.Encoding.UTF8,
                "application/json"
            );
            var response = await _client.PostAsync("/cohorts", content);
            Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.BadRequest));
        }
    }
}
