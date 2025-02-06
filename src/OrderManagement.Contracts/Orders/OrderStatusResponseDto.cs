using OrderManagement.Contracts.Common;
using OrderManagement.Domain.Enums;
using System.Text.Json.Serialization;

namespace OrderManagement.Contracts.Orders
{
    public class OrderStatusResponseDto
    {
        public int OrderId { get; set; }

        [JsonConverter(typeof(EnumToStringJsonConverter<OrderStatusEnum>))]
        public OrderStatusEnum OrderStatusId { get; set; }
        public DateTime DateTimeCreated { get; set; }
    }
}
