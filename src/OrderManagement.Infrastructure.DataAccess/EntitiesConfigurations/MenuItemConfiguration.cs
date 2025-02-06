using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Infrastructure.DataAccess.Entities;
namespace OrderManagement.Infrastructure.DataAccess.EntitiesConfigurations
{
    public class MenuItemConfiguration : IEntityTypeConfiguration<MenuItemEntity>
    {
        public void Configure(EntityTypeBuilder<MenuItemEntity> builder)
        {
            builder.ToTable("MenuItems");

            builder.HasKey(m => m.Id);
            builder.Property(e => e.Id).ValueGeneratedOnAdd();
            
            builder.Property(m => m.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(m => m.Ingredients)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(m => m.Allergies)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(m => m.Price)
                .IsRequired()
                .HasPrecision(10, 2); // Set precision for money values

            builder.Property(m => m.ExpectedPrepMinutes)
                .IsRequired();

            builder.Property(m => m.IsDeleted)
                .HasDefaultValue(false);
        }
    }
}
