namespace exercise.wwwapi.DTOs.UpdateUser
{
    public class UpdateUserRequestDTO
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Bio { get; set; }
        public string? GithubName { get; set; }
        public string? Username { get; set; }
        public string? MobileNumber { get; set; }
    }
}
