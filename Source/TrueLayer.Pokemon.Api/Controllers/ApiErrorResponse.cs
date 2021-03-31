namespace TrueLayer.Pokemon.Api.Controllers
{
    internal class ApiErrorResponse
    {
        private int errorCode;
        private string errorMessage;

        public ApiErrorResponse(int errorCode, string errorMessage)
        {
            this.errorCode = errorCode;
            this.errorMessage = errorMessage;
        }
    }
}