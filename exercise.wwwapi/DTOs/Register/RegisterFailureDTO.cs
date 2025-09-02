namespace exercise.wwwapi.DTOs.Register
{
    public class RegisterFailureDTO
    {
        public List<string> EmailErrors { get; set; } = [];
        public List<string> PasswordErrors { get; set; } = [];
    }
}
