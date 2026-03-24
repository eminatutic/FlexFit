using FlexFit.Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FlexFit.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GraphTestController : ControllerBase
    {
        private readonly IMemberGraphRepository _graphRepo;

        public GraphTestController(IMemberGraphRepository graphRepo)
        {
            _graphRepo = graphRepo;
        }

        [HttpGet("recommend/objects/{id}")]
        public async Task<IActionResult> RecommendObjects(string id)
        {
            var objects = await _graphRepo.GetRecommendedObjectsAsync(id);
            return Ok(objects);
        }
    }
}
