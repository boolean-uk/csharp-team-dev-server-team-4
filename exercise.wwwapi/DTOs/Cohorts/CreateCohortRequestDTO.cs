namespace exercise.wwwapi.DTOs.Cohorts
{
    public class CreateCohortRequestDTO
    {
        public int CohortNumber { get; set; }
        public required string CohortName { get; set; }
        public DateTime StartDate { get; set; } 
        public DateTime EndDate { get; set; } 
    }
}
