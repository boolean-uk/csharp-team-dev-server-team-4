using exercise.wwwapi.DTOs.Exercises;
using exercise.wwwapi.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.DTOs.Courses
{
    public class GetCourseModuleDTO
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public int ModuleId { get; set; }
        public GetModuleDTO Module { get; set; }

        public GetCourseModuleDTO(){}
        public GetCourseModuleDTO(CourseModule model)
        {
            Id = model.Id;
            CourseId = model.CourseId;
            ModuleId = model.ModuleId;
            Module = new GetModuleDTO(model.Module);
        }
    }
}
