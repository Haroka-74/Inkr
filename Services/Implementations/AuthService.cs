using Inkr.DTOs.Auth;
using Inkr.Models;
using Inkr.Services.Interfaces;
using Inkr.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Inkr.Services.Implementations
{
    public class AuthService(UserManager<InkrUser> userManager, RoleManager<IdentityRole> roleManager, ITokenProviderService tokenProviderService, IAuthNotificationService authNotificationService, IEmailTemplateService emailTemplateService) : IAuthService
    {
        public async Task<Result<RegisterResponseDTO>> RegisterAsync(RegisterRequestDTO request)
        {
            if (await userManager.FindByNameAsync(request.Username) is not null)
                return Result<RegisterResponseDTO>.Failure("Username is already registered", 409);

            if (await userManager.FindByEmailAsync(request.Email) is not null)
                return Result<RegisterResponseDTO>.Failure("Email is already registered", 409);

            var user = new InkrUser()
            {
                UserName = request.Username,
                Email = request.Email,
            };

            var result = await userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
                return Result<RegisterResponseDTO>.Failure(string.Join(" | ", result.Errors.Select(e => e.Description)), 400);

            if (!await roleManager.RoleExistsAsync("author"))
                await roleManager.CreateAsync(new IdentityRole("author"));

            if(request.IsAuthor)
                await userManager.AddToRoleAsync(user, "author");

            var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodedToken = Uri.EscapeDataString(token);

            var verificationLink = $"{AppSettings.Backend.URL}/api/auth/confirm-email?userId={user.Id}&token={encodedToken}";
            await authNotificationService.SendEmailConfirmationAsync(user, verificationLink);

            return Result<RegisterResponseDTO>.Success(new RegisterResponseDTO
            {
                Id = user.Id,
                Username = user.UserName,
                Email = user.Email,
            }, 201);
        }

        public async Task<Result<LoginResponseDTO>> LoginAsync(LoginRequestDTO request)
        {
            var user = await userManager.Users.Include(u => u.RefreshTokens).FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user is null || !await userManager.CheckPasswordAsync(user, request.Password))
                return Result<LoginResponseDTO>.Failure("Email or Password is incorrect!", 401);

            if (!await userManager.IsEmailConfirmedAsync(user))
                return Result<LoginResponseDTO>.Failure("Please confirm your email", 401);

            var accessToken = await tokenProviderService.GetAccessTokenAsync(user);
            var refreshToken = user.RefreshTokens.FirstOrDefault(rt => !rt.IsRevoked && DateTime.Now < rt.ExpiresAt);
            
            var tokensToRemove = user.RefreshTokens.Where(rt => rt.IsRevoked || DateTime.Now >= rt.ExpiresAt).ToList();

            foreach (var token in tokensToRemove)
                user.RefreshTokens.Remove(token);

            if (refreshToken is null)
            {
                refreshToken = new RefreshToken
                {
                    Token = tokenProviderService.GetRefreshToken(),
                    CreatedAt = DateTime.Now,
                    ExpiresAt = DateTime.Now.AddDays(Convert.ToDouble(AppSettings.RefreshToken.ExpiryDays)),
                    IsRevoked = false,
                    UserId = user.Id
                };

                user.RefreshTokens.Add(refreshToken);
            }

            var result = await userManager.UpdateAsync(user);

            if (!result.Succeeded)
                return Result<LoginResponseDTO>.Failure(string.Join(" | ", result.Errors.Select(e => e.Description)), 500);

            return Result<LoginResponseDTO>.Success(new LoginResponseDTO
            {
                Email = request.Email,
                AccessToken = new JwtSecurityTokenHandler().WriteToken(accessToken),
                RefreshToken = refreshToken.Token,
                RefreshTokenExpiration = refreshToken.ExpiresAt
            }, 200);
        }

        public async Task<Result<LoginResponseDTO>> RefreshTokenAsync(RefreshTokenDTO request)
        {
            var user = await userManager.Users.Include(u => u.RefreshTokens).SingleOrDefaultAsync(u => u.RefreshTokens.Any(r => r.Token == request.Token));

            if (user is null)
                return Result<LoginResponseDTO>.Failure("Invalid token", 401);

            var refreshToken = user.RefreshTokens.Single(r => r.Token == request.Token);

            if (refreshToken.IsRevoked || DateTime.Now >= refreshToken.ExpiresAt)
                return Result<LoginResponseDTO>.Failure("Inactive token", 401);

            refreshToken.IsRevoked = true;

            var tokensToRemove = user.RefreshTokens.Where(rt => rt.IsRevoked || DateTime.Now >= rt.ExpiresAt).ToList();

            foreach (var token in tokensToRemove)
                user.RefreshTokens.Remove(token);

            var newRefreshToken = new RefreshToken
            {
                Token = tokenProviderService.GetRefreshToken(),
                CreatedAt = DateTime.Now,
                ExpiresAt = DateTime.Now.AddDays(Convert.ToDouble(AppSettings.RefreshToken.ExpiryDays)),
                IsRevoked = false,
                UserId = user.Id
            };

            user.RefreshTokens.Add(newRefreshToken);
            await userManager.UpdateAsync(user);

            var newAccessToken = await tokenProviderService.GetAccessTokenAsync(user);

            return Result<LoginResponseDTO>.Success(new LoginResponseDTO
            {
                Email = user.Email!,
                AccessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
                RefreshToken = newRefreshToken.Token,
                RefreshTokenExpiration = newRefreshToken.ExpiresAt
            }, 200);
        }

        public async Task RevokeTokenAsync(RefreshTokenDTO request)
        {
            var user = await userManager.Users.Include(u => u.RefreshTokens).SingleOrDefaultAsync(u => u.RefreshTokens.Any(r => r.Token == request.Token));

            if (user is null)
                return;

            var refreshToken = user.RefreshTokens.Single(r => r.Token == request.Token);

            if (refreshToken.IsRevoked || DateTime.Now >= refreshToken.ExpiresAt)
                return;

            refreshToken.IsRevoked = true;
            await userManager.UpdateAsync(user);
        }

        public Result<GetProfileRequestDTO> GetProfileAsync(string accessToken)
        {
            var handler = new JwtSecurityTokenHandler();

            if (!handler.CanReadToken(accessToken))
                return Result<GetProfileRequestDTO>.Failure("Access token is not readable or incorrectly structured", 400);

            var jwtToken = handler.ReadJwtToken(accessToken);

            var claims = jwtToken.Claims;

            var userId = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var username = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var profilePicture = claims.FirstOrDefault(c => c.Type == "profile_picture")?.Value ?? "";
            var bio = claims.FirstOrDefault(c => c.Type == "bio")?.Value ?? "";

            if (userId is null || username is null || email is null)
                return Result<GetProfileRequestDTO>.Failure("Required claims are missing in the token", 400);

            return Result<GetProfileRequestDTO>.Success(new GetProfileRequestDTO
            {
                Id = userId,
                Username = username,
                Email = email,
                ProfilePicture = profilePicture,
                Bio = bio,
            }, 200);
        }

        public async Task<Result<GetProfileRequestDTO>> UpdateProfileAsync(UpdateProfileRequestDTO request)
        {
            var user = await userManager.FindByIdAsync(request.UserId);

            if (user is null)
                return Result<GetProfileRequestDTO>.Failure("User not found", 404);

            if (!string.IsNullOrWhiteSpace(request.Username) && request.Username != user.UserName)
            {
                if (await userManager.FindByNameAsync(request.Username) is not null)
                    return Result<GetProfileRequestDTO>.Failure("Username is already taken", 409);

                user.UserName = request.Username;
            }

            if (request.Bio is not null)
                user.Bio = request.Bio;

            if (request.ProfilePicture is not null)
                user.ProfilePicture = request.ProfilePicture;

            var result = await userManager.UpdateAsync(user);

            if (!result.Succeeded)
                return Result<GetProfileRequestDTO>.Failure(string.Join(" | ", result.Errors.Select(e => e.Description)), 400);

            return Result<GetProfileRequestDTO>.Success(new GetProfileRequestDTO
            {
                Id = user.Id,
                Username = user.UserName!,
                Email = user.Email!,
                ProfilePicture = user.ProfilePicture ?? "",
                Bio = user.Bio ?? ""
            }, 200);
        }

        public async Task<Result<string>> ConfirmEmailAsync(string userId, string token)
        {
            var user = await userManager.FindByIdAsync(userId);

            if (user is null)
                return Result<string>.Failure("User not found", 404);

            if (await userManager.IsEmailConfirmedAsync(user))
                return Result<string>.Failure("Email is already confirmed", 400);

            var decodedToken = Uri.UnescapeDataString(token);
            var result = await userManager.ConfirmEmailAsync(user, decodedToken);

            if (!result.Succeeded)
                return Result<string>.Failure("Email confirmation failed", 400);

            return Result<string>.Success(emailTemplateService.GenerateEmailConfirmationHtmlPage(), 200);
        }

        public async Task ResendEmailConfirmationAsync(ResendEmailConfirmationDTO request)
        {
            var user = await userManager.FindByEmailAsync(request.Email);

            if (user is null || await userManager.IsEmailConfirmedAsync(user))
                return;

            var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodedToken = Uri.EscapeDataString(token);

            var verificationLink = $"{AppSettings.Backend.URL}/api/auth/confirm-email?userId={user.Id}&token={encodedToken}";

            await authNotificationService.SendEmailConfirmationAsync(user, verificationLink);
        }

        public async Task ForgotPasswordAsync(ForgotPasswordDTO request)
        {
            var user = await userManager.FindByEmailAsync(request.Email);

            if (user is null || !await userManager.IsEmailConfirmedAsync(user))
                return;

            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = Uri.EscapeDataString(token);

            var resetLink = $"{AppSettings.Backend.URL}/api/auth/reset-password-form?userId={user.Id}&token={encodedToken}";

            await authNotificationService.SendPasswordResetAsync(user, resetLink);
        }

        public async Task<Result<string>> ResetPasswordAsync(ResetPasswordDTO request)
        {
            var user = await userManager.Users.Include(u => u.RefreshTokens).FirstOrDefaultAsync(u => u.Id == request.UserId);

            if (user is null)
                return Result<string>.Failure("User not found", 404);

            var decodedToken = Uri.UnescapeDataString(request.Token);
            var result = await userManager.ResetPasswordAsync(user, decodedToken, request.NewPassword);

            if (!result.Succeeded)
                return Result<string>.Failure(string.Join(" | ", result.Errors.Select(e => e.Description)), 400);
            
            if (user.RefreshTokens.Count != 0)
            {
                foreach (var token in user.RefreshTokens)
                    token.IsRevoked = true;

                await userManager.UpdateAsync(user);
            }

            return Result<string>.Success("Password reset successfully", 200);
        }
    }
}