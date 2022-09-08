using LMS.Data;
using LMS.Data.Data;
using Microsoft.EntityFrameworkCore;

namespace LMS.Web.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static async Task<IApplicationBuilder> SeedDataAsync(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                var db = serviceProvider.GetRequiredService<ApplicationDbContext>();

                db.Database.EnsureDeleted();
                db.Database.Migrate();

                try
                {
                    await SeedData.InitAsync(db, serviceProvider);
                }
                catch (Exception e)
                {

                    throw;
                }
            }

            return app;
        }
    }
}
