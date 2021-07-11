using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using A4KPI.DTO;
using A4KPI.Helpers;
using A4KPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A4KPI.Controllers
{
    public class ResultOfMonthController : ApiControllerBase
    {
        private readonly IResultOfMonthService _service;

        public ResultOfMonthController(IResultOfMonthService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllAsync()
        {
            return Ok(await _service.GetAllAsync());
        }
        [HttpPut]
        public async Task<ActionResult> UpdateResultOfMonthAsync([FromBody] ResultOfMonthRequestDto model)
        {
            return StatusCodeResult(await _service.UpdateResultOfMonthAsync(model));
        }
        [HttpPost]
        public async Task<ActionResult> AddAsync([FromBody] ResultOfMonthDto model)
        {
            return StatusCodeResult(await _service.AddAsync(model));
        }

        [HttpPut]
        public async Task<ActionResult> UpdateAsync([FromBody] ResultOfMonthDto model)
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
        public async Task<ActionResult> GetAllByMonth(int objectiveId)
        {
            return Ok(await _service.GetAllByMonth(objectiveId));
        }
        [HttpGet]
        public async Task<ActionResult> GetWithPaginationsAsync(PaginationParams paramater)
        {
            return Ok(await _service.GetWithPaginationsAsync(paramater));
        }

    }
}
