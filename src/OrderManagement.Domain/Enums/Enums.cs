namespace OrderManagement.Domain.Enums
{

    // TODO: Fix the swagger to display the enum values as strings
    public enum UserRoleEnum
    {
        Admin = 1,
        Staff = 2,
        Delivery = 3, 
        User = 4
    }

    public enum OrderTypeEnum
    {
        Pickup = 1,
        Delivery = 2
    }

    public enum OrderStatusEnum
    {
        None = 0,
        PendingPickup = 1,
        PreparingPickup = 2,
        ReadyForPickup = 3,
        
        PendingDelivery = 5,
        PreparingDelivery = 6,
        ReadyForDelivery = 7,
        
        OutForDelivery = 8,
        Delivered = 9,
        UnableToDeliver = 10
    }
}
