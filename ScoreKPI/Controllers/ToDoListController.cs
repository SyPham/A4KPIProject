using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ScoreKPI.DTO;
using ScoreKPI.Helpers;
using ScoreKPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
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
            return StatusCodeResult(await _service.AddAsync(model));
        }

        [HttpPut]
        public async Task<ActionResult> UpdateAsync([FromBody] ToDoListDto model)
        {
            return StatusCodeResult(await _service.UpdateAsync(model));
        }

        [HttpPost]
        public async Task<ActionResult> AddRangeAsync([FromBody] List<ToDoListDto> model)
        {
            return StatusCodeResult(await _service.AddRangeAsync(model));
        }

        [HttpPut]
        public async Task<ActionResult> UpdateRangeAsync([FromBody] List<ToDoListDto> model)
        {
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
