using System.Collections;
using System.Collections.Generic;
using System.Net;

namespace TrueLayer.Pokemon.Api.Middleware.ExceptionHandler
{
    public class ErrorResponse
    {
        public ErrorResponse(IEnumerable<ErrorDetail>errorDetails, HttpStatusCode httpStatusCode)
        {
            ErrorDetails = errorDetails;
            HttpStatusCode = httpStatusCode;
        }

        public IEnumerable<ErrorDetail> ErrorDetails { get; }
        public HttpStatusCode HttpStatusCode { get; }
    }
}