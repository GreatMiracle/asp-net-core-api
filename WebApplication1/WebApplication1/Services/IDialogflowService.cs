using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTOs.Request;

namespace WebApplication1.Services
{
    public interface IDialogflowService
    {
        Task<string> DetectIntentAsync(string responseId, string queryText);
    }
}
