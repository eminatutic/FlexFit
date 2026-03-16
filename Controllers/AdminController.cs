using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FlexFit.UnitOfWorkLayer;
using FlexFit.Models;

namespace FlexFit.Controllers
{
    [ApiController]
    [Route("api/admin")]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public AdminController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // POST: api/admin/fitness-object
        [HttpPost("fitness-object")]
        public async Task<IActionResult> CreateFitnessObject([FromBody] FitnessObject obj)
        {
            await _unitOfWork.FitnessObjects.AddAsync(obj);
            await _unitOfWork.SaveAsync();
            return Ok(obj);
        }

        // GET: api/admin/fitness-object/{id}
        [HttpGet("fitness-object/{id}")]
        public async Task<IActionResult> GetFitnessObject(int id)
        {
            var obj = await _unitOfWork.FitnessObjects.GetByIdAsync(id);
            if (obj == null) return NotFound();
            return Ok(obj);
        }

        // PUT: api/admin/fitness-object/{id}
        [HttpPut("fitness-object/{id}")]
        public async Task<IActionResult> UpdateFitnessObject(int id, [FromBody] FitnessObject obj)
        {
            var existing = await _unitOfWork.FitnessObjects.GetByIdAsync(id);
            if (existing == null) return NotFound();

            existing.Name = obj.Name;
            existing.City = obj.City;
            existing.Address = obj.Address;
            existing.Capacity = obj.Capacity;
            existing.WorkingHours = obj.WorkingHours;

            await _unitOfWork.FitnessObjects.UpdateAsync(existing);
            await _unitOfWork.SaveAsync();

            return Ok(existing);
        }

        // DELETE: api/admin/fitness-object/{id}
        [HttpDelete("fitness-object/{id}")]
        public async Task<IActionResult> DeleteFitnessObject(int id)
        {
            var existing = await _unitOfWork.FitnessObjects.GetByIdAsync(id);
            if (existing == null) return NotFound();

            await _unitOfWork.FitnessObjects.DeleteAsync(existing);
            await _unitOfWork.SaveAsync();

            return Ok();
        }
    }
}