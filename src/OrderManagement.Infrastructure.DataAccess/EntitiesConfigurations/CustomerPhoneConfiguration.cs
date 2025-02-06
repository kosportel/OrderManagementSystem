using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderManagement.Infrastructure.DataAccess.Entities;

namespace OrderManagement.Infrastructure.DataAccess.EntitiesConfigurations;

public class CustomerPhoneConfiguration : IEntityTypeConfiguration<CustomerPhoneEntity>
{
    public void Configure(EntityTypeBuilder<CustomerPhoneEntity> builder)
    {
        builder.ToTable("CustomerPhones");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).ValueGeneratedOnAdd();

        builder.Property(e => e.Telephone)
            .IsRequired()
            .HasMaxLength(15);
    }
}