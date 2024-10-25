using Google.Cloud.Dialogflow.V2;

namespace WebApplication1.DTOs.Request
{
    public class DialogflowRequest
    {
        public string ResponseId { get; set; }
        public QueryResultDTO QueryResult { get; set; }
    }
}
