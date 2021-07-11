using Microsoft.AspNetCore.Mvc;
using A4KPI.DTO;
using A4KPI.Helpers;
using A4KPI.Services;
using System.Threading.Tasks;

namespace A4KPI.Controllers
{
    public class CommentController : ApiControllerBase
    {
        private readonly ICommentService _service;

        public CommentController(ICommentService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<ActionResult> GetFisrtByAccountId(int accountId, int periodTypeId, int period, string scoreType)
        {
            return Ok(await _service.GetFisrtByAccountId(accountId, periodTypeId, period, scoreType));
        }
        [HttpGet]
        public async Task<ActionResult> GetAllAsync()
        {
            return Ok(await _service.GetAllAsync());
        }
     
        [HttpPost]
        public async Task<ActionResult> AddAsync([FromBody] CommentDto model)
        {
            return StatusCodeResult(await _service.AddAsync(model));
        }

        [HttpPut]
        public async Task<ActionResult> UpdateAsync([FromBody] CommentDto model)
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
        public async Task<ActionResult> GetAllByObjectiveId(int objectiveId)
        {
            return Ok(await _service.GetAllByObjectiveId(objectiveId));
        }
        [HttpGet]
        public async Task<ActionResult> GetWithPaginationsAsync(PaginationParams paramater)
        {
            return Ok(await _service.GetWithPaginationsAsync(paramater));
        }
        [HttpGet]
        public async Task<ActionResult> GetFunctionalLeaderCommentByAccountId(int accountId, int periodTypeId, int period)
        {
            return Ok(await _service.GetFunctionalLeaderCommentByAccountId(accountId, periodTypeId, period));
        }
        [HttpGet]
        public async Task<ActionResult> GetGHRCommentByAccountId(int accountId, int periodTypeId, int period)
        {
            return Ok(await _service.GetGHRCommentByAccountId(accountId, periodTypeId, period));
        }
    }
}
