namespace Inkr.DTOs.Auth
{
    public class GetProfileRequestDTO
    {
        public string Id { get; set; } = default!;
        public string Username { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string? ProfilePicture { get; set; }
        public string? Bio { get; set; }
    }
}