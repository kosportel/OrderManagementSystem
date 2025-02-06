using System.Reflection;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Infrastructure.DataAccess.Entities;

namespace OrderManagement.Infrastructure.DataAccess.DbContexts;

public class OrderManagementDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<CustomerEntity> Customers { get; set; }
    public DbSet<CustomerAddressEntity> CustomerAddresses { get; set; }
    public DbSet<CustomerPhoneEntity> CustomerPhones { get; set; }
    public DbSet<MenuItemEntity> MenuItems { get; set; }
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<OrderEntity> Orders { get; set; }
    public DbSet<OrderItemEntity> OrderItems { get; set; }
    public DbSet<OrderStatusEntity> OrderStatuses { get; set; }
    public DbSet<OrderAssignmentEntity> OrderAssignments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);

        // Set Database Collation
        modelBuilder.UseCollation("SQL_Latin1_General_CP1_CI_AS");
    }
}