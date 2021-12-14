using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using A4KPI.DTO;
using A4KPI.Helpers;
using A4KPI._Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using A4KPI._Services.Interface;

namespace A4KPI.Controllers
{
    public class MeetingController : ApiControllerBase
    {
        private readonly IMeetingService _service;

        public MeetingController(IMeetingService service)
        {
            _service = service;
        }



        [HttpGet("{userId}")]
        public async Task<ActionResult> GetAllKPI(int userId)
        {
            return Ok(await _service.GetAllKPI(userId));
        }


        [HttpGet("{kpiId}/{currentTime}")]
        public async Task<ActionResult> GetChartWithDateTime(int kpiId, DateTime currentTime)
        {
            return Ok(await _service.GetChartWithDateTime(kpiId,currentTime));
        }

        

    }
}
