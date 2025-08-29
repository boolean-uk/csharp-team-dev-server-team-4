namespace exercise.wwwapi.DTOs.Register
{
    public class RegisterFailureDTO
    {
        public List<string> EmailErrors { get; set; } = new();
        public List<string> PasswordErrors { get; set; } = new();
    }
}
