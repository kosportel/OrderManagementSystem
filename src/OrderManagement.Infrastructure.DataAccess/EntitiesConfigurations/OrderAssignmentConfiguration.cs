using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Infrastructure.DataAccess.Entities;

namespace OrderManagement.Infrastructure.DataAccess.EntitiesConfigurations
{
    public class OrderAssignmentConfiguration : IEntityTypeConfiguration<OrderAssignmentEntity>
    {
        public void Configure(EntityTypeBuilder<OrderAssignmentEntity> builder)
        {
            builder.ToTable("OrderAssignments");

            builder.HasKey(oa => oa.Id); 

            builder.Property(oa => oa.CreatedDateTime)
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Property(oa => oa.IsCompleted)
                .IsRequired()
                .HasDefaultValue(false);

            // ✅ Foreign Key Relationships
            builder.HasOne(oa => oa.Order)
                .WithMany(o => o.OrderAssignments)
                .HasForeignKey(oa => oa.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(oa => oa.User)
                .WithMany()
                .HasForeignKey(oa => oa.UserId)
                .OnDelete(DeleteBehavior.NoAction); 

            // Index for fast retrieval of active assignments
            builder.HasIndex(oa => new { oa.UserId, oa.IsCompleted });
        }
    }
}
