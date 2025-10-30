namespace Business.Mensajeria.Interfaces
{
    public interface IServiceTelegram
    {
        Task SendMessageAsync(string chatId, string message);
    }
}
