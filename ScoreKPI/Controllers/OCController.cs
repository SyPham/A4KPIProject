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
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class OCController : ApiControllerBase
    {
        private readonly IOCService _service;

        public OCController(IOCService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllAsync()
        {
            return Ok(await _service.GetAllAsync());
        }
        [HttpGet]
        public async Task<IActionResult> GetAllAsTreeView()
        {
            var ocs = await _service.GetAllAsTreeView();
            return Ok(ocs);
        }
        [HttpPost]
        public async Task<ActionResult> AddAsync([FromBody] OCDto model)
        {
            return StatusCodeResult(await _service.AddAsync(model));
        }

        [HttpGet("{OCname}")]
        public async Task<IActionResult> GetUserByOCname(string OCname)
        {
            var result = await _service.GetUserByOCname(OCname);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> MappingUserOC([FromBody]OcUserDto OcUserDto)
        {
            var result = await _service.MappingUserOC(OcUserDto);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> MappingRangeUserOC(OcUserDto OcUserDto)
        {
            var result = await _service.MappingRangeUserOC(OcUserDto);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> RemoveUserOC([FromBody]OcUserDto OcUserDto)
        {
            var result = await _service.RemoveUserOC(OcUserDto);
            return Ok(result);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateAsync([FromBody] OCDto model)
        {
            return StatusCodeResult(await _service.UpdateAsync(model));
        }

        [HttpDelete("{id}")]
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
