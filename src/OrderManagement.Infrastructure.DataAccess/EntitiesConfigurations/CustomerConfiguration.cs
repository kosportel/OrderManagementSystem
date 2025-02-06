using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderManagement.Infrastructure.DataAccess.Entities;

namespace OrderManagement.Infrastructure.DataAccess.EntitiesConfigurations;

public class CustomerConfiguration : IEntityTypeConfiguration<CustomerEntity>
{
    public void Configure(EntityTypeBuilder<CustomerEntity> builder)
    {
        builder.ToTable("Customers");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).ValueGeneratedOnAdd();

        builder.Property(e => e.UserId).IsRequired();

        builder.Property(a => a.IsDeleted)
            .IsRequired()
        .HasDefaultValue(false);

        builder.HasOne(c => c.User)  
            .WithOne()            
            .HasForeignKey<CustomerEntity>(c => c.UserId) 
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(c => c.UserId)
            .IsUnique();

        // One-to-Many Relationship: Customer → Addresses
        builder.HasMany(e => e.Addresses)
            .WithOne()
            .HasForeignKey("CustomerId") 
            .OnDelete(DeleteBehavior.NoAction);

        // One-to-Many Relationship: Customer → Phones
        builder.HasMany(e => e.Phones)
            .WithOne()
            .HasForeignKey("CustomerId") 
            .OnDelete(DeleteBehavior.NoAction);
    }
}