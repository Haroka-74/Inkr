using Inkr.Models;
using System.IdentityModel.Tokens.Jwt;

namespace Inkr.Services.Interfaces
{
    public interface ITokenProviderService
    {
        public string GetRefreshToken();
        public Task<JwtSecurityToken> GetAccessTokenAsync(InkrUser user);
    }
}