using exercise.wwwapi.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.DTOs.Exercises
{
    public class GetExerciseForUserDTO
    {
        public int Id { get; set; }

        public int UnitId { get; set; }
        public string Name { get; set; }

        public string GitHubLink { get; set; }

        public string Description { get; set; }

        public bool isSubmitted { get; set; }

        public GetExerciseForUserDTO()
        {
            
        }

        public GetExerciseForUserDTO(Exercise model, ICollection<UserExercise> userExercises)
        {
            Id = model.Id;
            UnitId = model.UnitId;
            Name = model.Name;
            isSubmitted = userExercises.Any(ue => ue.ExerciseId == model.Id && ue.Submitted);
        }
    }
}
