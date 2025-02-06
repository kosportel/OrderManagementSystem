using OrderManagement.Contracts.Common;
using OrderManagement.Contracts.Customers;
using OrderManagement.Domain.Enums;
using System.Text.Json.Serialization;

namespace OrderManagement.Contracts.Orders
{
    public class OrderResponseDto
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int CustomerAddressId { get; set; }

        [JsonConverter(typeof(EnumToStringJsonConverter<OrderTypeEnum>))]
        public OrderTypeEnum OrderTypeId { get; set; }
        public string Notes { get; set; } = string.Empty;
        public DateTime DateTimeCreated { get; set; }

        public CustomerResponseSimpleDto Customer { get; set; }
        public IEnumerable<OrderItemResponseDto> OrderItems { get; set; }
        public IEnumerable<OrderStatusResponseDto> OrderStatuses { get; set; }
    }
}
