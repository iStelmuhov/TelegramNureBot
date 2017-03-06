using System;

namespace Exceptions
{
    public class DomainLogicException : Exception
    {
        public DomainLogicException(string message)
            : base(message)
        { }
    }
}