using System;
using System.Linq;
using FluentAssertions;
using Microsoft.ApplicationInsights.Extensibility;
using TrueLayer.Pokemon.Api.Logging;
using Xunit;

namespace TrueLayer.Pokemon.Api.UnitTests.Logging
{
    public class ApplicationInsightsInstrumentorShould
    {
        private readonly IInstrumentor _instrumentor;
        private readonly FakeTelemetryChannel _fakeTelemetryChannel;

        public ApplicationInsightsInstrumentorShould()
        {
            _fakeTelemetryChannel = new();
            _instrumentor = GetApplicationInsightInstrumentor();
        }

        [Theory]
        [InlineData(1, "Something went wrong")]
        [InlineData(1, "Something Set on Fire")]
        public void Send_Exception_Telemetry_When_Log_Exception_Is_Called(int eventId, string message)
        {
            var exception = new Exception(message);
            _instrumentor.LogException(eventId, exception);

            _fakeTelemetryChannel.AllTelemetryEntries.Should().HaveCount(1);
            _fakeTelemetryChannel.ExceptionEntries.Should().HaveCount(1);

            var entry = _fakeTelemetryChannel.ExceptionEntries.FirstOrDefault();

            entry.Exception.Should().Be(exception);
            entry.Properties.Should().Contain(new System.Collections.Generic.KeyValuePair<string, string>("EventId", eventId.ToString()));
        }

        private IInstrumentor GetApplicationInsightInstrumentor()
        {
            var telemetryConfiguration = new TelemetryConfiguration()
            {
                TelemetryChannel = _fakeTelemetryChannel,
                InstrumentationKey = Guid.NewGuid().ToString()
            };

            return new ApplicationInsightsInstrumentor(new(telemetryConfiguration));
        }
    }
}
