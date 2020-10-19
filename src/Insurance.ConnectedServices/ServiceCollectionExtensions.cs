using Insurance.ConnectedServices.ProductApi;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Insurance.ConnectedServices
{
    public static class ServiceCollectionExtensions
    {
        public static void AddConnectedServices(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddSingleton<IProductApiClient, ProductApiClient>();
            serviceCollection.AddSingleton<IProductApiConfiguration>(configuration.Get<ProductApiConfiguration>(nameof(ProductApiConfiguration)));
        }
    }

    static class ConfigurationExtensions
    {
        public static TConfig Get<TConfig>(this IConfiguration configuration, string sectionName) where TConfig : class, new()
        {
            TConfig config = new TConfig();
            configuration.GetSection(sectionName).Bind(config);
            return config;
        }
    }
}