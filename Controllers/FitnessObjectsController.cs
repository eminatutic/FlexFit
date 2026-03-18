using FlexFit.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FlexFit.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FitnessObjectsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public FitnessObjectsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllFitnessObjectsQuery());
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetFitnessObjectByIdQuery(id));
            if (result == null) return NotFound();
            return Ok(result);
        }
    }
}
