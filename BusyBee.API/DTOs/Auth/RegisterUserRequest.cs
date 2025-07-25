namespace BusyBee.API.DTOs.Auth
{
    public record RegisterUserRequest
    {
        public required string UserName { get; init; }
        public required string FullName { get; init; }
        public required string Email { get; init; }
        public required string PhoneNumber { get; init; }
        public required string Password { get; init; }
        public bool RememberMe { get; init; } = true;
    }
}
