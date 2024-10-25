using Google.Apis.Auth.OAuth2;
using Google.Cloud.Dialogflow.V2;
using Google.Cloud.Translation.V2;
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
        //private readonly TranslationClient _translationClient;

        public DialogflowService(IConfiguration configuration)
        {
            // Lấy thông tin từ cấu hình
            //_projectId = configuration["Dialogflow:ProjectId"];
            //var clientEmail = configuration["Dialogflow:ClientEmail"];
            //var privateKey = configuration["Dialogflow:PrivateKey"];

            //// Tạo credentials cho Dialogflow
            //var credential = GoogleCredential.FromJson($"{{\"type\":\"service_account\",\"client_email\":\"{clientEmail}\",\"private_key\":\"{privateKey}\"}}");
            //var scopedCredential = credential.CreateScoped(SessionsClient.DefaultScopes);

            //_sessionsClient = new SessionsClientBuilder
            //{
            //    ChannelCredentials = scopedCredential.ToChannelCredentials()
            //}.Build();


            //_projectId = configuration["Dialogflow:ProjectId"];

            //// Lấy đường dẫn file JSON từ appsettings.json
            //var credentialsFilePath = configuration["Dialogflow:CredentialsFilePath"];

            //// Combine đường dẫn tương đối với thư mục gốc của dự án
            //var fullPath = Path.Combine(Directory.GetCurrentDirectory(), credentialsFilePath);

            //// Đọc credentials từ file JSON
            //var credential = GoogleCredential.FromFile(fullPath);
            //var scopedCredential = credential.CreateScoped(SessionsClient.DefaultScopes);

            //_sessionsClient = new SessionsClientBuilder
            //{
            //    ChannelCredentials = scopedCredential.ToChannelCredentials()
            //}.Build();



            _projectId = configuration["Dialogflow:ProjectId"];

            // Khởi tạo TranslationClient
            //_translationClient = TranslationClient.Create();

            // Đường dẫn tới file JSON chứa credentials
            var credentialsFilePath = configuration["Dialogflow:CredentialsFilePath"];
            var credential = GoogleCredential.FromFile(credentialsFilePath);

            var scopedCredential = credential.CreateScoped(SessionsClient.DefaultScopes);

            // Khởi tạo SessionsClient
            _sessionsClient = new SessionsClientBuilder
            {
                ChannelCredentials = credential.ToChannelCredentials()
            }.Build(); // Đảm bảo rằng bạn có dấu ngoặc () ở đây
        }

        public async Task<string> DetectIntentAsync(string sessionId, string text)
        {

            // Dịch câu hỏi từ tiếng Việt sang tiếng Anh
            //var translatedText = await TranslateToEnglish(text);


            var sessionName = new SessionName(_projectId, sessionId);
            var queryInput = new QueryInput
            {
                Text = new TextInput
                {
                    Text = text,
                    LanguageCode = "en" // Ngôn ngữ có thể được thay đổi
                }
            };

            var response = await _sessionsClient.DetectIntentAsync(sessionName, queryInput);

            // Dịch kết quả từ tiếng Anh sang tiếng Việt (nếu cần)
            //var translatedResponse = await TranslateToVietnamese(response.QueryResult.FulfillmentText);

            return response.QueryResult.FulfillmentText;
        }

        //private async Task<string> TranslateToEnglish(string text)
        //{
        //    // Dùng Google Cloud Translation API để dịch từ tiếng Việt sang tiếng Anh
        //    var response = await _translationClient.TranslateTextAsync(text, "en", "vi");
        //    return response.TranslatedText;
        //}

        //private async Task<string> TranslateToVietnamese(string text)
        //{
        //    // Dùng Google Cloud Translation API để dịch từ tiếng Anh sang tiếng Việt
        //    var response = await _translationClient.TranslateTextAsync(text, "vi", "en");
        //    return response.TranslatedText;
        //}

    }

    
}
