using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace WebApplication1.Middleware
{
    public class JiraService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        private readonly string _username;
        private readonly string _apiToken;

        public JiraService(HttpClient httpClient, string baseUrl, string username, string apiToken)
        {
            _httpClient = httpClient;
            _baseUrl = baseUrl;
            _username = username;
            _apiToken = apiToken;

            // Thiết lập thông tin xác thực cho HttpClient
            var byteArray = Encoding.ASCII.GetBytes($"{_username}:{_apiToken}");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
        }

        // Phương thức để tạo một vấn đề mới
        public async Task<string> CreateIssueAsync(string projectKey, string summary, string description)
        {
            var issue = new
            {
                fields = new
                {
                    project = new { key = projectKey },
                    summary,
                    description,
                    issuetype = new { name = "Task" }
                }
            };

            var content = new StringContent(JsonSerializer.Serialize(issue), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_baseUrl}/rest/api/3/issue", content);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();
            return result;
        }

        // Phương thức để lấy thông tin tất cả các vấn đề trong một dự án
        public async Task<string> GetIssuesAsync(string projectKey)
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/rest/api/3/search?jql=project={projectKey}");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        // Phương thức để cập nhật một vấn đề
        public async Task<string> UpdateIssueAsync(string issueId, string newSummary)
        {
            var issueUpdate = new
            {
                fields = new
                {
                    summary = newSummary
                }
            };

            var content = new StringContent(JsonSerializer.Serialize(issueUpdate), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"{_baseUrl}/rest/api/3/issue/{issueId}", content);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
    }
}
