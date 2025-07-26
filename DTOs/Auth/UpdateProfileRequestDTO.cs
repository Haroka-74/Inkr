namespace Inkr.DTOs.Auth
{
    public class UpdateProfileRequestDTO
    {
        public string UserId { get; set; } = string.Empty;
        public string? Username { get; set; }
        public string? ProfilePicture { get; set; }
        public string? Bio { get; set; }
    }
}