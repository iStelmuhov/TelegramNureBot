using System.Linq;

namespace NureBot.Repository
{
    public interface IRepository<T>
    {
        void StartTransaction();

        void Commit();

        void Rollback();

        int Count();

        T Load(long id);

        IQueryable<T> LoadAll();

        void Add(T t);

        void Delete(T t);

        T FindByDomainId(long domainId);

        IQueryable<long> SelectAllDomainIds();
    }
}