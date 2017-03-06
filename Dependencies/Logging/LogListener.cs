using System;
using System.Diagnostics.Tracing;
using Microsoft.Practices.EnterpriseLibrary.SemanticLogging;
using Microsoft.Practices.Unity;

namespace Dependencies.Logging
{
    public class LogListener : IDisposable
    {
        internal void OnStartup()
        {
            listenerInfo = new ConsoleEventListener(ConsoleColor.DarkBlue, ConsoleColor.Blue);
            listenerInfo.EnableEvents(Log, EventLevel.LogAlways, BotEventSource.Keywords.ServiceTracing);
            
            listenerErrors = new ConsoleEventListener(ConsoleColor.DarkRed, ConsoleColor.Red);
            listenerErrors.EnableEvents(Log, EventLevel.LogAlways, BotEventSource.Keywords.Diagnostic);

            Log.StartupSucceeded();
        }

        public void Dispose()
        {
            listenerInfo.DisableEvents(Log);
            listenerErrors.DisableEvents(Log);

            listenerInfo.Dispose();
            listenerErrors.Dispose();
        }


        [Dependency]
        protected BotEventSource Log { get; set; }

        private EventListener listenerInfo;
        private EventListener listenerErrors;
        
    }
}