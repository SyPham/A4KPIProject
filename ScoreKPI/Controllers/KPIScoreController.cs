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
        [HttpGet]
        public async Task<ActionResult> GetFisrtByAccountId(int scoreBy)
        {
            return Ok(await _service.GetFisrtByAccountId(scoreBy));
        }
        [HttpGet]
        public async Task<ActionResult> GetAllAsync()
        {
            return Ok((await _service.GetAllAsync()).OrderBy(x=>x.Point));
        }

        [HttpPost]
        public async Task<ActionResult> AddAsync([FromBody] KPIScoreDto model)
        {
            var accessToken = HttpContext.Request.Headers["Authorization"];
            int accountID = JWTExtensions.GetDecodeTokenById(accessToken);
            model.AccountId = accountID;
            return StatusCodeResult(await _service.AddAsync(model));
        }

        [HttpPut]
        public async Task<ActionResult> UpdateAsync([FromBody] KPIScoreDto model)
        {
            var accessToken = HttpContext.Request.Headers["Authorization"];
            int accountID = JWTExtensions.GetDecodeTokenById(accessToken);
            model.AccountId = accountID;
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
