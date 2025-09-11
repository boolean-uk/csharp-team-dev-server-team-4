using exercise.wwwapi.Models;
using System.Text.Json.Serialization;

namespace exercise.wwwapi.DTOs.Cohorts
{
    public class CohortsSuccessDTO
    {
        [JsonPropertyName("cohorts")]
        public List<Cohort> Cohorts { get; set; } = [];
    }
}
