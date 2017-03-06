using System;
using System.Diagnostics.Tracing;

namespace Dependencies.Logging
{
    public class ConsoleEventListener : EventListener
    {
        private ConsoleColor BackgroundColor { get; }
        private ConsoleColor ForegroundColor { get; }

        public ConsoleEventListener(ConsoleColor backgroundColor, ConsoleColor foregroundColor)
        {
            BackgroundColor = backgroundColor;
            ForegroundColor = foregroundColor;
        }

        protected override void OnEventWritten(EventWrittenEventArgs eventData)
        {
            var foreground = Console.ForegroundColor;
            var background = Console.BackgroundColor;

            Console.ForegroundColor = ForegroundColor;
            Console.BackgroundColor = BackgroundColor;


            switch (eventData.EventId)
            {
                case BotEventSource.ServiceMethodStartId:
                    Console.WriteLine($"Leavel:{eventData.Level} Message:Invoking method {eventData.Payload[0]}.{eventData.Payload[1]} EventId:{eventData.EventId}");
                    break;
                case BotEventSource.ServiceMethodEndId:
                    Console.WriteLine($"Leavel:{eventData.Level} Message:Method {eventData.Payload[0]}.{eventData.Payload[1]} finished! EventId:{eventData.EventId}");
                    break;
                default: Console.WriteLine($"Leavel:{eventData.Level} Message:{eventData.Message} EventId:{eventData.EventId}");
                break;
            }

            Console.ForegroundColor = foreground;
            Console.BackgroundColor = background;
        }
    }
}