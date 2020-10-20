using Insurance.ConnectedServices.ProductApi;
using Insurance.Extensions;
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
}