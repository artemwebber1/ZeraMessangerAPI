using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SoftworkMessanger.Services.Authentification.Jwt
{
    public interface IJwtGenerator
    {
        JwtSecurityToken GenerateToken(IEnumerable<Claim> claims);
    }
}
