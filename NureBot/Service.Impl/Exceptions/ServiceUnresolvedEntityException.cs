using System;

namespace NureBot.Service.Impl.Exceptions
{
    public class ServiceUnresolvedEntityException : ServiceValidationException
    {
        public ServiceUnresolvedEntityException(Type entityType, long entityId)
            : base(
                    string.Format(
                        "Unresolved entity #{1} of type {0}",
                        entityType.ToString(), entityId
                    )
                )
        { }
    }
}