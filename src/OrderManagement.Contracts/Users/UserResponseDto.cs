using OrderManagement.Contracts.Common;
using OrderManagement.Domain.Enums;
using System.Text.Json.Serialization;

namespace OrderManagement.Contracts.Users
{
    public class UserResponseDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        [JsonConverter(typeof(EnumToStringJsonConverter<UserRoleEnum>))]
        public UserRoleEnum RoleId { get; set; }
    }
}
