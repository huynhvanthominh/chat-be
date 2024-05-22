
namespace chat_be.Models.Responses
{
    public class PayloadResponse<T>
    {
        public string Message { get; set; }
        public bool Success { get; set; }
        public int StatusCode { get; set; }
        public T? Payload { get; set; }
        public PayloadResponse()
        {
        }

        public PayloadResponse(string message, bool success, T? payload, int statusCode = 200)
        {
            Message = message;
            Success = success;
            Payload = payload;
            StatusCode = statusCode;
        }
    }
}