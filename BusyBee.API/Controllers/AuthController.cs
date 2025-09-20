using BusyBee.API.DTOs;
using BusyBee.API.DTOs.API;
using BusyBee.API.DTOs.Auth;
using BusyBee.API.Services;
using BusyBee.Domain.Interfaces;
using BusyBee.Domain.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BusyBee.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly IUserRepository _userRepository;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly SignInManager<User> _signInManager;


        public AuthController(
            AuthService authService,
            IUserRepository userRepository,
            UserManager<User> userManager,
            IConfiguration configuration,
            SignInManager<User> signInManager)
        {
            _authService = authService;
            _userRepository = userRepository;
            _userManager = userManager;
            _configuration = configuration;
            _signInManager = signInManager;
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
                return Unauthorized();

            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
                return NotFound();

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

        // === Новый блок для входа через Google ===

        [HttpGet("external-login")]
        public IActionResult ExternalLogin([FromQuery] string provider, [FromQuery] string returnUrl)
        {
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Auth", new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }

        [HttpGet("external-login-callback")]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = "/")
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
                return Redirect("/login?error=ExternalLoginFailed");

            // ищем пользователя
            var user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
            if (user == null)
            {
                // если нет — создаём
                user = new User
                {
                    UserName = info.Principal.FindFirstValue(ClaimTypes.Email),
                    Email = info.Principal.FindFirstValue(ClaimTypes.Email),
                    FullName = info.Principal.FindFirstValue(ClaimTypes.Name)
                };

                await _userManager.CreateAsync(user);
                await _userManager.AddLoginAsync(user, info);
            }

            var token = await _authService.GenerateJwtToken(user); ;
            return Redirect($"{returnUrl}?token={token}");
        }
    }
}
