namespace BusyBee.API.DTOs.Auth
{
    public class RegisterResult
    {
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
        public static RegisterResult Ok() => new() { Success = true };
        public static RegisterResult Fail(string message) => new() { Success = false, ErrorMessage = message };
    }
}
