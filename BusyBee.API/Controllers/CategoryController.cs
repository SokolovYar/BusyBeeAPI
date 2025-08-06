using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BusyBee.Domain.Models;
using BusyBee.DataAccess.Repositories;
using BusyBee.API.DTOs.API;
using Microsoft.AspNetCore.Authorization;


namespace BusyBee.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly WorkCategoryRepository _categoryRepository;
        public CategoryController(WorkCategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllCategories()
        {
            try
            {
                var categories = await _categoryRepository.GetAllCategoriesAsync();
                return Ok(new ApiResponse<List<WorkCategory>>

                {
                    Status = new StatusInfo
                    {
                        Code = StatusCodes.Status200OK,
                        Message = "Categories retrieved successfully",
                        IsSuccess = true
                    },
                    Meta = new MetaInfo(HttpContext),
                    Data = categories
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string>
                {
                    Status = new StatusInfo
                    {
                        Code = StatusCodes.Status500InternalServerError,
                        Message = $"Error retrieving categories: {ex.Message}",
                        IsSuccess = false
                    },
                    Meta = new MetaInfo(HttpContext),
                    Data = null
                });
            }
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            try
            {
                var category = await _categoryRepository.GetCategoryByIdAsync(id);
                if (category == null)
                {
                    return NotFound($"Category with ID {id} not found.");
                }
                return Ok(new ApiResponse<WorkCategory>
                {
                    Status = new StatusInfo
                    {
                        Code = StatusCodes.Status200OK,
                        Message = "Category retrieved successfully",
                        IsSuccess = true
                    },
                    Meta = new MetaInfo(HttpContext),
                    Data = category
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string>
                {
                    Status = new StatusInfo
                    {
                        Code = StatusCodes.Status500InternalServerError,
                        Message = $"Error retrieving category: {ex.Message}",
                        IsSuccess = false
                    },
                    Meta = new MetaInfo(HttpContext),
                    Data = null
                });
            }
        }


        [Authorize(Policy = "AdminPolicy")]
        [HttpPost("add")]
        public async Task<IActionResult> AddCategory([FromBody] BusyBee.Domain.Models.WorkCategory category)
        {
            if (category == null)
            {
                return BadRequest(new ApiResponse<string>
                {
                    Status = new StatusInfo
                    {
                        Code = StatusCodes.Status400BadRequest,
                        Message = "Category cannot be null.",
                        IsSuccess = false
                    },
                    Meta = new MetaInfo(HttpContext),
                    Data = null
                });
            }
            try
            {
                await _categoryRepository.AddCategoryAsync(category);
                return CreatedAtAction(nameof(GetCategoryById), new { id = category.Id }, new ApiResponse<WorkCategory>
                {
                    Status = new StatusInfo
                    {
                        Code = StatusCodes.Status201Created,
                        Message = "Category added successfully",
                        IsSuccess = true
                    },
                    Meta = new MetaInfo(HttpContext),
                    Data = category
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string>
                {
                    Status = new StatusInfo
                    {
                        Code = StatusCodes.Status500InternalServerError,
                        Message = $"Error adding category: {ex.Message}",
                        IsSuccess = false
                    },
                    Meta = new MetaInfo(HttpContext),
                    Data = null
                });
            }
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] WorkCategory category)
        {
            if (category == null || category.Id != id)
            {
                return BadRequest(new ApiResponse<string>
                {
                    Status = new StatusInfo
                    {
                        Code = StatusCodes.Status400BadRequest,
                        Message = "Invalid category data.",
                        IsSuccess = false
                    },
                    Meta = new MetaInfo(HttpContext),
                    Data = null
                });
            }

            try
            {
                var existingCategory = await _categoryRepository.GetCategoryByIdAsync(id);
                if (existingCategory == null)
                {
                    return NotFound(new ApiResponse<string>
                    {
                        Status = new StatusInfo
                        {
                            Code = StatusCodes.Status404NotFound,
                            Message = $"Category with ID {id} not found.",
                            IsSuccess = false
                        },
                        Meta = new MetaInfo(HttpContext),
                        Data = null
                    });
                }

                // Обновляем только необходимые поля
                existingCategory.Name = category.Name;

                // Сохраняем изменения (метод не должен создавать новый объект)
                await _categoryRepository.UpdateCategoryAsync(existingCategory);

                return Ok(new ApiResponse<WorkCategory>
                {
                    Status = new StatusInfo
                    {
                        Code = StatusCodes.Status200OK,
                        Message = "Category updated successfully",
                        IsSuccess = true
                    },
                    Meta = new MetaInfo(HttpContext),
                    Data = existingCategory
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string>
                {
                    Status = new StatusInfo
                    {
                        Code = StatusCodes.Status500InternalServerError,
                        Message = $"Error updating category: {ex.Message}",
                        IsSuccess = false
                    },
                    Meta = new MetaInfo(HttpContext),
                    Data = null
                });
            }
        }


        //[Authorize(Roles = "admin")]
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                await _categoryRepository.DeleteCategoryAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string>
                {
                    Status = new StatusInfo
                    {
                        Code = StatusCodes.Status500InternalServerError,
                        Message = $"Error deleting category: {ex.Message}",
                        IsSuccess = false
                    },
                    Meta = new MetaInfo(HttpContext),
                    Data = null
                });
            }
        }
    }
}
