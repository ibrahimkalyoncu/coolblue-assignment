using Microsoft.Extensions.DependencyInjection;

namespace Insurance.Data
{
    public static class ServiceCollectionExtensions
    {
        public static void AddRepositories(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IInsuranceRuleRepository, InsuranceRuleRepository>();
        }
    }
}