using Microsoft.AspNetCore.Mvc;
using ScoreKPI.DTO;
using ScoreKPI.Helpers;
using ScoreKPI.Services;
using System.Linq;
using System.Threading.Tasks;

namespace ScoreKPI.Controllers
{
    public class KPIScoreController : ApiControllerBase
    {
        private readonly IKPIScoreService _service;

        public KPIScoreController(IKPIScoreService service)
        {
            _service = service;
        }
        /// <summary>
        /// Lấy điển đã chấm cho user
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> GetFisrtByAccountId(int accountId, int periodTypeId, int period, string scoreType)
        {
            return Ok(await _service.GetFisrtByAccountId(accountId, periodTypeId, period, scoreType));
        }
        [HttpGet]
        public async Task<ActionResult> GetFisrtSelfScoreByAccountId(int accountId, int periodTypeId, int period, string scoreType)
        {
            return Ok(await _service.GetFisrtSelfScoreByAccountId(accountId, periodTypeId, period, scoreType));
        }
        [HttpGet]
        public async Task<ActionResult> GetAllAsync()
        {
            return Ok((await _service.GetAllAsync()).OrderBy(x=>x.Point));
        }

        [HttpPost]
        public async Task<ActionResult> AddAsync([FromBody] KPIScoreDto model)
        {
            return StatusCodeResult(await _service.AddAsync(model));
        }

        [HttpPut]
        public async Task<ActionResult> UpdateAsync([FromBody] KPIScoreDto model)
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
