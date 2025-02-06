using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderManagement.Infrastructure.DataAccess.Entities;

namespace OrderManagement.Infrastructure.DataAccess.EntitiesConfigurations;

public class CustomerAddressConfiguration : IEntityTypeConfiguration<CustomerAddressEntity>
{
    public void Configure(EntityTypeBuilder<CustomerAddressEntity> builder)
    {
        builder.ToTable("CustomerAddresses");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).ValueGeneratedOnAdd();

        builder.Property(e => e.Street)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(e => e.City)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.PostalCode)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(e => e.BuildingNr)
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(e => e.Floor)
            .IsRequired();

        builder.Property(a => a.Latitude)
            .IsRequired()
            .HasColumnType("decimal(9,6)");

        builder.Property(a => a.Longitude)
            .IsRequired()
            .HasColumnType("decimal(9,6)");

        builder.Property(a => a.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);

    }
}