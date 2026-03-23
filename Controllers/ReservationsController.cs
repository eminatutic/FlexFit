using FlexFit.Models;
using FlexFit.UnitOfWorkLayer;
using FlexFit.Application.DTOs;
using FlexFit.Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlexFit.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationsController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        private readonly IMediator _mediator;

        public ReservationsController(IUnitOfWork uow, IMediator mediator)
        {
            _uow = uow;
            _mediator = mediator;
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
            var reservation = await _uow.Reservations.GetByIdAsync(id);
            if (reservation == null) return NotFound(new { message = "Rezervacija nije pronađena." });

            reservation.Status = ReservationStatus.NoShow;
            await _uow.Reservations.UpdateAsync(reservation);
            await _uow.SaveAsync();

            var result = await _mediator.Send(new ProcessNoShowPenaltyCommand(id));
            if (!result) return BadRequest(new { message = "Greška pri dodeljivanju kaznenog poena." });

            return Ok(new { message = "Rezervacija označena kao 'NoShow' i kazneni poen je uspešno upisan." });
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
