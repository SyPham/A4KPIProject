using Microsoft.AspNetCore.Mvc;
using ScoreKPI.Controllers;
using ScoreKPI.DTO;
using ScoreKPI.Helpers;
using ScoreKPI.Services;
using System.Threading.Tasks;

namespace ScoreAttitude.Controllers
{
    public class AttitudeController : ApiControllerBase
    {
        private readonly IAttitudeService _service;

        public AttitudeController(IAttitudeService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllAsync()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpPost]
        public async Task<ActionResult> AddAsync([FromBody] AttitudeDto model)
        {
            return StatusCodeResult(await _service.AddAsync(model));
        }

        [HttpPut]
        public async Task<ActionResult> UpdateAsync([FromBody] AttitudeDto model)
        {
            return StatusCodeResult(await _service.UpdateAsync(model));
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            return StatusCodeResult(await _service.DeleteAsync(id));
        }

        [HttpGet]
        public async Task<ActionResult> GetByIdAsync(int id)
        {
            return Ok(await _service.GetByIdAsync(id));
        }

        [HttpGet]
        public async Task<ActionResult> GetWithPaginationsAsync(PaginationParams paramater)
        {
            return Ok(await _service.GetWithPaginationsAsync(paramater));
        }

    }
}
