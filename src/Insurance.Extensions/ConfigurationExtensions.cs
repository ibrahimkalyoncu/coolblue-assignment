using Microsoft.Extensions.Configuration;

namespace Insurance.Extensions
{
    public static class ConfigurationExtensions
    {
        public static TConfig Get<TConfig>(this IConfiguration configuration, string sectionName) where TConfig : class, new()
        {
            TConfig config = new TConfig();
            configuration.GetSection(sectionName).Bind(config);
            return config;
        }
    }
}