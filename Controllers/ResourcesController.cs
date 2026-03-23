using FlexFit.Models;
using FlexFit.UnitOfWorkLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlexFit.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResourcesController : ControllerBase
    {
        private readonly IUnitOfWork _uow;

        public ResourcesController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [HttpPost("create")]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> CreateResource([FromBody] FlexFit.Application.DTOs.CreateResourceDto dto)
        {
            await _uow.Resources.CreateResourceAsync(dto);
            return Ok(new { message = "Sprava uspešno dodata." });
        }
    }
}
