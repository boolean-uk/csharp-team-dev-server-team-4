using exercise.wwwapi.Models;

namespace exercise.wwwapi.DTOs.Exercises
{
    public class UpdateCreateExerciseDTO
    {        
        public string Name { get; set; }
        public string GitHubLink { get; set; }
        public string Description { get; set; }

    }
}
