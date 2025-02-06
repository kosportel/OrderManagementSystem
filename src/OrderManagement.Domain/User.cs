using OrderManagement.Domain.Enums;

namespace OrderManagement.Domain
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public UserRoleEnum RoleId { get; set; }
        public bool IsDeleted { get; set; } = false;

    }
}
