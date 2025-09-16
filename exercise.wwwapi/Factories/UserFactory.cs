using exercise.wwwapi.DTOs;
using exercise.wwwapi.DTOs.Notes;
using exercise.wwwapi.Enums;
using exercise.wwwapi.Models;
using exercise.wwwapi.Models.UserInfo;
using System.Numerics;

namespace exercise.wwwapi.Factories
{
    public static class UserFactory
    {
        public static UserDTO GetUserDTO(User user, PrivilegeLevel privilegeLevel)
        {
            var userDTO = new UserDTO()
            {
                Id = user.Id,
                FirstName = user.Profile.FirstName,
                LastName = user.Profile.LastName,
                Bio = user.Profile.Bio,
                Github = user.Profile.Github,
                Username = user.Credential.Username,
                Email = user.Credential.Email,
                Phone = user.Profile.Phone,
                StartDate = user.Profile.StartDate,
                EndDate = user.Profile.EndDate,
                Specialism = user.Profile.Specialism,
                CohortId = user.CohortId,
                Role = user.Credential.Role.ToString()
            };

            if (privilegeLevel == PrivilegeLevel.Teacher) 
            {
                userDTO.Notes = user.Notes.Select(note => NoteFactory.GetNoteDTO(note)).ToList();  
            }

            return userDTO;
        }
    }
}
