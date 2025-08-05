using BusyBee.API.DTOs;
using BusyBee.API.DTOs.API;
using BusyBee.API.Services;
using BusyBee.DataAccess.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace BusyBee.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SideBarController : ControllerBase
    {
        private readonly ISideBarService _service;

        public SideBarController(ISideBarService service)
        {
            _service = service;
        }

        [OutputCache(Duration = 60)]
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<SideBarDto>> Get()
        {
            Response.Headers["Cache-Control"] = "public, max-age=60";
            var dto = await _service.GetSideBarAsync(); 
            return Ok(new ApiResponse<SideBarDto>
            {
                Status = new StatusInfo
                {
                    Code = 200,
                    Message = "Success",
                    IsSuccess = true
                },
                Meta = new MetaInfo(HttpContext),
                Data = dto
            });
        }
    }
}
