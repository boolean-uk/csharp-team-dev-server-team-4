using exercise.wwwapi.DTOs.Users;
using exercise.wwwapi.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.DTOs.CohortCourse;

    public class GetCohortCourseDTO
    {
        public int Id { get; set; }
        public int CohortId { get; set; }
        public int CourseId { get; set; }

        public string CohortName { get; set; }
        public string CourseName { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public ICollection<UserDTO> Users { get; set; } = new List<UserDTO>();

        public GetCohortCourseDTO() { }
        public GetCohortCourseDTO(Models.CohortCourse model)
        {
            Id = model.Id;
            CohortId = model.CohortId;
            CourseId = model.CourseId;
            CohortName = model.Cohort.CohortName;
            CourseName = model.Course.Name;
            StartDate = $"{model.Cohort.StartDate:MMMM} {model.Cohort.StartDate:yyyy}";
            EndDate = $"{model.Cohort.EndDate:MMMM} {model.Cohort.EndDate:yyyy}";
            Users = model.UserCCs.Select(uc => new UserDTO(uc.User)).ToList();


    }
    }

