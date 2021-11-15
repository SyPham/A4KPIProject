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
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class OCPolicyController : ApiControllerBase
    {
        private readonly IOCPolicyService _service;

        public OCPolicyController(IOCPolicyService service)
        {
            _service = service;
        }

        [HttpDelete("{id}")]
        public async Task<bool> DeletePolicy(int id)
        {
            var data = await _service.DeletePolicy(id);
            return data;
        }
       

        [HttpGet]
        public async Task<ActionResult> GetAllPolicy()
        {
            return Ok(await _service.GetAllPolicy());
        }

        [HttpGet]
        public async Task<ActionResult> GetAllLevel3()
        {
            var oc = await _service.GetAllLevel3();
            return Ok(oc);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllAsTreeView()
        {
            var ocs = await _service.GetAllAsTreeView();
            return Ok(ocs);
        }

       
        [HttpGet("{ocID}")]
        public async Task<IActionResult> GetUserByOcID(int ocID)
        {
            var result = await _service.GetUserByOcID(ocID);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> MappingPolicyOC([FromBody]OCPolicyDto Dto)
        {
            var result = await _service.MappingPolicyOC(Dto);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> RemovePolicyOC([FromBody] OCPolicyDto Dto)
        {
            var result = await _service.RemovePolicyOC(Dto);
            return Ok(result);
        }


        [HttpPost]
        public async Task<IActionResult> MappingRangeUserOC(OCAccountDto OcUserDto)
        {
            var result = await _service.MappingRangeUserOC(OcUserDto);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> RemoveUserOC([FromBody]OCAccountDto OcUserDto)
        {
            var result = await _service.RemoveUserOC(OcUserDto);
            return Ok(result);
        }

        

        

    }
}
