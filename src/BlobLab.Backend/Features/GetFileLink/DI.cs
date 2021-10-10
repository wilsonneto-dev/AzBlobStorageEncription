using Microsoft.Extensions.DependencyInjection;

namespace BlobLab.Backend.Features.GetFileLink
{
    public static class DI
    {
        public static IServiceCollection AddGetFileLinkFeature(this IServiceCollection services)
        {
            services.AddScoped<IDataAccess, DataAccess>();
            services.AddScoped<IStorageService, StorageService>();
            return services;
        }
    }
}
