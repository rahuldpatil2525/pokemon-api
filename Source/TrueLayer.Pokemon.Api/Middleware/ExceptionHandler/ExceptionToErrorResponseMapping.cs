using System;

namespace TrueLayer.Pokemon.Api.Middleware.ExceptionHandler
{
    public interface IExceptionToErrorResponseMapping
    {
        ErrorResponse DeriveErrorResponse(Exception exception);
    }

    public class ExceptionToErrorResponseMapping : IExceptionToErrorResponseMapping
    {
        public ErrorResponse DeriveErrorResponse(Exception exception)
        {
            return exception switch
            {
                _ => new ErrorResponse(new[]
                {
                    new ErrorDetail("UnKnown Error Occoured", ErrorCode.UnknownError)
                }, System.Net.HttpStatusCode.InternalServerError)
            };
        }
    }
}
