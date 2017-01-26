namespace Sitecore.Foundation.CDN
{
    using DependencyInjection;
    using Jobs;
    using Microsoft.Extensions.DependencyInjection;
    using Providers;

    public class RegisterDependencies : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<ISiteProvider, SiteProvider>();
            serviceCollection.AddScoped<IJobRunner, JobRunner>();
            serviceCollection.AddScoped<IDatabaseProvider, DefaultDatabaseProvider>();
        }
    }
}