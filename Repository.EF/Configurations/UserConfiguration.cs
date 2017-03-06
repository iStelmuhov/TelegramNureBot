using System.Data.Entity.ModelConfiguration;
using Model;

namespace Repository.EF.Configurations
{
    public class UserConfiguration:EntityTypeConfiguration<User>
    {
        public UserConfiguration()
        {
            HasKey(a => a.DatabaseId);
            Property(a => a.Id).IsRequired();
            Property(a => a.FirstName).IsRequired();
            Property(a => a.Role).IsRequired();
        }
    }
}