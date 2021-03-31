namespace TrueLayer.Pokemon.Api.Contract.V1.Response
{
    public class ApiErrorResponse
    {
        public ApiErrorResponse()
        {
        }
        public ApiErrorResponse(int errorCode, string errorMessage)
        {
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
        }

        public int ErrorCode { get; set; }
        public string ErrorMessage { get; set; }

    }
}