using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ZeraMessanger.Services.Authentification.Jwt
{
    public class JwtGenerator : IJwtGenerator
    {
        public JwtGenerator()
        {
            _jwtOptions = new JwtOptions();
        }

        private readonly JwtOptions _jwtOptions;

        public JwtSecurityToken GenerateToken(IEnumerable<Claim> claims)
        {
            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
                claims: claims,
                signingCredentials: _jwtOptions.SigningCredentials);

            return jwtSecurityToken;
        }
    }
}
