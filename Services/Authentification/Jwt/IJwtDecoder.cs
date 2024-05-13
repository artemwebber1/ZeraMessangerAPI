namespace ZeraMessanger.Services.Authentification.Jwt
{
    public interface IJwtDecoder
    {
        string GetClaimValue(string claimType, HttpRequest request);
    }
}
