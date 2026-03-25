using FlexFit.Domain.Models;
using FlexFit.Infrastructure.UnitOfWorkLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlexFit.Presentation.Controllers
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
            return Ok(new { message = "Sprava uspesno dodata." });
        }

        [HttpPost("update-status")]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> UpdateStatus([FromBody] FlexFit.Application.DTOs.UpdateResourceStatusDto dto)
        {
            var resource = await _uow.Resources.GetByIdAsync(dto.ResourceId);
            if (resource == null) return NotFound(new { message = "Sprava nije pronadjena." });

            resource.Status = (FlexFit.Domain.Models.ResourceStatus)dto.Status;
            await _uow.Resources.UpdateAsync(resource);
            return Ok(new { message = "Status sprave uspesno azuriran." });
        }
    }
}
