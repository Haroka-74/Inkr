namespace Inkr.DTOs.Auth
{
    public class LoginResponseDTO
    {
        public string Email { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
        public string AccessToken { get; set; } = null!;
        public DateTime RefreshTokenExpiration { get; set; }
    }
}