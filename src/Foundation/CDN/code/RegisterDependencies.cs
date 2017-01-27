namespace Sitecore.Foundation.CDN
{
    using DependencyInjection;
    using Domain;
    using Http;
    using Jobs;
    using Microsoft.Extensions.DependencyInjection;
    using Providers;
    using Repositories;

    public class RegisterDependencies : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IPathService, PathService>();
            serviceCollection.AddSingleton<ICdnContextRepository, CdnContextRepository>();
            serviceCollection.AddSingleton<IHttpService, HttpService>();
            serviceCollection.AddSingleton<IHistoryService, HistoryService>();
            serviceCollection.AddSingleton<ISerializer, JsonSerializer>();
            serviceCollection.AddSingleton<ISiteProvider, SiteProvider>();
            serviceCollection.AddSingleton<IDeliveryService, DefaultDeliveryService>();
            serviceCollection.AddScoped<IJobRunner, PurgeJobRunner>();
            serviceCollection.AddScoped<IDatabaseProvider, DefaultDatabaseProvider>();
            serviceCollection.AddScoped<ICdnSettings, CdnSettings>();
        }
    }
}