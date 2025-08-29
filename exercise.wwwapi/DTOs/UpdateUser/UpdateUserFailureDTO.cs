namespace exercise.wwwapi.DTOs.UpdateUser
{
    public class UpdateUserFailureDTO
    {
        public List<string> EmailErrors { get; set; } = new();
        public List<string> PasswordErrors { get; set; } = new();

        public List<string> MobileNumberErrors { get; set; } = new();
    }
}
