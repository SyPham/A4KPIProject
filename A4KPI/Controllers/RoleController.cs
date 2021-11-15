using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using A4KPI.DTO;
using A4KPI.Helpers;
using A4KPI._Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A4KPI.Controllers
{
    public class RoleController : ApiControllerBase
    {
        private readonly IRoleService _service;

        public RoleController(IRoleService service)
        {
            _service = service;
        }
        [HttpGet("{userID}")]
        public async Task<IActionResult> GetRoleByUserID(int userID)
        {
            var result = await _service.GetRoleByUserID(userID);
            return Ok(result);
        }
        [HttpPut("{userID}/{roleID}")]
        public async Task<IActionResult> MapUserRole(int userID, int roleID)
        {
            var result = await _service.MapUserRole(userID, roleID);
            return Ok(result);
        }
        [HttpGet]
        public async Task<ActionResult> GetAllAsync()
        {
            return Ok(await _service.GetAllAsync());
        }
        [HttpGet]
        public async Task<ActionResult> GetAccounts()
        {
            return Ok(await _service.GetAccounts());
        }

        [HttpPost]
        public async Task<ActionResult> AddAsync(RoleDto model)
        {
            return StatusCodeResult(await _service.AddAsync(model));
        }

        [HttpPut]
        public async Task<ActionResult> UpdateAsync(RoleDto model)
        {
            return StatusCodeResult(await _service.UpdateAsync(model));
        }
        [HttpPut]
        public async Task<ActionResult> LockAsync(int id)
        {
            return StatusCodeResult(await _service.LockAsync(id));
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
