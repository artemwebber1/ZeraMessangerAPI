namespace SoftworkMessanger.Models.Dto.ChatDto
{
    /// <summary>
    /// Данные, нужные для создания нового чата.
    /// </summary>
    /// <param name="ChatName">Имя нового чата.</param>
    /// <param name="ChatCreatorId">Id создателя чата.</param>
    public record NewChatData(
        string ChatName,
        int ChatCreatorId);
}
