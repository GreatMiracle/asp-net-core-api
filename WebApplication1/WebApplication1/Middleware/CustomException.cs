namespace WebApplication1.Middleware
{
    public class CustomException : Exception
    {
        public int StatusCode { get; }

        public CustomException(string message, int statusCode) : base(message)
        {
            StatusCode = statusCode;
        }

        public CustomException(string message, int statusCode, Exception innerException)
            : base(message, innerException)
        {
            StatusCode = statusCode;
        }
    }
}
