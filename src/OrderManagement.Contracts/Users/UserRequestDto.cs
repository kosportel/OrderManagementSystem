using OrderManagement.Contracts.Common;
using OrderManagement.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OrderManagement.Contracts.Users
{
    public class UserRequestDto
    {
        public int Id { get; set; }
        [Required] public string FirstName { get; set; } = string.Empty;
        [Required] public string LastName { get; set; } = string.Empty;
        [Required] public string Email { get; set; } = string.Empty;
        [Required] public string Password { get; set; } = string.Empty;

        [JsonConverter(typeof(EnumToStringJsonConverter<UserRoleEnum>))]
        [Required] public UserRoleEnum RoleId { get; set; }
    }
}
