using Microsoft.Extensions.DependencyInjection;

namespace BlobLab.Backend.Features.ListFiles
{
    public static class DI
    {
        public static IServiceCollection AddListFilesFeature(this IServiceCollection services)
        {
            services.AddScoped<IDataAccess, DataAccess>();
            return services;
        }
    }
}
