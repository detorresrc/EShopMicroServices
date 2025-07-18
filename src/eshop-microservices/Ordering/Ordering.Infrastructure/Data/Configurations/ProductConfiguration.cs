using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ordering.Infrastructure.Data.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).HasConversion(
            productId => productId.Value, // ProductId to database value
            dbId => ProductId.Of(dbId) // Database value to ProductId
        );
        
        builder.Property(p => p.Name).HasMaxLength(100).IsRequired();
    }
}