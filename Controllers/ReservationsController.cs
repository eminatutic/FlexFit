using FlexFit.Models;
using FlexFit.UnitOfWorkLayer;
using FlexFit.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlexFit.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationsController : ControllerBase
    {
        private readonly IUnitOfWork _uow;

        public ReservationsController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [HttpPost("book")]
        [Authorize(Roles = "Member,Admin,Employee")]
        public async Task<IActionResult> BookResource([FromBody] ReservationDto dto)
        {
            var result = await _uow.Reservations.BookResourceAsync(dto);

            if (!result.isSuccess)
            {
                return BadRequest(new { message = result.message });
            }
            
            return Ok(new { message = result.message });
        }

        [HttpPost("mark-no-show/{id}")]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> MarkNoShow(int id)
        {
            // Dummy logic to simulate penalizing no show.
            // In a real application, you'd fetch the reservation, assign the penalty point, and save.
            return Ok(new { message = "Kazneni poen je uspešno upisan zbog nepojavljivanja." });
        }

        [HttpDelete("cancel/{id}")]
        [Authorize(Roles = "Member,Admin,Employee")]
        public async Task<IActionResult> CancelReservation(int id)
        {
            var reservation = await _uow.Reservations.GetByIdAsync(id);
            if (reservation == null)
            {
                return NotFound(new { message = "Rezervacija nije pronađena." });
            }

            await _uow.Reservations.DeleteAsync(reservation);
            return Ok(new { message = "Rezervacija je uspešno otkazana." });
        }

        [HttpGet("resource/{resourceId}")]
        public async Task<IActionResult> GetResourceReservations(int resourceId)
        {
            var reservations = await _uow.Reservations.FindAsync(r => r.ResourceId == resourceId);
            var now = DateTime.UtcNow;

            var result = reservations.Select(r => new {
                id = r.Id,
                memberId = r.MemberId,
                startTime = r.StartTime,
                endTime = r.EndTime,
                status = r.Status,
                isExpired = r.EndTime <= now
            }).ToList();

            return Ok(result);
        }
    }
}
