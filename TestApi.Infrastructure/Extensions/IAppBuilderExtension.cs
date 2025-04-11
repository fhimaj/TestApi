using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TestApi.Infrastructure.DbContexts;

namespace TestApi.Infrastructure.Extensions
{
    public static class IAppBuilderExtension
    {
        public static void Migrate(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();

            var db = scope.ServiceProvider.GetRequiredService<MainDbContext>();

            db.Database.Migrate();
        }
    }
}
