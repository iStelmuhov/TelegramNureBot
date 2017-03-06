using System.Data.Common;
using System.Data.Entity;
using Model;
using Repository.EF.Configurations;

namespace Repository.EF
{
    public class NureBotDbContext : DbContext
    {
        public NureBotDbContext(bool drop = false)
        {
            if (drop)
                Database.SetInitializer(new DropCreateDatabaseAlways<NureBotDbContext>());

            Database.Log = (s => System.Diagnostics.Debug.WriteLine(s));
        }

        public NureBotDbContext() : base()
        {
        }

        public NureBotDbContext(DbConnection connection)
            : base(connection, true)
        {
            Database.SetInitializer(new DropCreateDatabaseAlways<NureBotDbContext>());

            Database.Log = (s => System.Diagnostics.Debug.WriteLine(s));
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new UserConfiguration());
            modelBuilder.Configurations.Add(new StudentConfiguration());
            modelBuilder.Configurations.Add(new TeacherConfiguration());
        }
    }
}