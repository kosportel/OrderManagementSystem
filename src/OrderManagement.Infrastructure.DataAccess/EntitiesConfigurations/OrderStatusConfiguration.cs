using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderManagement.Infrastructure.DataAccess.Entities;

namespace OrderManagement.Infrastructure.DataAccess.EntitiesConfigurations
{
    public class OrderStatusEntityConfiguration : IEntityTypeConfiguration<OrderStatusEntity>
    {
        public void Configure(EntityTypeBuilder<OrderStatusEntity> builder)
        {
            builder.ToTable("OrderStatuses");

            builder.HasKey(os => new { os.OrderId, os.OrderStatusId, os.DateTimeCreated });

            builder.Property(os => os.DateTimeCreated)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.HasOne(os => os.Order)
                .WithMany(o => o.OrderStatuses)
                .HasForeignKey(os => os.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
