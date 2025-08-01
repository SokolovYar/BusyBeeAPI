﻿namespace BusyBee.API.DTOs.Auth
{
    public record LoginUserRequest
    {
        public string Password { get; set; } = null!;
        public string Email { get; set; } = null!;
        public bool RememberMe { get; set; } = false;
    }
}
