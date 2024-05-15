using System.IdentityModel.Tokens.Jwt;

namespace ZeraMessanger.Services.Authentification.Jwt
{
    public class JwtDecoder : IJwtDecoder
    {
        public string GetClaimValue(string claimType, HttpRequest request)
        {
            JwtSecurityToken Jwt = new JwtSecurityTokenHandler().ReadJwtToken(
               request
               .Headers
               .Authorization
               .ToString()
               .Replace("Bearer ", string.Empty));

            return Jwt.Claims.FirstOrDefault(claim => claim.Type == claimType)!.Value;
        }
    }
}
