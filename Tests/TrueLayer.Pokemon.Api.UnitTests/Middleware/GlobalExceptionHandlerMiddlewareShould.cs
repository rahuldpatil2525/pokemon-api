using System;
using System.IO;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using TrueLayer.Pokemon.Api.Logging;
using TrueLayer.Pokemon.Api.Middleware;
using TrueLayer.Pokemon.Api.Middleware.ExceptionHandler;
using Xunit;

namespace TrueLayer.Pokemon.Api.UnitTests.Middleware
{
    public class GlobalExceptionHandlerMiddlewareShould
    {
        private readonly Moq.Mock<IInstrumentor> _instrumentor;
        private readonly Moq.Mock<IExceptionToErrorResponseMapping> _exceptionToErrorResponseMapping;
        private ErrorResponse _expectedErrorResponse;
        public GlobalExceptionHandlerMiddlewareShould()
        {
            _instrumentor = new();
            _exceptionToErrorResponseMapping = new();

            _expectedErrorResponse = new ErrorResponse(new[] { new ErrorDetail("Error Message", ErrorCode.UnknownError) }, HttpStatusCode.InternalServerError);

            _exceptionToErrorResponseMapping.Setup(x => x.DeriveErrorResponse(It.IsAny<Exception>())).Returns(_expectedErrorResponse);
        }

        [Fact]
        public async Task Log_General_Unhandled_Exception_And_Return_Error_Response_When_Exception_Thrown()
        {
            var middleware = new GlobalExceptionHandlerMiddleware((innerHttpContext) => throw new System.Exception());

            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();

            await middleware.InvokeAsync(context, _instrumentor.Object, _exceptionToErrorResponseMapping.Object);

            context.Response.Body.Seek(0, SeekOrigin.Begin);

            var body = new StreamReader(context.Response.Body).ReadToEnd();
            var errorMessage = JsonSerializer.Deserialize<ErrorDetail[]>(body);
            errorMessage.Should().BeEquivalentTo(_expectedErrorResponse.ErrorDetails);
            errorMessage.Length.Should().Be(1);

            context.Response.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
    }
}
