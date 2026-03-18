using FlexFit.Application.Commands;
using FlexFit.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlexFit.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Employee,Admin")]
    public class GuardController : ControllerBase
    {
        private readonly IMediator _mediator;

        public GuardController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("sell-daily-ticket")]
        public async Task<IActionResult> SellDailyTicket([FromBody] SellDailyTicketDto dto)
        {
            var result = await _mediator.Send(new SellDailyTicketCommand(dto));

            if (!result)
            {
                return BadRequest(new { message = "Invalid or already active daily ticket." });
            }

            return Ok(new { message = "Daily ticket sold successfully." });
        }
    }
}
