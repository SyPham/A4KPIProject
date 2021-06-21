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
    public class ProgressController : ApiControllerBase
    {
        private readonly IProgressService _service;

        public ProgressController(IProgressService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllAsync()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpPost]
        public async Task<ActionResult> AddAsync([FromBody] ProgressDto model)
        {
            return StatusCodeResult(await _service.AddAsync(model));
        }

        [HttpPut]
        public async Task<ActionResult> UpdateAsync([FromBody] ProgressDto model)
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
