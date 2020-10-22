using Insurance.Data.Database.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Insurance.Data
{
    public static class ServiceCollectionExtensions
    {
        public static void AddDbContext(this IServiceCollection serviceCollection, string connectionString)
        {
            serviceCollection.AddDbContext<InsuranceDbContext>(options => options.UseSqlServer(connectionString));
        }
    }
}