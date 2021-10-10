using Microsoft.Extensions.DependencyInjection;

namespace BlobLab.Backend.Features.DownloadEncriptedFile
{
    public static class DI
    {
        public static IServiceCollection AddDownloadEncriptedFileFeature(this IServiceCollection services)
        {
            services.AddScoped<IDataAccess, DataAccess>();
            services.AddScoped<IStorageService, StorageService>();
            return services;
        }
    }
}
