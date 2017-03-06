using System.Diagnostics.Tracing;

namespace Dependencies.Logging
{
    [EventSource(Name = "Bot")]
    public sealed class BotEventSource : EventSource
    {
        public class Keywords
        {
            public const EventKeywords Diagnostic = (EventKeywords)1;
            public const EventKeywords ServiceTracing = (EventKeywords)2;
        }


        public const int StartupSucceededId = 1;
        public const int ApplicationFailureId = 2;
        public  const int ServiceMethodStartId = 3;
        public const int ServiceMethodEndId = 4;


        [Event(
                StartupSucceededId,
                Message = "Startup succeeded",
                Level = EventLevel.Informational,
                Keywords = Keywords.Diagnostic
          )]
        internal void StartupSucceeded()
        {
            if (this.IsEnabled())
                this.WriteEvent(StartupSucceededId);
        }


        [Event(
                ApplicationFailureId,
                Message = "Application Failure: {0}",
                Level = EventLevel.Critical,
                Keywords = Keywords.Diagnostic
          )]
        internal void Failure(string message)
        {
            if (this.IsEnabled())
                this.WriteEvent(ApplicationFailureId, message);
        }


        [Event(
                ServiceMethodStartId,
                Message = "Invoking method {0}.{1}",
                Level = EventLevel.Informational,
                Keywords = Keywords.ServiceTracing
          )]
        internal void ServiceMethodStarted(string serviceName, string methodName)
        {
            if (this.IsEnabled())
                this.WriteEvent(ServiceMethodStartId, serviceName, methodName);
        }


        [Event(
                ServiceMethodEndId,
                Message = "Method {0}.{1} finished",
                Level = EventLevel.Informational,
                Keywords = Keywords.ServiceTracing
          )]
        internal void ServiceMethodFinished(string serviceName, string methodName)
        {
            if (this.IsEnabled())
                this.WriteEvent(ServiceMethodEndId, serviceName, methodName);
        }
    }
}