using System.Data.Entity;
using System.Linq;
using NureBot.Model;

namespace NureBot.Repository.EF.Implementations
{
    public class UserRepository:BasicRepository<User>,IUserRepository
    {
        public UserRepository(NureBotDbContext dbContext, DbSet<User> dbSet) 
            : base(dbContext, dbSet)
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