using BusyBee.Domain.Interfaces;
using BusyBee.Domain.Models;
using BusyBee.API.DTOs.API;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace BusyBee.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }


        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetUserByIdAsync(string id)
        {
            try
            {
                var user = await _userRepository.GetUserByIdAsync(id);
                user.PasswordHash = null; 
                return Ok(new ApiResponse<User>
                {
                    Status = new StatusInfo
                    {
                        Code = StatusCodes.Status200OK,
                        Message = "User retrieved successfully",
                        IsSuccess = true
                    },
                    Meta = new MetaInfo(HttpContext),
                    Data = user
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string>
                {
                    Status = new StatusInfo
                    {
                        Code = StatusCodes.Status500InternalServerError,
                        Message = $"Error retrieving user by id: {ex.Message}",
                        IsSuccess = false
                    },
                    Meta = new MetaInfo(HttpContext),
                    Data = null
                });
            }
        }


        [HttpGet("email/{email}")]
        public async Task<IActionResult> GetUserByEmailAsync(string email)
        {
            try
            {
                var user = await _userRepository.GetRequiredUserAsync(email);
                user.PasswordHash = null;

                return Ok(new ApiResponse<User>
                {
                    Status = new StatusInfo
                    {
                        Code = StatusCodes.Status200OK,
                        Message = "User retrieved successfully",
                        IsSuccess = true
                    },
                    Meta = new MetaInfo(HttpContext),
                    Data = user
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string>
                {
                    Status = new StatusInfo
                    {
                        Code = StatusCodes.Status500InternalServerError,
                        Message = $"Error retrieving user by email: {ex.Message}",
                        IsSuccess = false
                    },
                    Meta = new MetaInfo(HttpContext),
                    Data = null
                });
            }
        }


        /*
         * Удалён, т.к. проблемы с передачей пароля. 
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddUserAsync([FromBody] User user)
        {
            try
            {
                await _userRepository.AddUserAsync(user);
                return CreatedAtAction(nameof(GetUserByIdAsync), new { userId = user.Id }, new ApiResponse<User>
                {
                    Status = new StatusInfo
                    {
                        Code = StatusCodes.Status201Created,
                        Message = "User created successfully",
                        IsSuccess = true
                    },
                    Meta = new MetaInfo(HttpContext),
                    Data = user
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string>
                {
                    Status = new StatusInfo
                    {
                        Code = StatusCodes.Status500InternalServerError,
                        Message = $"Error creating user: {ex.Message}",
                        IsSuccess = false
                    },
                    Meta = new MetaInfo(HttpContext),
                    Data = null
                });
            }
        }
        */

        [Authorize (Roles = "Admin")]
        [HttpPut]
        public async Task<IActionResult> UpdateUserAsync([FromBody] User user)
        {
            try
            {
                await _userRepository.UpdateUserAsync(user);
                return Ok(new ApiResponse<User>
                {
                    Status = new StatusInfo
                    {
                        Code = StatusCodes.Status200OK,
                        Message = "User updated successfully",
                        IsSuccess = true
                    },
                    Meta = new MetaInfo(HttpContext),
                    Data = user
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string>
                {
                    Status = new StatusInfo
                    {
                        Code = StatusCodes.Status500InternalServerError,
                        Message = $"Error updating user: {ex.Message}",
                        IsSuccess = false
                    },
                    Meta = new MetaInfo(HttpContext),
                    Data = null
                });
            }
        }


        [Authorize(Roles = "Admin")]
        [HttpDelete("delete/{userId}")]
        public async Task<IActionResult> DeleteUserAsync(string userId)
        {
            try
            {
                await _userRepository.DeleteUserAsync(userId);
                return Ok(new ApiResponse<string>
                {
                    Status = new StatusInfo
                    {
                        Code = StatusCodes.Status200OK,
                        Message = "User deleted successfully",
                        IsSuccess = true
                    },
                    Meta = new MetaInfo(HttpContext),
                    Data = null
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string>
                {
                    Status = new StatusInfo
                    {
                        Code = StatusCodes.Status500InternalServerError,
                        Message = $"Error deleting user: {ex.Message}",
                        IsSuccess = false
                    },
                    Meta = new MetaInfo(HttpContext),
                    Data = null
                });
            }
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllUsersAsync()
        {
            try
            {
                var users = await _userRepository.GetAllUsersAsync();
                return Ok(new ApiResponse<IEnumerable<User>>
                {
                    Status = new StatusInfo
                    {
                        Code = StatusCodes.Status200OK,
                        Message = "Users retrieved successfully",
                        IsSuccess = true
                    },
                    Meta = new MetaInfo(HttpContext),
                    Data = users
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string>
                {
                    Status = new StatusInfo
                    {
                        Code = StatusCodes.Status500InternalServerError,
                        Message = $"Error retrieving all users: {ex.Message}",
                        IsSuccess = false
                    },
                    Meta = new MetaInfo(HttpContext),
                    Data = null
                });
            }
        }


        //404 ошибка. потом разобраться!
        [Authorize(Roles = "Admin")]
        [HttpPost("change-role/{userId}")]
        public async Task<IActionResult> ChangeUserRoleAsync(string userId, [FromQuery] string newUserRole)
        {
            try
            {
                await _userRepository.ChangeUserRoleAsync(userId, newUserRole);
                return Ok(new ApiResponse<string>
                {
                    Status = new StatusInfo
                    {
                        Code = StatusCodes.Status200OK,
                        Message = "User role changed successfully",
                        IsSuccess = true
                    },
                    Meta = new MetaInfo(HttpContext),
                    Data = null
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string>
                {
                    Status = new StatusInfo
                    {
                        Code = StatusCodes.Status500InternalServerError,
                        Message = $"Error changing user role: {ex.Message}",
                        IsSuccess = false
                    },
                    Meta = new MetaInfo(HttpContext),
                    Data = null
                });
            }
        }

    }
}
