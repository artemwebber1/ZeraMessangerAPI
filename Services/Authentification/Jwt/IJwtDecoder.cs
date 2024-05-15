namespace ZeraMessanger.Services.Authentification.Jwt
{
    /// <summary>
    /// Декодировщик JWT-токенов.
    /// </summary>
    public interface IJwtDecoder
    {
        /// <summary>
        /// Получение значения клэима с указанным типом <paramref name="claimType"/> из JWT-токена.
        /// </summary>
        /// <param name="claimType">Тип клэима, значение которого нужно вернуть.</param>
        /// <param name="request">Объект класса <see cref="HttpRequest"/>, из которого будет получен JWT-токен.</param>
        /// <returns>Значение клэима в виде строки.</returns>
        string GetClaimValue(string claimType, HttpRequest request);
    }
}
