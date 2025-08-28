namespace exercise.wwwapi.DTOs
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Bio { get; set; }
        public string? GithubUrl { get; set; }
        public string? Username { get; set; }

        public string? MobileNumber { get; set; }
    }
}
