using BusyBee.API.DTOs;
using BusyBee.API.Services;
using BusyBee.DataAccess.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet]
        public async Task<ActionResult<SideBarDto>> Get()
        {
            var dto = await _service.GetSideBarAsync(); // ✅ Всё правильно
            return Ok(dto);
        }
    }
}
