using Microsoft.AspNetCore.Mvc;
using A4KPI.DTO;
using A4KPI.Helpers;
using A4KPI._Services.Services;
using System.Threading.Tasks;
using System.Linq;

namespace A4KPI.Controllers
{
    public class KPINewController : ApiControllerBase
    {
        private readonly IKPINewService _service;

        public KPINewController(IKPINewService service)
        {
            _service = service;
        }
     

        [HttpGet]
        public async Task<IActionResult> GetListPic()
        {
            var result = await _service.GetListPic();
            return Ok(result);
        }
     
        [HttpGet("{lang}")]
        public async Task<IActionResult> GetAllType(string lang)
        {
            return Ok((await _service.GetAllType(lang)));
        }
      
        [HttpGet("{lang}")]
        public async Task<IActionResult> GetAllAsTreeView(string lang)
        {
            var ocs = await _service.GetAllAsTreeView(lang);
            return Ok(ocs);
        }

        [HttpGet("{lang}/{userId}")]
        public async Task<IActionResult> GetAllAsTreeView2nd3rd(string lang, int userId)
        {
            var ocs = await _service.GetAllAsTreeView2nd3rd(lang, userId);
            return Ok(ocs);
        }
        [HttpPost]
        public async Task<ActionResult> AddAsync([FromBody] KPINewDto model)
        {

            return StatusCodeResult(await _service.AddAsync(model));
        }

        [HttpPut]
        public async Task<ActionResult> UpdateAsync([FromBody] KPINewDto model)
        {
            return StatusCodeResult(await _service.UpdateAsync(model));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            return Ok(await _service.Delete(id));
        }

      

    }
}
