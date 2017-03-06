using System.Data.Entity.ModelConfiguration;
using Model;

namespace Repository.EF.Configurations
{
    public class StudentConfiguration:EntityTypeConfiguration<Student>
    {
        public StudentConfiguration()
        {
            HasKey(a => a.DatabaseId);
            Property(a => a.Id).IsRequired();
            Property(a => a.FirstName).IsRequired();
            Property(a => a.Role).IsRequired();

            Property(a => a.GroupId).IsOptional();
        }
    }
}