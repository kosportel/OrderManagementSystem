using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OrderManagement.Domain.Enums;
using OrderManagement.Infrastructure.DataAccess.DbContexts;
using OrderManagement.Infrastructure.DataAccess.Entities;

namespace OrderManagement.Infrastructure.DataAccess.Management
{
    public static class SeederManager
    {
        public static IServiceProvider SeedData(this IServiceProvider services, bool isDevelopmentEnv)
        {
            if (!isDevelopmentEnv) return services;

            using var scope = services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<OrderManagementDbContext>();

            AddUser(dbContext);
            AddMenuItems(dbContext);
            AddCustomers(dbContext);
            AddOrders(dbContext);
            AddOrderItems(dbContext);
            AddOrderStatuses(dbContext);

            return services;
        }

        private static void AddUser(OrderManagementDbContext dbContext)
        {

            if (!dbContext.Users.Any()) // Only insert if empty
            {
                dbContext.Users.AddRange(
                    new UserEntity()
                    {
                        FirstName = "Agile",
                        LastName = "Actors Admin",
                        Email = "agile_admin@actors.com",
                        PasswordHash = "I7LUaoRKyQuRwZLJz6ZLiQ==.CltFbLOWFS0i5AeXKRUVNHKgegU8klURtIPziVJjxEA=",
                        RoleId = (int)UserRoleEnum.Admin,
                        IsDeleted = false
                    },
                    new UserEntity()
                    {
                        FirstName = "Agile",
                        LastName = "Actors Staff",
                        Email = "agile_staff@actors.com",
                        PasswordHash = "I7LUaoRKyQuRwZLJz6ZLiQ==.CltFbLOWFS0i5AeXKRUVNHKgegU8klURtIPziVJjxEA=",
                        RoleId = (int)UserRoleEnum.Staff,
                        IsDeleted = false
                    },
                    new UserEntity()
                    {
                        FirstName = "Agile",
                        LastName = "Actors Delivery",
                        Email = "agile_delivery@actors.com",
                        PasswordHash = "I7LUaoRKyQuRwZLJz6ZLiQ==.CltFbLOWFS0i5AeXKRUVNHKgegU8klURtIPziVJjxEA=",
                        RoleId = (int)UserRoleEnum.Delivery,
                        IsDeleted = false
                    },
                    new UserEntity()
                    {
                        FirstName = "Agile",
                        LastName = "Actors User",
                        Email = "agile_user@actors.com",
                        PasswordHash = "I7LUaoRKyQuRwZLJz6ZLiQ==.CltFbLOWFS0i5AeXKRUVNHKgegU8klURtIPziVJjxEA=",
                        RoleId = (int)UserRoleEnum.User,
                        IsDeleted = false
                    },
                    new UserEntity()
                    {
                        FirstName = "Konstantinos",
                        LastName = "Port Admin",
                        Email = "Kon_admin@Port.com",
                        PasswordHash = "I7LUaoRKyQuRwZLJz6ZLiQ==.CltFbLOWFS0i5AeXKRUVNHKgegU8klURtIPziVJjxEA=",
                        RoleId = (int)UserRoleEnum.Admin,
                        IsDeleted = false
                    },
                    new UserEntity()
                    {
                        FirstName = "Konstantinos",
                        LastName = "Port Staff",
                        Email = "Kon_staff@Port.com",
                        PasswordHash = "I7LUaoRKyQuRwZLJz6ZLiQ==.CltFbLOWFS0i5AeXKRUVNHKgegU8klURtIPziVJjxEA=",
                        RoleId = (int)UserRoleEnum.Staff,
                        IsDeleted = false
                    },
                    new UserEntity()
                    {
                        FirstName = "Konstantinos",
                        LastName = "Port Delivery",
                        Email = "Kon_delivery@Port.com",
                        PasswordHash = "I7LUaoRKyQuRwZLJz6ZLiQ==.CltFbLOWFS0i5AeXKRUVNHKgegU8klURtIPziVJjxEA=",
                        RoleId = (int)UserRoleEnum.Delivery,
                        IsDeleted = false
                    },
                    new UserEntity()
                    {
                        FirstName = "Konstantinos",
                        LastName = "Port User",
                        Email = "Kon_user@Port.com",
                        PasswordHash = "I7LUaoRKyQuRwZLJz6ZLiQ==.CltFbLOWFS0i5AeXKRUVNHKgegU8klURtIPziVJjxEA=",
                        RoleId = (int)UserRoleEnum.User,
                        IsDeleted = false
                    }
                );
                dbContext.SaveChanges();
            }
        }

        private static void AddMenuItems(OrderManagementDbContext dbContext)
        {
            
            if (!dbContext.MenuItems.Any()) // Only insert if empty
            {
                dbContext.MenuItems.AddRange(
                    new MenuItemEntity
                    {
                        Name = "Margherita Pizza",
                        Ingredients = "Tomato, Mozzarella, Basil",
                        Allergies = "Dairy, Gluten",
                        Price = 8.99m,
                        ExpectedPrepMinutes = 15,
                    },
                    new MenuItemEntity
                    {
                        Name = "Cheeseburger",
                        Ingredients = "Beef, Cheese, Lettuce, Tomato, Bun",
                        Allergies = "Dairy, Gluten",
                        Price = 10.99m,
                        ExpectedPrepMinutes = 10,
                    },
                    new MenuItemEntity
                    {
                        Name = "Caesar Salad",
                        Ingredients = "Romaine Lettuce, Croutons, Parmesan Cheese, Caesar Dressing",
                        Allergies = "Dairy, Gluten, Eggs",
                        Price = 7.99m,
                        ExpectedPrepMinutes = 5,
                    },
                    new MenuItemEntity
                    {
                        Name = "Spaghetti Carbonara",
                        Ingredients = "Spaghetti, Eggs, Parmesan Cheese, Bacon, Black Pepper",
                        Allergies = "Dairy, Eggs, Gluten",
                        Price = 12.99m,
                        ExpectedPrepMinutes = 20,
                    },
                    new MenuItemEntity
                    {
                        Name = "Grilled Chicken Sandwich",
                        Ingredients = "Chicken, Lettuce, Tomato, Whole Wheat Bun",
                        Allergies = "Gluten",
                        Price = 9.99m,
                        ExpectedPrepMinutes = 12,
                    }
                );
                dbContext.SaveChanges();
            }
        }

        private static void AddCustomers(OrderManagementDbContext dbContext)
        {
            if (dbContext.Customers.Any()) return;

            var random = new Random();

            foreach (var user in dbContext.Users.Where(x => x.RoleId == (int)UserRoleEnum.User).ToList())
            {
                dbContext.Add(new CustomerEntity
                {
                    UserId = user.Id,
                    Addresses =
                    [
                        new CustomerAddressEntity
                        {
                            Street = $"kpStreet{random.Next(1, 100)}",
                            City = "Greece",
                            PostalCode = "10001",
                            BuildingNr = "12A",
                            Floor = 2,
                            Latitude = 34.0522,
                            Longitude = -118.2437, 
                            IsDeleted = false
                        },
                        new CustomerAddressEntity
                        {
                            Street = $"kpStreet{random.Next(1, 100)}",
                            City = "Greece",
                            PostalCode = "10001",
                            BuildingNr = "12A",
                            Floor = 2,
                            Latitude = 34.0522,
                            Longitude = -118.2437,
                            IsDeleted = false
                        }
                    ],
                    Phones =
                    [
                        new CustomerPhoneEntity { Telephone = "+1234567890" },
                        new CustomerPhoneEntity { Telephone = "+0987654321" }
                    ]
                });
            }

            dbContext.SaveChanges();

        }

        private static void AddOrders(OrderManagementDbContext dbContext)
        {
            if (!dbContext.Orders.Any())
            {
                var customers = dbContext.Customers.Include(x => x.Addresses).ToList();
                var menuItems = dbContext.MenuItems.ToList();
               
                if (!customers.Any() || !menuItems.Any())
                    return; 

                var random = new Random();
                var orders = new List<OrderEntity>();

                for (var i = 0; i < 10; i++) 
                {
                    var customer = customers[random.Next(customers.Count)];
                    var address = customer.Addresses.FirstOrDefault();

                    if (address == null) continue;

                    var order = new OrderEntity
                    {
                        CustomerId = customer.Id,
                        OrderTypeId = random.Next(1,3),
                        AddressId = address.Id,
                        Notes = $"Test order {i + 1}",
                        DateTimeCreated = DateTime.UtcNow
                    };

                    orders.Add(order);
                }

                dbContext.Orders.AddRange(orders);
                dbContext.SaveChanges();
            }
        }

        private static void AddOrderItems(OrderManagementDbContext dbContext)
        {
            if (!dbContext.OrderItems.Any())
            {
                var orders = dbContext.Orders.ToList();
                var menuItems = dbContext.MenuItems.ToList();
                var random = new Random();

                if (!orders.Any() || !menuItems.Any()) return; 

                var orderItems = new List<OrderItemEntity>();

                foreach (var order in orders)
                {
                    var itemsCount = random.Next(1, 4); 
                    for (var j = 0; j < itemsCount; j++)
                    {
                        var menuItem = menuItems[random.Next(menuItems.Count)];
                        orderItems.Add(new OrderItemEntity
                        {
                            OrderId = order.Id,
                            MenuItemId = menuItem.Id,
                            Quantity = random.Next(1, 5),
                            Price = menuItem.Price,
                            Notes = "Extra sauce"
                        });
                    }
                }

                dbContext.OrderItems.AddRange(orderItems);
                dbContext.SaveChanges();
            }
        }

        private static void AddOrderStatuses(OrderManagementDbContext dbContext)
        {
            if (!dbContext.OrderStatuses.Any())
            {
                var orders = dbContext.Orders.ToList();
                var orderStatuses = new List<OrderStatusEntity>();

                for (var i = 0; i<= orders.Count-1; i++)
                {
                    var initialStep = orders[i].OrderTypeId == (int)OrderTypeEnum.Pickup ? (int)OrderStatusEnum.PendingPickup : (int)OrderStatusEnum.PendingDelivery; 

                    orderStatuses.Add(new OrderStatusEntity
                    {
                        OrderId = orders[i].Id,
                        OrderStatusId = initialStep,
                        DateTimeCreated = DateTime.UtcNow
                    });
                }

                dbContext.OrderStatuses.AddRange(orderStatuses);
                dbContext.SaveChanges();
            }
        }
    }
}
