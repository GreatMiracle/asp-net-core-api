
using Newtonsoft.Json;
using System.Text;

namespace WebApplication1.Services.Impl
{
    public class GChatService : IGChatService
    {
        private readonly IConfiguration _configuration;

        public GChatService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendMessageAsync(string message)
        {
            // Lấy giá trị URL từ appsettings.json
            var webhookUrl = _configuration["GChatWebhook:Url"];

            // Kiểm tra nếu URL không tồn tại
            if (string.IsNullOrEmpty(webhookUrl))
            {
                throw new InvalidOperationException("Webhook URL is not configured.");
            }
            using var httpClient = new HttpClient();
            var payload = new { text = message }; // Payload cần có định dạng JSON với thuộc tính 'text'
            var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
            await httpClient.PostAsync(webhookUrl, content);
        }
    }
}
