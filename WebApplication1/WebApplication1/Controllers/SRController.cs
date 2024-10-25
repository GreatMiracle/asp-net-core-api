using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTOs.Request;
using WebApplication1.DTOs.Response;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [Route("api/sr")]
    [ApiController]
    public class SRController: ControllerBase
    {
        private readonly IGChatService _gChatService;
        private readonly IEmailService _emailService;

        public SRController(IGChatService gChatService, IEmailService emailService)
        {
            _gChatService = gChatService;
            _emailService = emailService;
        }

        [HttpPost("send/ggchat")]
        public async Task<IActionResult> SendGgChat(int requestId)
        {
            // Thực hiện logic hủy yêu cầu SR tại đây
            // Sau khi hủy, gửi thông báo qua GChat
            await _gChatService.SendMessageAsync($"Request ID {requestId} has been terminated.");

            return Ok("Request terminated and notification sent.");
        }

        [HttpPost("send/email")]
        public async Task<IActionResult> SendEmail(string toEmail)
        {
            // Logic hủy yêu cầu SR
            // Sau khi hủy, gửi email thông báo
            await _emailService.SendEmailAsync(toEmail, "Request Termination", "Your SR has been terminated.");
            return Ok();
        }


    }
}
