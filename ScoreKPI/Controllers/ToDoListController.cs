using Microsoft.AspNetCore.Mvc;
using ScoreKPI.DTO;
using ScoreKPI.Helpers;
using ScoreKPI.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ScoreKPI.Controllers
{
    public class ToDoListController : ApiControllerBase
    {
        private readonly IToDoListService _service;

        public ToDoListController(IToDoListService service)
        {
            _service = service;
        }
        /// <summary>
        /// Lấy danh sách cho KPI Score
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns>Danh sách cho KPI Score</returns>
        [HttpGet]
        public async Task<ActionResult> GetAllInCurrentQuarterByAccountGroup(int accountId)
        {
            var accessToken = HttpContext.Request.Headers["Authorization"];
            int accountID = JWTExtensions.GetDecodeTokenById(accessToken);
            return Ok(await _service.GetAllInCurrentQuarterByAccountGroup(accountId));
        }
        [HttpGet]
        public async Task<ActionResult> GetAllKPIScoreByAccountId()
        {
            var accessToken = HttpContext.Request.Headers["Authorization"];
            int accountId = JWTExtensions.GetDecodeTokenById(accessToken);
            return Ok(await _service.GetAllKPIScoreByAccountId(accountId));
        }
        [HttpGet]
        public async Task<ActionResult> GetAllKPISelfScoreByObjectiveId(int objectiveId)
        {
            var accessToken = HttpContext.Request.Headers["Authorization"];
            int accountId = JWTExtensions.GetDecodeTokenById(accessToken);
            return Ok(await _service.GetAllKPISelfScoreByObjectiveId(objectiveId, accountId));
        }
        [HttpGet]
        public async Task<ActionResult> L0()
        {
            var accessToken = HttpContext.Request.Headers["Authorization"];
            int accountId = JWTExtensions.GetDecodeTokenById(accessToken);
            return Ok(await _service.L0(accountId));
        }
        [HttpGet]
        public async Task<ActionResult> L1()
        {
            var accessToken = HttpContext.Request.Headers["Authorization"];
            int accountId = JWTExtensions.GetDecodeTokenById(accessToken);
            return Ok(await _service.L1(accountId));
        }
        [HttpGet]
        public async Task<ActionResult> GetAllObjectiveByL1L2()
        {
            return Ok(await _service.GetAllObjectiveByL1L2());
        }
        [HttpGet]
        public async Task<ActionResult> GetAllAsync()
        {
            return Ok(await _service.GetAllAsync());
        }
        [HttpGet]
        public async Task<ActionResult> GetAllByObjectiveIdAsync(int objectiveId)
        {
            return Ok(await _service.GetAllByObjectiveIdAsync(objectiveId));
        }
        [HttpGet]
        public async Task<ActionResult> GetAllInCurrentQuarterByObjectiveIdAsync(int objectiveId)
        {
            return Ok(await _service.GetAllInCurrentQuarterByObjectiveIdAsync(objectiveId));
        }
        [HttpPost]
        public async Task<ActionResult> AddAsync([FromBody] ToDoListDto model)
        {
            var accessToken = HttpContext.Request.Headers["Authorization"];
            int accountId = JWTExtensions.GetDecodeTokenById(accessToken);

            model.CreatedBy = accountId;
            return StatusCodeResult(await _service.AddAsync(model));
        }

        [HttpPut]
        public async Task<ActionResult> UpdateAsync([FromBody] ToDoListDto model)
        {
            var accessToken = HttpContext.Request.Headers["Authorization"];
            int accountId = JWTExtensions.GetDecodeTokenById(accessToken);

            model.ModifiedBy = accountId;
            return StatusCodeResult(await _service.UpdateAsync(model));
        }

        [HttpPost]
        public async Task<ActionResult> AddRangeAsync([FromBody] List<ToDoListDto> model)
        {
            var accessToken = HttpContext.Request.Headers["Authorization"];
            int accountId = JWTExtensions.GetDecodeTokenById(accessToken);
            foreach (var item in model)
            {
                item.CreatedBy = accountId;
            }
            return StatusCodeResult(await _service.AddRangeAsync(model));
        }

        [HttpPut]
        public async Task<ActionResult> UpdateRangeAsync([FromBody] List<ToDoListDto> model)
        {
            var accessToken = HttpContext.Request.Headers["Authorization"];
            int accountId = JWTExtensions.GetDecodeTokenById(accessToken);
            foreach (var item in model)
            {
                item.CreatedBy = accountId;
            }
            return StatusCodeResult(await _service.UpdateRangeAsync(model));
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
