namespace exercise.wwwapi.DTOs.UpdateUser
{
    public class UpdateUserSuccessDTO
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Bio { get; set; }
        public string? Github { get; set; }
        public string? Phone { get; set; }
    }
}
