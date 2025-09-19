using exercise.wwwapi.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.DTOs.Courses
{
    public class GetCourseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<GetCourseModuleDTO> CourseModules { get; set; } = new List<GetCourseModuleDTO>();
        public GetCourseDTO(){}
        public GetCourseDTO(Course model)
        {
            Id = model.Id;
            Name = model.Name;
            CourseModules = model.CourseModules.Select(cm => new GetCourseModuleDTO(cm)).ToList();
        }
    }
}
