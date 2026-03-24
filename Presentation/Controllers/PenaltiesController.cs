using FlexFit.Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using FlexFit.Infrastructure.UnitOfWorkLayer;
using FlexFit.Domain.Models;

namespace FlexFit.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PenaltiesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IUnitOfWork _uow;

        public PenaltiesController(IMediator mediator, IUnitOfWork uow)
        {
            _mediator = mediator;
            _uow = uow;
        }

        [HttpGet("cards")]
        public async Task<IActionResult> GetAllCards()
        {
            var cards = await _uow.PenaltyCards.GetAllAsync();
            return Ok(cards);
        }

        [HttpGet("points")]
        public async Task<IActionResult> GetAllPoints()
        {
            var points = await _uow.PenaltyPoints.GetAllAsync();
            return Ok(points);
        }

        [HttpPost("cards")]
        public async Task<IActionResult> CreateCard([FromBody] CreatePenaltyCardCommand command)
        {
            var success = await _mediator.Send(command);
            if (!success) return BadRequest(new { message = "ÄŒlan je veÄ‡ dobio kaznenu kartu u ovom objektu u poslednjih 12h." });
            return Ok(new { message = "Kaznena karta uspeÅ¡no izdata." });
        }

        [HttpPost("points")]
        public async Task<IActionResult> CreatePoint([FromBody] CreatePenaltyPointCommand command)
        {
            var success = await _mediator.Send(command);
            if (!success) return BadRequest(new { message = "GreÅ¡ka pri izdavanju kaznenog poena." });
            return Ok(new { message = "Kazneni poen uspeÅ¡no dodat." });
        }

        public class CancelDto { public string Type { get; set; } public string Reason { get; set; } }

        [HttpPut("{id}/cancel")]
        public async Task<IActionResult> Cancel(int id, [FromBody] CancelDto dto)
        {
            var success = await _mediator.Send(new CancelPenaltyCommand(id, dto.Type, dto.Reason));
            if (!success) return NotFound(new { message = "Kazna nije pronaÄ‘ena ili je veÄ‡ stornirana." });
            return Ok(new { message = "UspeÅ¡no stornirano uz napomenu." });
        }

        [HttpPost("{id}/pay")]
        public async Task<IActionResult> Pay(int id)
        {
            var success = await _mediator.Send(new PayPenaltyCommand(id));
            if (!success) return BadRequest(new { message = "Kazna nije pronaÄ‘ena, veÄ‡ je plaÄ‡ena ili je stornirana." });
            return Ok(new { message = "Kazna je uspeÅ¡no plaÄ‡ena." });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, [FromQuery] string type)
        {
            var success = await _mediator.Send(new DeletePenaltyCommand(id, type));
            if (!success) return NotFound(new { message = "Kazna nije pronaÄ‘ena." });
            return Ok(new { message = "UspeÅ¡no obrisano." });
        }
    }
}
