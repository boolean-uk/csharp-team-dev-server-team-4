
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace exercise.wwwapi.Models;

public class CohortDTO
{
    [JsonPropertyName("course_id")]

    public int CohortNumber { get; set; }

    public string CohortName { get; set; }

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}