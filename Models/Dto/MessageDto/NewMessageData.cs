namespace SoftworkMessanger.Models.Dto.MessageDto
{
    /// <summary>
    /// Данные нового сообщения в чате.
    /// </summary>
    /// <param name="AuthorId">Id автора сообщения.</param>
    /// <param name="ChatId">Id чата, в котором находится сообщение.</param>
    /// <param name="MessageText">Текст сообщения.</param>
    public record NewMessageData(
        int AuthorId,
        int ChatId,
        string MessageText);
}
