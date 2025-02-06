using OrderManagement.Contracts.Common;
using System.Text.Json.Serialization;
using OrderManagement.Domain.Enums;

namespace OrderManagement.Contracts.Orders
{
    public class OrderRequestDto
    {
        public int Id { get; set; }
        public required int CustomerId { get; set; }
        public required int AddressId { get; set; }
        
        [JsonConverter(typeof(EnumToStringJsonConverter<OrderTypeEnum>))]
        public required OrderTypeEnum OrderTypeId { get; set; }
        public string Notes { get; set; } = string.Empty;

        public List<OrderItemRequestDto> OrderItems { get; set; } = new();
    }
}
