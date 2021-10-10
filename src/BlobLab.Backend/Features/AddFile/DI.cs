using Microsoft.Extensions.DependencyInjection;

namespace BlobLab.Backend.Features.AddFile
{
    public static class DI
    {
        public static IServiceCollection AddAddFileFeature(this IServiceCollection services)
        {
            services.AddScoped<IDataAccess, DataAccess>();
            services.AddScoped<IStorageService, StorageService>();
            return services;
        }
    }
}
