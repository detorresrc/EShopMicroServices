using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ordering.Infrastructure.Data.Configurations;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).HasConversion(
            orderItemId => orderItemId.Value, // OrderItemId to database value
            dbId => OrderItemId.Of(dbId) // Database value to OrderItemId
        );

        builder.HasOne<Product>()
            .WithMany()
            .HasForeignKey(oi => oi.ProductId);

        builder.Property(oi => oi.Price).IsRequired();
        builder.Property(oi => oi.Quantity).IsRequired();
    }
}