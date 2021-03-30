using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using TrueLayer.Pokemon.Api.Logging;
using TrueLayer.Pokemon.Api.Middleware.ExceptionHandler;

namespace TrueLayer.Pokemon.Api.Middleware
{
    public class GlobalExceptionHandlerMiddleware
    {
        private const int DefaultEventId = EventIds.GeneralUnhandledException;

        private readonly RequestDelegate _next;

        public GlobalExceptionHandlerMiddleware( RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IInstrumentor instrumentor, IExceptionToErrorResponseMapping exceptionToErrorResponseMapping)
        {
            try
            {
                await _next(context);
            }
            catch(Exception ex)
            {
                instrumentor.LogException(DefaultEventId, ex);
                await WriteResponseAsync(context, ex, exceptionToErrorResponseMapping);
            }
        }

        private async Task WriteResponseAsync(HttpContext context, Exception ex, IExceptionToErrorResponseMapping exceptionToErrorResponseMapping)
        {
            var errorResponse = exceptionToErrorResponseMapping.DeriveErrorResponse(ex);
            var result = JsonSerializer.Serialize(errorResponse.ErrorDetails);
            context.Response.ContentType = "application.json";
            context.Response.StatusCode = (int)errorResponse.HttpStatusCode;
            context.Response.GetTypedHeaders().CacheControl = new Microsoft.Net.Http.Headers.CacheControlHeaderValue
            {
                NoStore = true
            };

            await context.Response.WriteAsync(result);
        }
    }
}
