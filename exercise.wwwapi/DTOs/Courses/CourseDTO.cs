using exercise.wwwapi.Models;
using System.Drawing;

namespace exercise.wwwapi.DTOs.Courses
{
    public class CourseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public CourseDTO() { }
        public CourseDTO(Course model)
        {
            Id = model.Id;
            Name = model.Name;
        }
        public CourseDTO(Models.CohortCourse model)
        {
            Id = model.Course.Id;
            Name = model.Course.Name;
        }
    }
    
}