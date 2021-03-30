using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;

namespace TrueLayer.Pokemon.Api.UnitTests.Logging
{
    public class FakeTelemetryChannel : ITelemetryChannel
    {
        public ConcurrentBag<ITelemetry> AllTelemetryEntries { get; } = new ConcurrentBag<ITelemetry>();
        
        public IList<ExceptionTelemetry> ExceptionEntries => GetEntries<ExceptionTelemetry>();

        public bool? DeveloperMode { get; set; }
        
        public string EndpointAddress { get; set; }

        public void Dispose()
        {}

        public void Flush()
        {}

        public void Send(ITelemetry item) => AllTelemetryEntries.Add(item);
        
        private IList<T> GetEntries<T>() =>
           AllTelemetryEntries
               .Where(x => x is T)
               .Cast<T>()
               .ToList();

    }
}
