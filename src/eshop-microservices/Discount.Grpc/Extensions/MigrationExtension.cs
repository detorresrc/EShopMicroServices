using Discount.Grpc.Data;
using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Extensions;

public static class MigrationExtension
{
    public static IApplicationBuilder UseMigration(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        using var dbContext = scope.ServiceProvider.GetRequiredService<DiscountDbContext>();
        
        // Check if on development environment
        if (app.ApplicationServices.GetService<IWebHostEnvironment>()?.IsDevelopment() != true) return app;
        
        if(dbContext.Database.GetPendingMigrations().Any())
            dbContext.Database.MigrateAsync();

        return app;
    }
}