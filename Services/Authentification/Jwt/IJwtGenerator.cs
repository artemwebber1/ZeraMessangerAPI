﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ZeraMessanger.Services.Authentification.Jwt
{
    public interface IJwtGenerator
    {
        JwtSecurityToken GenerateToken(IEnumerable<Claim> claims);
    }
}
