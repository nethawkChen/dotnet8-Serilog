using Serilog.Core;
using Serilog.Events;

namespace Serilog.LogBranch.Extensions {
    public class LogEnricher : ILogEventEnricher {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory) {
            if(logEvent.Properties.TryGetValue("SourceContext",out LogEventPropertyValue sourceContext)) {
                var controllerName = sourceContext.ToString().Replace("\"", "").Split('.').Last().Replace("Controllers", "");
                logEvent.AddPropertyIfAbsent(new LogEventProperty("ControllerName",new ScalarValue(controllerName)));
            }
        }
    }
}
