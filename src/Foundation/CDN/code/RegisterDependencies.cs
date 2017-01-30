namespace Sitecore.Foundation.CDN
{
    using Abstractions;
    using DependencyInjection;
    using Domain;
    using Http;
    using Jobs;
    using Media;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using Providers;
    using Repositories;
    using Resources.Media;

    public class RegisterDependencies : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.Replace(ServiceDescriptor.Singleton<MediaProvider, CdnMediaProvider>());
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