using Microsoft.Extensions.DependencyInjection;

namespace Insurance.Services
{
    public static class ServiceCollectionExtensions
    {
        public static void AddBusinessServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IInsuranceCalculatorService, InsuranceCalculatorService>();
        }
    }
}