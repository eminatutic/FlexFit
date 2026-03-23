using FlexFit.Application.DTOs;
using FlexFit.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlexFit.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TimeSlotsController : ControllerBase
    {
        private readonly ITimeSlotRepository _repository;

        public TimeSlotsController(ITimeSlotRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("{resourceId}")]
        public async Task<IActionResult> GetTimeSlots(int resourceId)
        {
            try {
                var result = await _repository.GetTimeSlotsAsync(resourceId);
                return Ok(result);
            } catch (Exception ex) {
                return StatusCode(500, new { error = ex.Message, inner = ex.InnerException?.Message, stackTrace = ex.StackTrace });
            }
        }

        [HttpPost]
        // [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> CreateTimeSlot([FromBody] TimeSlotDto dto)
        {
            try {
                var id = await _repository.CreateTimeSlotAsync(dto);
                return Ok(new { message = "Predloženi termin uspešno dodat.", id = id });
            } catch (ArgumentException ex) {
                return BadRequest(ex.Message);
            } catch (Exception ex) {
                return StatusCode(500, new { error = ex.Message, details = ex.InnerException?.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> DeleteTimeSlot(int id)
        {
            var success = await _repository.DeleteTimeSlotAsync(id);
            if (!success)
                return NotFound("Termin nije pronađen.");

            return Ok(new { message = "Termin uspešno obrisan." });
        }
    }
}
