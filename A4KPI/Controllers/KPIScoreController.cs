using Microsoft.AspNetCore.Mvc;
using A4KPI.DTO;
using A4KPI.Helpers;
using A4KPI.Services;
using System.Linq;
using System.Threading.Tasks;

namespace A4KPI.Controllers
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
        public async Task<ActionResult> GetFisrtSelfScoreL1ByAccountId(int accountId, int periodTypeId, int period, string scoreType)
        {
            return Ok(await _service.GetFisrtSelfScoreL1ByAccountId(accountId, periodTypeId, period, scoreType));
        }

        /// <summary>
        /// Lấy điểm L1 chấm cho L0 theo tháng, quý hoặc nửa năm. Sử dụng cho L2 modal
        /// </summary>
        /// <param name="accountId">L0</param>
        /// <param name="periodTypeId">Loại tháng, quý, nửa năm</param>
        /// <param name="period">Tháng, quý hoặc nửa năm</param>
        /// <param name="scoreType">Chấm điểm bởi L1, L2 hoặc GHR</param>
        /// <returns>Trả về điểm L1 chấm cho L0 theo tháng, quý hoặc nửa năm</returns>
        [HttpGet]
        public async Task<ActionResult> GetFisrtKPIScoreL1ByAccountId(int accountId, int periodTypeId, int period, string scoreType)
        {
            return Ok(await _service.GetFisrtKPIScoreL1ByAccountId(accountId, periodTypeId, period, scoreType));
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
