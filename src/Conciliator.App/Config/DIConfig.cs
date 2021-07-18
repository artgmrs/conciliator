using Conciliator.App.Data.Context;
using Conciliator.App.Data.Repository;
using Conciliator.App.Interfaces;
using Conciliator.App.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Conciliator.App.Config
{
    public static class DIConfig
    {
        public static IServiceCollection ResolveDependencies(this IServiceCollection services)
        {
            services.AddScoped<ConciliatorDbContext>();
            services.AddScoped<IExtractRepository, ExtractRepository>();
            services.AddScoped<IExtractService, ExtractService>();

            return services;
        }
    }
}
