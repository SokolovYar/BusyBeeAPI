using BusyBee.Domain.Models;
using BusyBee.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using BusyBee.API.DTOs.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BusyBee.API.Services
{
    public class AuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly PasswordHasher<User> _passwordHasher = new();

        public AuthService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public async Task<string?> AuthenticateAsync(LoginUserRequest request)
        {
            var user = await _userRepository.GetRequiredUserAsync(request.Email);
            if (user == null) return null;

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash!, request.Password);
            if (result == PasswordVerificationResult.Failed) return null;

            return GenerateJwtToken(user);
        }

        public async Task<RegisterResult> RegisterAsync(RegisterUserRequest request)
        {
            var existingUser = await _userRepository.GetRequiredUserAsync(request.Email);
            if (existingUser != null)
                return RegisterResult.Fail("User with this email already exists");

            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                UserName = request.UserName,
                FullName = request.FullName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                LockoutEnabled = !request.RememberMe
            };

            user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);
            await _userRepository.AddUserAsync(user);
            return RegisterResult.Ok();
        }

        private string GenerateJwtToken(User user)
        {
            
            var lockoutEnd = DateTime.UtcNow.AddHours(4).ToString("o");
            if (user.LockoutEnabled!) lockoutEnd = DateTime.UtcNow.AddHours(72).ToString("o");
   

            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Email, user.Email ?? ""),
            new Claim(ClaimTypes.Name, user.FullName ?? ""),
            new Claim(ClaimTypes.Role, "customer"),     // Временно добавляем роль "customer"
            new Claim(ClaimTypes.Expiration, lockoutEnd)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
