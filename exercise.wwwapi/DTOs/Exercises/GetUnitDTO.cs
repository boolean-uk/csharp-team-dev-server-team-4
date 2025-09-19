using exercise.wwwapi.Models;
using exercise.wwwapi.Models.Exercises;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.DTOs.Exercises
{
    public class GetUnitDTO
    {
        public int Id { get; set; }

        public int ModuleId { get; set; }

        public string Name { get; set; }
        public ICollection<Exercise_noUnit> Exercises { get; set; } = new List<Exercise_noUnit>();

        public GetUnitDTO(){}

        public GetUnitDTO(Unit model)
        {
            Id = model.Id;
            ModuleId = model.ModuleId;
            Name = model.Name;
            Exercises = model.Exercises.Select(e => new Exercise_noUnit(e)).ToList();
        }
    }
}
