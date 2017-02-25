using System;

namespace NureBot.Service.Impl.Exceptions
{
    public class ServiceValidationException : Exception
    {
        public ServiceValidationException(string message)
            : base(message)
        { }
    }
}