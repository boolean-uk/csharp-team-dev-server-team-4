using exercise.wwwapi.DTOs.Notes;
using exercise.wwwapi.DTOs.Users;
using exercise.wwwapi.Enums;
using exercise.wwwapi.Models;
using System.Numerics;

namespace exercise.wwwapi.Factories
{
    public static class UserFactory
    {
        public static UserDTO GetUserDTO(User user, PrivilegeLevel privilegeLevel)
        {
            var userDTO = new UserDTO(user);

            if (privilegeLevel == PrivilegeLevel.Teacher) 
            {
                userDTO.Notes = user.Notes.Select(note => NoteFactory.GetNoteDTO(note)).ToList();  
            }

            return userDTO;
        }
    }
}
