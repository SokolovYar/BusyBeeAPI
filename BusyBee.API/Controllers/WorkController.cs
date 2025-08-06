using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BusyBee.Domain.Interfaces;
using BusyBee.Domain.Models;
using BusyBee.API.DTOs.API;
using BusyBee.DataAccess.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace BusyBee.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkController : ControllerBase
    {

        private readonly IWorkRepository _workRepository;
        public WorkController(IWorkRepository workRepository)
        {
            _workRepository = workRepository ?? throw new ArgumentNullException(nameof(workRepository));
        }


        [HttpGet("all")]
        public async Task<IActionResult> GetAllWorks()
        {
            try
            {
                var works = await _workRepository.GetAllWorks();
                return Ok(new ApiResponse<List<Work>>
                {
                    Status = new StatusInfo
                    {
                        Code = StatusCodes.Status200OK,
                        Message = "Works retrieved successfully",
                        IsSuccess = true
                    },
                    Meta = new MetaInfo(HttpContext),
                    Data = works.ToList()
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string>
                {
                    Status = new StatusInfo
                    {
                        Code = StatusCodes.Status500InternalServerError,
                        Message = $"Error retrieving works: {ex.Message}",
                        IsSuccess = false
                    },
                    Meta = new MetaInfo(HttpContext),
                    Data = null
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetWorkById(int id)
        {
            try
            {
                var work = await _workRepository.GetWorkById(id);
                if (work == null)
                {
                    return NotFound(new ApiResponse<string>
                    {
                        Status = new StatusInfo
                        {
                            Code = StatusCodes.Status404NotFound,
                            Message = $"Work with ID {id} not found.",
                            IsSuccess = false
                        },
                        Meta = new MetaInfo(HttpContext),
                        Data = null
                    });
                }
                return Ok(new ApiResponse<Work>
                {
                    Status = new StatusInfo
                    {
                        Code = StatusCodes.Status200OK,
                        Message = "Work retrieved successfully",
                        IsSuccess = true
                    },
                    Meta = new MetaInfo(HttpContext),
                    Data = work
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string>
                {
                    Status = new StatusInfo
                    {
                        Code = StatusCodes.Status500InternalServerError,
                        Message = $"Error retrieving work: {ex.Message}",
                        IsSuccess = false
                    },
                    Meta = new MetaInfo(HttpContext),
                    Data = null
                });
            }
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpPost]
        public async Task<IActionResult> AddWork([FromBody] Work work, int categoryId)
        {
            if (work == null)
            {
                return BadRequest(new ApiResponse<string>
                {
                    Status = new StatusInfo
                    {
                        Code = StatusCodes.Status400BadRequest,
                        Message = "Work cannot be null.",
                        IsSuccess = false
                    },
                    Meta = new MetaInfo(HttpContext),
                    Data = null
                });
            }
            try
            {
                await _workRepository.AddWork(work, categoryId);
                return CreatedAtAction(nameof(GetWorkById), new { id = work.Id }, new ApiResponse<Work>
                {
                    Status = new StatusInfo
                    {
                        Code = StatusCodes.Status201Created,
                        Message = "Work created successfully",
                        IsSuccess = true
                    },
                    Meta = new MetaInfo(HttpContext),
                    Data = work
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string>
                {
                    Status = new StatusInfo
                    {
                        Code = StatusCodes.Status500InternalServerError,
                        Message = $"Error creating work: {ex.Message}",
                        IsSuccess = false
                    },
                    Meta = new MetaInfo(HttpContext),
                    Data = null
                });
            }
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateWork(int id, [FromBody] Work work)
        {
            if (work == null || work.Id != id)
            {
                return BadRequest(new ApiResponse<string>
                {
                    Status = new StatusInfo
                    {
                        Code = StatusCodes.Status400BadRequest,
                        Message = "Invalid work data.",
                        IsSuccess = false
                    },
                    Meta = new MetaInfo(HttpContext),
                    Data = null
                });
            }
            try
            {
                await _workRepository.UpdateWork(work);
                return Ok(new ApiResponse<Work>
                {
                    Status = new StatusInfo
                    {
                        Code = StatusCodes.Status200OK,
                        Message = "Work updated successfully",
                        IsSuccess = true
                    },
                    Meta = new MetaInfo(HttpContext),
                    Data = work
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string>
                {
                    Status = new StatusInfo
                    {
                        Code = StatusCodes.Status500InternalServerError,
                        Message = $"Error updating work: {ex.Message}",
                        IsSuccess = false
                    },
                    Meta = new MetaInfo(HttpContext),
                    Data = null
                });
            }
        }


        [Authorize(Policy = "AdminPolicy")]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteWork(int id)
        {
            try
            {
                await _workRepository.DeleteWork(id);
                return Ok(new ApiResponse<string>
                {
                    Status = new StatusInfo
                    {
                        Code = StatusCodes.Status200OK,
                        Message = "Work deleted successfully",
                        IsSuccess = true
                    },
                    Meta = new MetaInfo(HttpContext),
                    Data = null
                });
            }
            catch (KeyNotFoundException knfEx)
            {
                return NotFound(new ApiResponse<string>
                {
                    Status = new StatusInfo
                    {
                        Code = StatusCodes.Status404NotFound,
                        Message = knfEx.Message,
                        IsSuccess = false
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
                        Message = $"Error deleting work: {ex.Message}",
                        IsSuccess = false
                    },
                    Meta = new MetaInfo(HttpContext),
                    Data = null
                });
            }
        }


        [HttpGet("category/{categoryId}")]
        public async Task<IActionResult> GetWorksByCategory(int categoryId)
        {
            try
            {
                var works = await _workRepository.GetWorksByCategory(categoryId);
                if (works == null || !works.Any())
                {
                    return NotFound(new ApiResponse<string>
                    {
                        Status = new StatusInfo
                        {
                            Code = StatusCodes.Status404NotFound,
                            Message = $"No works found for category ID {categoryId}.",
                            IsSuccess = false
                        },
                        Meta = new MetaInfo(HttpContext),
                        Data = null
                    });
                }
                return Ok(new ApiResponse<List<Work>>
                {
                    Status = new StatusInfo
                    {
                        Code = StatusCodes.Status200OK,
                        Message = "Works retrieved successfully",
                        IsSuccess = true
                    },
                    Meta = new MetaInfo(HttpContext),
                    Data = works.ToList()
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string>
                {
                    Status = new StatusInfo
                    {
                        Code = StatusCodes.Status500InternalServerError,
                        Message = $"Error retrieving works by category: {ex.Message}",
                        IsSuccess = false
                    },
                    Meta = new MetaInfo(HttpContext),
                    Data = null
                });
            }
        }












    }
}
