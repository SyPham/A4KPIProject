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
    public class PeriodController : ApiControllerBase
    {
        private readonly IPeriodService _service;

        public PeriodController(IPeriodService service)
        {
            _service = service;
        }

      
        [HttpGet]
        public async Task<ActionResult> GetAllByPeriodTypeIdAsync(int periodTypeId)
        {
            return Ok(await _service.GetAllByPeriodTypeIdAsync(periodTypeId));
        }
       

    }
}
