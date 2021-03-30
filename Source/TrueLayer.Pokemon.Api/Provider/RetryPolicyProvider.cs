using System;
using System.Net.Http;
using Polly;
using TrueLayer.Pokemon.Api.Configuration;
using TrueLayer.Pokemon.Api.Logging;

namespace TrueLayer.Pokemon.Api.Provider
{
    public interface IRetryPolicyProvider
    {
        IAsyncPolicy AsyncPolicy { get; }
    }

    public class RetryPolicyProvider : IRetryPolicyProvider
    {
        private readonly IInstrumentor _instrumentor;
        public RetryPolicyProvider(IRetryPolicyConfiguration retryPolicyConfiguration, IInstrumentor instrumentor)
        {
            _instrumentor = instrumentor;

            AsyncPolicy = Policy
                .Handle<HttpRequestException>()
                .WaitAndRetryAsync(retryPolicyConfiguration.RetryAttempts, retryAttempt => TimeSpan.FromMilliseconds(retryPolicyConfiguration.RetryInterval * retryAttempt), OnRetry);
        }

        private void OnRetry(Exception exception, TimeSpan timeSpan)
        {
            _instrumentor.LogException(EventIds.RetryException, exception);
        }

        public IAsyncPolicy AsyncPolicy { get; }
    }
}
