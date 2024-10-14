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
        public async Task<IActionResult> HandleChatbotRequest([FromBody] DialogflowRequest request)
        {
            var responseText = await _dialogflowService.DetectIntentAsync(request.ResponseId, request.QueryResult.QueryText);
            var response = new DialogflowResponse
            {
                FulfillmentText = responseText
            };

            return Ok(response);
        }

    }
}
