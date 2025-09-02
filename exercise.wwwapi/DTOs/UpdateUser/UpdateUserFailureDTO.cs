namespace exercise.wwwapi.DTOs.UpdateUser
{
    public class UpdateUserFailureDTO
    {
        public List<string> EmailErrors { get; set; } = [];
        public List<string> PasswordErrors { get; set; } = [];
        public List<string> MobileNumberErrors { get; set; } = [];
    }
}
