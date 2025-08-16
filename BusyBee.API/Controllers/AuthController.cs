using BusyBee.API.DTOs.API;
using BusyBee.API.DTOs.Auth;
using BusyBee.API.Services;
using BusyBee.Domain.Interfaces;
using BusyBee.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using BusyBee.API.DTOs;

namespace BusyBee.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly IUserRepository _userRepository;

        public AuthController(AuthService authService, IUserRepository userRepository)
        {
            _authService = authService;
            _userRepository = userRepository;
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserRequest request)
        {
            var token = await _authService.AuthenticateAsync(request);
            if (token == null)
                return Unauthorized(new { message = "Invalid email or password" });

            return Ok(new ApiResponse<string>
            {
                Status = new StatusInfo
                {
                    Code = StatusCodes.Status200OK,
                    Message = "Login successful",
                    IsSuccess = true
                },
                Meta = new MetaInfo(HttpContext),
                Data = token
            });
        }

        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok(new ApiResponse<string>
            {
                Status = new StatusInfo
                {
                    Code = StatusCodes.Status200OK,
                    Message = "AuthController is working!",
                    IsSuccess = true
                },
                Meta = new MetaInfo(HttpContext),
                Data = "AuthController is working!"
            });
        }


        [HttpPost("signup")]
        public async Task<IActionResult> Signup([FromBody] RegisterUserRequest request)
        {
            var result = await _authService.RegisterAsync(request);
            if (!result.Success)
                return BadRequest(new { message = result.ErrorMessage });

            return Ok(new ApiResponse<string>
            {
                Status = new StatusInfo
                {
                    Code = StatusCodes.Status200OK,
                    Message = "User registered successfully",
                    IsSuccess = true
                },
                Meta = new MetaInfo(HttpContext),
                Data = "User registered successfully"
            });
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> Me()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Unauthorized(new ApiResponse<string>
                { 
                    Status = new StatusInfo
                    {
                        Code = StatusCodes.Status401Unauthorized,
                        Message = "User not authenticated",
                        IsSuccess = false
                    },
                    Meta = new MetaInfo(HttpContext),
                    Data = null
                });
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
                return NotFound(new ApiResponse<string>
                {
                    Status = new StatusInfo
                    {
                        Code = StatusCodes.Status404NotFound,
                        Message = "User not found",
                        IsSuccess = false
                    },
                    Meta = new MetaInfo(HttpContext),
                    Data = null
                });

            return Ok(new ApiResponse<UserDTO>
            {
                Status = new StatusInfo
                {
                    Code = StatusCodes.Status200OK,
                    Message = "User authenticated successfully",
                    IsSuccess = true
                },
                Meta = new MetaInfo(HttpContext),
                Data = new UserDTO
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    FullName = user.FullName,
                    UserRoles = await _userRepository.GetUserRolesAsync(user.Id)
                }
            });
        }
  
    }
}

