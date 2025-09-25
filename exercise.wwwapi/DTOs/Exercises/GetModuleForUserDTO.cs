using exercise.wwwapi.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.DTOs.Exercises
{
    public class GetModuleForUserDTO
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public bool IsCompleted { get; set; }

        public ICollection<GetUnitForUserDTO> Units { get; set; } = new List<GetUnitForUserDTO>();

        public GetModuleForUserDTO()
        {
            
        }

        public GetModuleForUserDTO(Module model, ICollection<UserExercise> userExercises)
        {
            Id = model.Id;
            Title = model.Title;
            Units = model.Units.Select(u => new GetUnitForUserDTO(u, userExercises)).ToList();
            IsCompleted = Units.All(u => u.IsCompleted);
        }
        
    }
}
