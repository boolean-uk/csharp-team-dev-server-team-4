using exercise.wwwapi.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.DTOs.Exercises
{
    public class GetModuleDTO
    {
        public int Id { get; set; }
     
        public string Title { get; set; }
        public ICollection<GetUnitDTO> Units { get; set; } = new List<GetUnitDTO>();

        public GetModuleDTO(){}

        public GetModuleDTO(Module model)
        {
            Id = model.Id;
            Title = model.Title;
            Units = model.Units.Select(u => new GetUnitDTO(u)).ToList();
        }
    }
}
