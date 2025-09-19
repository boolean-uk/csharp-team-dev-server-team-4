
using System.Text.Json.Serialization;

public class CohortPostDTO
{
    public string CohortName { get; set; }

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}