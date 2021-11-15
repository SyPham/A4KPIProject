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
    public class MeetingController : ApiControllerBase
    {
        private readonly IMeetingService _service;

        public MeetingController(IMeetingService service)
        {
            _service = service;
        }



        [HttpGet]
        public async Task<ActionResult> GetAllKPI()
        {
            return Ok(await _service.GetAllKPI());
        }

        [HttpGet("{levelId}/{picId}")]
        public async Task<ActionResult> GetAllKPIByPicAndLevel(int levelId , int picId)
        {
            return Ok(await _service.GetAllKPIByPicAndLevel(levelId, picId));
        }

        [HttpGet("{kpiId}")]
        public async Task<ActionResult> GetChart(int kpiId)
        {
            return Ok(await _service.GetChart(kpiId));
        }

        [HttpGet("{kpiId}/{currentTime}")]
        public async Task<ActionResult> GetChartWithDateTime(int kpiId, DateTime currentTime)
        {
            return Ok(await _service.GetChartWithDateTime(kpiId,currentTime));
        }

        [HttpGet("{kpiId}")]
        public async Task<ActionResult> GetDataTable(int kpiId)
        {
            return Ok(await _service.GetDataTable(kpiId));
        }

        

    }
}
