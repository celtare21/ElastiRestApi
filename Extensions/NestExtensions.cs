using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nest;

namespace ElastiRestApi.Extensions
{
    public static class NestExtensions
    {
        public static void AddNestClient(this IServiceCollection services, IConfiguration config)
        {
            var node = new Uri(config.GetSection("UserSecrets")["NodeUri"]);
            var settings = new ConnectionSettings(node);

            settings.BasicAuthentication(config.GetSection("UserSecrets")["Username"], config.GetSection("UserSecrets")["Password"]);
            settings.ThrowExceptions();
            settings.PrettyJson();

            var client = new ElasticClient(settings);

            services.AddSingleton(client);
        }
    }
}
