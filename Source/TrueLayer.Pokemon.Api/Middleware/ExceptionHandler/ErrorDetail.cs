using System.Text.Json.Serialization;

namespace TrueLayer.Pokemon.Api.Middleware.ExceptionHandler
{
    public class ErrorDetail
    {
        public ErrorDetail()
        {
        }

        public ErrorDetail(string errorMessage, ErrorCode errorCode)
        {
            ErrorCode = errorCode.ToString();
            ErrorMessage = errorMessage;
        }

        [JsonPropertyName("errorCode")]
        public string ErrorCode { get; set; }
        [JsonPropertyName("errorMessage")]
        public string ErrorMessage { get; set; }
    }
}