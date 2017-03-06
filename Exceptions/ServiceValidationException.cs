using System;

namespace Exceptions
{
    public class ServiceValidationException : Exception
    {
        public ServiceValidationException(string message)
            : base(message)
        { }
    }
}