using System.Net.Http.Headers;
using WebApplication1.Middleware;
using WebApplication1.Services;
using WebApplication1.Services.Impl;

namespace WebApplication1.Core.Configs
{
    public static class ClientConfig
    {
        public static void CallJiraServices(IServiceCollection services)
        {
            services.AddHttpClient<IThirdPartyApiService, ThirdPartyApiServiceImpl>(client =>
            {
                client.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });

            // Cấu hình cho REST Countries API
            services.AddHttpClient<ICountryService, CountryServiceImpl>(client =>
            {
                client.BaseAddress = new Uri("https://restcountries.com/v3.1/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });

            services.AddHttpClient<JiraService>(client =>
            {
                // Thay đổi các thông số theo nhu cầu của bạn
                client.BaseAddress = new Uri("https://your-jira-instance.atlassian.net");
            });

            services.AddSingleton(sp =>
            {
                var httpClient = sp.GetRequiredService<HttpClient>();
                return new JiraService(httpClient, "https://your-jira-instance.atlassian.net", "your-email@example.com", "your-api-token");
            });
        }
    }
}
