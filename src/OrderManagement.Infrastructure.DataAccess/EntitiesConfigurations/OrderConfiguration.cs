using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderManagement.Infrastructure.DataAccess.Entities;

namespace OrderManagement.Infrastructure.DataAccess.EntitiesConfigurations
{
 public class OrderEntityConfiguration : IEntityTypeConfiguration<OrderEntity>
    {
        public void Configure(EntityTypeBuilder<OrderEntity> builder)
        {
            builder.ToTable("Orders");

            builder.HasKey(o => o.Id);
            builder.Property(e => e.Id).ValueGeneratedOnAdd();

            builder.Property(o => o.Notes)
                .HasMaxLength(255);

            builder.Property(o => o.DateTimeCreated)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Property(m => m.IsDeleted)
                .HasDefaultValue(false);

            builder.HasOne(o => o.Customer)
                .WithMany()
                .HasForeignKey(o => o.CustomerId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(o => o.Address)
                .WithMany()
                .HasForeignKey(o => o.AddressId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(o => o.OrderAssignments)
                .WithOne(oa => oa.Order)
                .HasForeignKey(oa => oa.OrderId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }

}
