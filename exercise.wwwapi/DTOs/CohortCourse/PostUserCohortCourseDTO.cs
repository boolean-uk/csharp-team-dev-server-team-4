using exercise.wwwapi.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.DTOs.CohortCourse
{
    public class PostUserCohortCourseDTO
    {
        public int CohortId { get; set; }
        public int CourseId { get; set; }
    }
}
