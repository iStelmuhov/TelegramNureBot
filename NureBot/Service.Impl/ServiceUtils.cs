using System;
using NureBot.Repository;
using NureBot.Service.Impl.Exceptions;

namespace NureBot.Service.Impl
{
    sealed class ServiceUtils
    {
        private ServiceUtils() { }

        public static TEntity ResolveEntity<TEntity>(
            IRepository<TEntity> repository,
            long domainId
        )
        {
            TEntity entity = repository.FindByDomainId(domainId);
            if (entity != null)
                return entity;

            throw new ServiceUnresolvedEntityException(typeof(TEntity), domainId);
        }
    }
}