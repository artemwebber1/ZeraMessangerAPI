namespace SoftworkMessanger.Models.Dto.MessageDto
{
    /// <summary>
    /// Данные нового сообщения в чате.
    /// </summary>
    /// <param name="ChatId">Id чата, в который будет отправлено сообщение.</param>
    /// <param name="MessageText">Текст сообщения.</param>
    public record NewMessageData(
        int ChatId,
        string MessageText);
}
