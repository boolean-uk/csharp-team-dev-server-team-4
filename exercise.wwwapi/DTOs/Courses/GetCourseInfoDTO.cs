using exercise.wwwapi.Models;

namespace exercise.wwwapi.DTOs.Courses
{
    public class GetCourseInfoDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public GetCourseInfoDTO(){}

        public GetCourseInfoDTO(Course model)
        {
            Id = model.Id;
            Name = model.Name;
        }
    }
}
