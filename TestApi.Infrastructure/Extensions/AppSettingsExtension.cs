using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using TestApi.Infrastructure.Models;

namespace TestApi.Infrastructure.Extensions
{
    public static class ConfigureAppSettings
    {
        public static AppSettings GetAppSettings(this IServiceCollection services)
        {
            return services.BuildServiceProvider().GetRequiredService<IOptions<AppSettings>>().Value;
        }
    }
}
