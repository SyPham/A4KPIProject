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
        private readonly IGHRService _serviceGHR;

        public ReportController(
            IQ1Q3Service serviceQ1Q3,
            IH1H2Service serviceH1H2,
            IHQHRService serviceHQHR,
            IGHRService serviceGHR

            )
        {
            _serviceQ1Q3 = serviceQ1Q3;
            _serviceH1H2 = serviceH1H2;
            _serviceHQHR = serviceHQHR;
            _serviceGHR = serviceGHR;
        }

        [HttpGet]
        public async Task<ActionResult> GetGHRData()
        {
            return Ok(await _serviceGHR.GetData());
        }
        [HttpGet]
        public async Task<IActionResult> GetGHRReportH1Info(int accountId)
        {
            return Ok(await _serviceGHR.GetReportInfoH1(accountId));
        }
        [HttpGet]
        public async Task<IActionResult> GetGHRReportH2Info(int accountId)
        {
            return Ok(await _serviceGHR.GetReportInfoH2(accountId));
        }
        [HttpPut]
        public async Task<IActionResult> ReportUpdateComment(List<CommentUpdateDto> model)
        {
            var status = await _serviceGHR.ReportUpdateComment(model);
            return Ok(status);

        }
        [HttpPut]
        public async Task<IActionResult> ReportUpdateAtScore(List<AtScoreUpdateDto> model)
        {
            var status = await _serviceGHR.ReportUpdateAtScore(model);
            return Ok(status);

        }
        [HttpPut]
        public async Task<IActionResult> ReportUpdateSpeComment(List<CommentUpdateDto> model)
        {
            var status = await _serviceGHR.ReportUpdateSpeComment(model);
            return Ok(status);

        }
        [HttpGet]
        public async Task<ActionResult> GetQ1Q3Data()
        {
            return Ok(await _serviceQ1Q3.GetQ1Q3Data());
        }

        

        [HttpGet]
        public async Task<ActionResult> GetQ1Q3DataByLeo(DateTime currentTime)
        {
            return Ok(await _serviceQ1Q3.GetQ1Q3DataByLeo(currentTime));
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
        public async Task<IActionResult> ExportExcelByLeo(DateTime currentTime)
        {
            var bin = await _serviceQ1Q3.ExportExcelByLeo(currentTime);
            return File(bin, "application/octet-stream", "Q1,Q3 Report 季報表.xlsx");
        }
        [HttpGet]
        public async Task<IActionResult> GetQ1Q3ReportInfo(int accountId)
        {
            return Ok(await _serviceH1H2.GetReportInfo(accountId));
        }

        [HttpGet]
        public async Task<IActionResult> GetH1H2ReportInfo(int accountId)
        {
            return Ok(await _serviceH1H2.GetReportInfo(accountId));
        }

        

        [HttpGet("{accountId}")]
        public async Task<IActionResult> ExportH1H2Excel(int accountId)
        {
            var bin = await _serviceH1H2.ExportExcel(accountId);
            return File(bin, "application/octet-stream", "H1,H2 Report 季報表.xlsx");
        }
    }
}
