﻿namespace SoftworkMessanger.Models.Dto
{
    /// <summary>
    /// Поверхностная информация о чате, которую видет пользователь в списке его чатов 
    /// (название чата, последнее сообщение, иконка).
    /// </summary>
    /// <param name="ChatId">Id чата.</param>
    /// <param name="ChatName">Имя чата.</param>
    /// <param name="LastMessageText">Последнее сообщение в чате.</param>
    public record ChatFirstView(
        int ChatId,
        string ChatName,
        string LastMessageText);
}
