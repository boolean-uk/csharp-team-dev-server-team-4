using exercise.wwwapi.DTOs.Users;
using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.DTOs.Register;

[NotMapped]
public class RegisterSuccessDTO
{
    public UserDTO User { get; } = new();
}