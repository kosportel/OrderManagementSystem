using OrderManagement.Contracts.Common;
using OrderManagement.Domain.Enums;
using System.Text.Json.Serialization;

namespace OrderManagement.Contracts.Orders
{
    public class UpdateOrderStatusRequestDto
    {
        [JsonConverter(typeof(EnumToStringJsonConverter<OrderStatusEnum>))]
        public OrderStatusEnum OrderStatusId { get; set; }
    }
}
