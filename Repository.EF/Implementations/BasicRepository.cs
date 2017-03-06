using System.Data.Entity;
using System.Linq;

namespace Repository.EF.Implementations
{
    public class BasicRepository<T> where T : class
    {
        private NureBotDbContext dbContext;
        public DbSet<T> dbSet;
        protected BasicRepository(NureBotDbContext dbContext, DbSet<T> dbSet)
        {
            this.dbContext = dbContext;
            this.dbSet = dbSet;
        }

        protected NureBotDbContext GetDBContext()
        {
            return this.dbContext;
        }

        protected DbSet<T> GetDBSet()
        {
            return this.dbSet;
        }

        public void Add(T obj)
        {
            dbSet.Add(obj);
        }

        public void Delete(T obj)
        {
            dbSet.Remove(obj);
        }

        public IQueryable<T> LoadAll()
        {
            return dbSet;
        }

        public T Load(long id)
        {
            return dbSet.Find(id);
        }

        public int Count()
        {
            return dbSet.Count();
        }

        public void StartTransaction()
        {
            this.dbContext.Database.BeginTransaction();
        }

        public void Commit()
        {
            this.dbContext.ChangeTracker.DetectChanges();
            this.dbContext.SaveChanges();
            this.dbContext.Database.CurrentTransaction.Commit();
        }

        public void Rollback()
        {
            this.dbContext.Database.CurrentTransaction.Rollback();
        }
    }
}