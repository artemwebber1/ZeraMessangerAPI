namespace ZeraMessanger.Models.Dto.ChatDto
{
    /// <summary>
    /// Поверхностная информация о чате, которую видет пользователь в списке его чатов 
    /// (название чата, количество участников и т. п.).
    /// </summary>
    /// <param name="ChatId">Id чата.</param>
    /// <param name="ChatName">Имя чата.</param>
    /// <param name="MembersCount">Количество участников чата.</param>
    public record ChatFirstView(
        int ChatId,
        string ChatName,
        int MembersCount);
}
