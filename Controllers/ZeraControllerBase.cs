using Microsoft.AspNetCore.Mvc;
using ZeraMessanger.Services.Authentification.Jwt;

namespace ZeraMessanger.Controllers
{
    public abstract class ZeraControllerBase : ControllerBase
    {
        protected ZeraControllerBase(IJwtDecoder jwtDecoder)
        {
            _jwtDecoder = jwtDecoder;
        }

        private readonly IJwtDecoder _jwtDecoder;

        /// <summary>
        /// Id авторизированного пользователя.
        /// </summary>
        protected int IdentityId => int.Parse(_jwtDecoder.GetClaimValue("UserId", Request));
    }
}
