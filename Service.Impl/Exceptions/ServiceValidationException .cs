using System;

namespace Service.Impl.Exceptions
{
    public class ServiceValidationException : Exception
    {
        public ServiceValidationException(string message)
            : base(message)
        { }
    }
}