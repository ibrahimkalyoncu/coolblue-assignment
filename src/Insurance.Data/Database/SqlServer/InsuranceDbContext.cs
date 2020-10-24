using System.Reflection;
using Insurance.Data.Domain;
using Microsoft.EntityFrameworkCore;

namespace Insurance.Data.Database.SqlServer
{
    public class InsuranceDbContext : DbContext
    {
        public InsuranceDbContext(){}
        public InsuranceDbContext(DbContextOptions<InsuranceDbContext> options): base(options) {}

        public DbSet<InsuranceRangeRule> InsuranceRangeRules { get; set; }
        public DbSet<InsuranceProductTypeRule> InsuranceProductTypeRules { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}