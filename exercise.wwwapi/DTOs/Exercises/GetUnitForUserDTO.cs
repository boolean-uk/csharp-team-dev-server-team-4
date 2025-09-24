using exercise.wwwapi.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.DTOs.Exercises
{
    public class GetUnitForUserDTO

    {
        public int Id { get; set; }
        public int ModuleId { get; set; }
        public string Name { get; set; }

        public bool IsCompleted { get; set; }

        public ICollection<GetExerciseForUserDTO> Exercises { get; set; } = new List<GetExerciseForUserDTO>();

        public GetUnitForUserDTO()
        {
            
        }

        public GetUnitForUserDTO(Unit model, ICollection<UserExercise> userExercises)
        {
            Id = model.Id;
            ModuleId = model.ModuleId;
            Name = model.Name;
            Exercises = model.Exercises.Select(e => new GetExerciseForUserDTO(e, userExercises)).ToList();
            IsCompleted = Exercises.All(e => e.isSubmitted);
        }
    }
}
