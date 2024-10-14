using Google.Apis.Auth.OAuth2;
using Google.Cloud.Dialogflow.V2;
using Grpc.Auth;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Channels;
using WebApplication1.DTOs.Request;

namespace WebApplication1.Services.Impl
{
    public class DialogflowService : IDialogflowService
    {
        private readonly SessionsClient _sessionsClient;
        private readonly string _projectId;

        public DialogflowService(IConfiguration configuration)
        {
            // Lấy thông tin từ cấu hình
            _projectId = configuration["Dialogflow:ProjectId"];
            var clientEmail = configuration["Dialogflow:ClientEmail"];
            var privateKey = configuration["Dialogflow:PrivateKey"];

            // Tạo credentials cho Dialogflow
            var credential = GoogleCredential.FromJson($"{{\"type\":\"service_account\",\"client_email\":\"{clientEmail}\",\"private_key\":\"{privateKey}\"}}");
            var scopedCredential = credential.CreateScoped(SessionsClient.DefaultScopes);

            _sessionsClient = new SessionsClientBuilder
            {
                ChannelCredentials = scopedCredential.ToChannelCredentials()
            }.Build();
        }

        public async Task<string> DetectIntentAsync(string sessionId, string text)
        {
            var sessionName = new SessionName(_projectId, sessionId);
            var queryInput = new QueryInput
            {
                Text = new TextInput
                {
                    Text = text,
                    LanguageCode = "vi" // Ngôn ngữ có thể được thay đổi
                }
            };

            var response = await _sessionsClient.DetectIntentAsync(sessionName, queryInput);
            return response.QueryResult.FulfillmentText;
        }
    }
}
