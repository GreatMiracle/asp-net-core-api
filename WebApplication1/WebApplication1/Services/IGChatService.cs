namespace WebApplication1.Services
{
    public interface IGChatService
    {
        Task SendMessageAsync(string message);
    }
}
