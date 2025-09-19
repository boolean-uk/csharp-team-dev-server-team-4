using exercise.wwwapi.Models;

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
    }
}