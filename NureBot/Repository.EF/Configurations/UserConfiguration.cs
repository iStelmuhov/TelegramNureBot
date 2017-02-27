using System.Data.Entity.ModelConfiguration;
using System.Runtime.InteropServices.ComTypes;
using TelegramNureBot.Model;

namespace TelegramNureBot.Repository.EF.Configurations
{
    public class UserConfiguration:EntityTypeConfiguration<User>
    {
        public UserConfiguration()
        {
            HasKey(a => a.DatabaseId);
            Property(a => a.Id).IsRequired();
            Property(a => a.FirstName).IsRequired();
            Property(a => a.Role).IsRequired();
            Property(a => a.Group).IsOptional();
            
        }
    }
}