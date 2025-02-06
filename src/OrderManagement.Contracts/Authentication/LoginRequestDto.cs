﻿namespace OrderManagement.Contracts.Authentication
{
    public class LoginRequestDto
    {
        public required string Email { get; set; } = string.Empty;
        public required string Password { get; set; } = string.Empty;
    }
}
