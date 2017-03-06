using System.Data.Entity;
using System.Linq;
using Model;

namespace Repository.EF.Implementations
{
    public class UserRepository:BasicRepository<User>,IUserRepository
    {
        public UserRepository(NureBotDbContext dbContext) 
            : base(dbContext,dbContext.Users)
        {
        }

        public User FindByDomainId(long domainId)
        {
            return dbSet.SingleOrDefault(e => e.Id == domainId);
        }

        public IQueryable<long> SelectAllDomainIds()
        {
            return dbSet.Select(e => e.Id);
        }

    }
}