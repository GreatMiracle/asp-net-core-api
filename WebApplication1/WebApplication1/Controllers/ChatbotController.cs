using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTOs.Request;
using WebApplication1.DTOs.Response;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [Route("api/chatbot")]
    [ApiController]
    public class ChatbotController: ControllerBase
    {

        private readonly IDialogflowService _dialogflowService;

        public ChatbotController(IDialogflowService dialogflowService)
        {
            _dialogflowService = dialogflowService;
        }

        [HttpPost]
        public async Task<IActionResult> HandleChatbotRequest([FromBody] ChatRequest request)
        {
            if (string.IsNullOrEmpty(request.Question))
            {
                return BadRequest("Question cannot be empty.");
            }
            var sessionId = Guid.NewGuid().ToString();  // Tạo một session ID ngẫu nhiên cho mỗi câu hỏi
            var response = await _dialogflowService.DetectIntentAsync(sessionId, request.Question);
            return Ok(new { Answer = response });

            //var responseText = await _dialogflowService.DetectIntentAsync(request.ResponseId, request.QueryResult.QueryText);
            //var response = new DialogflowResponse
            //{
            //    FulfillmentText = responseText
            //};

            //return Ok(response);
        }

    }
}
