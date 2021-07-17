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
    public class ReportController : ApiControllerBase
    {
        private readonly IQ1Q3Service _serviceQ1Q3;
        private readonly IH1H2Service _serviceH1H2;
        private readonly IHQHRService _serviceHQHR;

        public ReportController(
            IQ1Q3Service serviceQ1Q3,
            IH1H2Service serviceH1H2,
            IHQHRService serviceHQHR

            )
        {
            _serviceQ1Q3 = serviceQ1Q3;
            _serviceH1H2 = serviceH1H2;
            _serviceHQHR = serviceHQHR;
        }

        [HttpGet]
        public async Task<ActionResult> GetQ1Q3Data()
        {
            return Ok(await _serviceQ1Q3.GetQ1Q3Data());
        }

        [HttpGet]
        public async Task<ActionResult> GetQ1Q3DataByLeo()
        {
            return Ok(await _serviceQ1Q3.GetQ1Q3DataByLeo());
        }

        [HttpGet]
        public async Task<ActionResult> GetH1H2Data()
        {
            return Ok(await _serviceH1H2.GetH1H2Data());
        }

        [HttpGet]
        public async Task<ActionResult> GetHQHRData()
        {
            return Ok(await _serviceHQHR.GetHQHRData());
        }
        [HttpGet("{accountId}")]
        public async Task<IActionResult> ExportExcel(int accountId)
        {
            var bin = await _serviceQ1Q3.ExportExcel(accountId);
            return File(bin, "application/octet-stream", "Q1,Q3 Report 季報表.xlsx");
        }

        [HttpGet]
        public async Task<IActionResult> ExportExcelByLeo()
        {
            var bin = await _serviceQ1Q3.ExportExcelByLeo();
            return File(bin, "application/octet-stream", "Q1,Q3 Report 季報表.xlsx");
        }
        [HttpGet]
        public async Task<IActionResult> GetQ1Q3ReportInfo(int accountId)
        {
            return Ok(await _serviceQ1Q3.GetReportInfo(accountId));
        }
    }
}
