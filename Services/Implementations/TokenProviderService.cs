using Inkr.Models;
using Inkr.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Inkr.Services.Implementations
{
    public class TokenProviderService(UserManager<InkrUser> userManager) : ITokenProviderService
    {
        public string GetRefreshToken() => Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

        public async Task<JwtSecurityToken> GetAccessTokenAsync(InkrUser user)
        {
            var claims = await userManager.GetClaimsAsync(user);
            var roles = await userManager.GetRolesAsync(user);

            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
            claims.Add(new Claim(ClaimTypes.Name, user.UserName!));
            claims.Add(new Claim(ClaimTypes.Email, user.Email!));
            claims.Add(new Claim("profile_picture", user.ProfilePicture ?? ""));
            claims.Add(new Claim("bio", user.Bio ?? ""));

            foreach (var role in roles)
                claims.Add(new Claim(ClaimTypes.Role, role));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppSettings.JWT.SecretKey));
            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var accessToken = new JwtSecurityToken(
                issuer: AppSettings.JWT.Issuer,
                audience: AppSettings.JWT.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(AppSettings.JWT.ExpiryMinutes)),
                signingCredentials: signingCredentials
            );

            return accessToken;
        }
    }
}