using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Ordering.Infrastructure.Data.Extensions;

public static class DatabaseExtensions
{
    public static async Task InitializeDatabaseAsync(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        
            var hasPendingMigrations = await context.Database.GetPendingMigrationsAsync();
            if(hasPendingMigrations.Any())
                await context.Database.MigrateAsync();

            await SeedAsync(context);
        }
    }

    private static async Task SeedAsync(ApplicationDbContext context)
    {
        await SeedCustomerAsync(context);
        await SeedProductAsync(context);
        await SeedOrdersWithItemsAsync(context);
    }

    private static async Task SeedCustomerAsync(ApplicationDbContext context)
    {
        if (!await context.Customers.AnyAsync())
        {
            await context.Customers.AddRangeAsync(InitialData.Customers);
            await context.SaveChangesAsync();
        }
    }
    
    private static async Task SeedProductAsync(ApplicationDbContext context)
    {
        if (!await context.Products.AnyAsync())
        {
            await context.Products.AddRangeAsync(InitialData.Products);
            await context.SaveChangesAsync();
        }
    }

    private static async Task SeedOrdersWithItemsAsync(ApplicationDbContext context)
    {
        if (!await context.Orders.AnyAsync())
        {
            await context.Orders.AddRangeAsync(InitialData.OrdersWithItems);
            await context.SaveChangesAsync();
        }
    }
}