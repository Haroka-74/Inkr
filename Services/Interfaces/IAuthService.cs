using Inkr.DTOs.Auth;
using Inkr.Shared;

namespace Inkr.Services.Interfaces
{
    public interface IAuthService
    {
        Task<Result<RegisterResponseDTO>> RegisterAsync(RegisterRequestDTO request);
        Task<Result<LoginResponseDTO>> LoginAsync(LoginRequestDTO request);
        Task<Result<LoginResponseDTO>> RefreshTokenAsync(RefreshTokenDTO request);
        Task RevokeTokenAsync(RefreshTokenDTO request);
        Result<GetProfileRequestDTO> GetProfileAsync(string accessToken);
        Task<Result<GetProfileRequestDTO>> UpdateProfileAsync(UpdateProfileRequestDTO request);
        Task<Result<string>> ConfirmEmailAsync(string userId, string token);
        Task ResendEmailConfirmationAsync(ResendEmailConfirmationDTO request);
        Task ForgotPasswordAsync(ForgotPasswordDTO request);
        Task<Result<string>> ResetPasswordAsync(ResetPasswordDTO request);
    }
}