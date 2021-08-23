using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetUtility;
using OfficeOpenXml;
using A4KPI.DTO;
using A4KPI.Helpers;
using A4KPI.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace A4KPI.Controllers
{
    public class ToDoListController : ApiControllerBase
    {
        private readonly IToDoListService _service;
        private readonly IAccountService _serviceAccount;
        private readonly IObjectiveService _serviceObjective;

        public ToDoListController(IToDoListService service,
            IAccountService serviceAccount,
            IObjectiveService serviceObjective)
        {
            _service = service;
            _serviceAccount = serviceAccount;
            _serviceObjective = serviceObjective;
        }
    
        [HttpGet]
        public async Task<ActionResult> GetAllKPIScoreByAccountId()
        {
            var accessToken = HttpContext.Request.Headers["Authorization"];
            int accountId = JWTExtensions.GetDecodeTokenById(accessToken);
            return Ok(await _service.GetAllKPIScoreByAccountId(accountId));
        }
        [HttpGet]
        public async Task<ActionResult> GetAllKPISelfScoreByObjectiveId(int objectiveId)
        {
            var accessToken = HttpContext.Request.Headers["Authorization"];
            int accountId = JWTExtensions.GetDecodeTokenById(accessToken);
            return Ok(await _service.GetAllKPISelfScoreByObjectiveId(objectiveId, accountId));
        }
        [HttpGet]
        public async Task<ActionResult> GetAllAttitudeScoreByFunctionalLeader()
        {
            return Ok(await _service.GetAllAttitudeScoreByFunctionalLeader());
        }

        [HttpGet]
        public async Task<ActionResult> GetAllKPIScoreL0ByPeriod(int period)
        {

            return Ok(await _service.GetAllKPIScoreL0ByPeriod(period));
        }
        
        [HttpGet]
        public async Task<ActionResult> GetAllKPIScoreGHRByAccountId(int accountId, DateTime currentTime)
        {

            return Ok(await _service.GetAllKPIScoreGHRByAccountId(accountId, currentTime));
        }
        [HttpGet]
        public async Task<ActionResult> GetAllKPIScoreL1ByAccountId(int accountId, DateTime currentTime)
        {
         
            return Ok(await _service.GetAllKPIScoreL1ByAccountId(accountId, currentTime));
        }
        [HttpGet]
        public async Task<ActionResult> GetAllKPIScoreL2ByAccountId(int accountId, DateTime currentTime)
        {

            return Ok(await _service.GetAllKPIScoreL2ByAccountId(accountId, currentTime));
        }

        [HttpGet]
        public async Task<ActionResult> GetAllAttitudeScoreL1ByAccountId(int accountId)
        {

            return Ok(await _service.GetAllAttitudeScoreL1ByAccountId(accountId));
        }
        [HttpGet]
        public async Task<ActionResult> GetAllAttitudeScoreL2ByAccountId(int accountId)
        {

            return Ok(await _service.GetAllAttitudeScoreL2ByAccountId(accountId));
        }
        [HttpGet]
        public async Task<ActionResult> GetAllAttitudeScoreGFLByAccountId(int accountId)
        {

            return Ok(await _service.GetAllAttitudeScoreGFLByAccountId(accountId));
        }
        [HttpGet]
        public async Task<ActionResult> GetQuarterlySetting()
        {
            return Ok(await _service.GetQuarterlySetting());
        }
        [HttpGet]
        public async Task<ActionResult> L0( DateTime currentTime)
        {
            var accessToken = HttpContext.Request.Headers["Authorization"];
            int accountId = JWTExtensions.GetDecodeTokenById(accessToken);
            return Ok(await _service.L0(accountId, currentTime));
        }
        [HttpGet]
        public async Task<ActionResult> L1(DateTime currentTime)
        {
            var accessToken = HttpContext.Request.Headers["Authorization"];
            int accountId = JWTExtensions.GetDecodeTokenById(accessToken);
            return Ok(await _service.L1(accountId, currentTime));
        }
        [HttpGet]
        public async Task<ActionResult> FunctionalLeader(DateTime currentTime)
        {
            var accessToken = HttpContext.Request.Headers["Authorization"];
            int accountId = JWTExtensions.GetDecodeTokenById(accessToken);
            return Ok(await _service.FunctionalLeader(accountId, currentTime));
        }
        [HttpGet]
        public async Task<ActionResult> Updater(DateTime currentTime)
        {
            var accessToken = HttpContext.Request.Headers["Authorization"];
            int accountId = JWTExtensions.GetDecodeTokenById(accessToken);
            return Ok(await _service.Updater(accountId, currentTime));
        }
        [HttpGet]
        public async Task<ActionResult> L2(DateTime currentTime)
        {
            var accessToken = HttpContext.Request.Headers["Authorization"];
            int accountId = JWTExtensions.GetDecodeTokenById(accessToken);
            return Ok(await _service.L2(accountId, currentTime));
        }
        [HttpGet]
        public async Task<ActionResult> FHO()
        {
            var accessToken = HttpContext.Request.Headers["Authorization"];
            int accountId = JWTExtensions.GetDecodeTokenById(accessToken);
            return Ok(await _service.FHO(accountId));
        }
        [HttpGet]
        public async Task<ActionResult> GHR(DateTime currentTime)
        {
            var accessToken = HttpContext.Request.Headers["Authorization"];
            int accountId = JWTExtensions.GetDecodeTokenById(accessToken);
            return Ok(await _service.GHR(accountId, currentTime));
        }
        [HttpGet]
        public async Task<ActionResult> GM(DateTime currentTime)
        {
            var accessToken = HttpContext.Request.Headers["Authorization"];
            int accountId = JWTExtensions.GetDecodeTokenById(accessToken);
            return Ok(await _service.GM(accountId, currentTime));
        }
        [HttpGet]
        public async Task<ActionResult> GetAllObjectiveByL1L2()
        {
            return Ok(await _service.GetAllObjectiveByL1L2());
        }
        [HttpGet]
        public async Task<ActionResult> GetAllAsync()
        {
            return Ok(await _service.GetAllAsync());
        }
        [HttpGet]
        public async Task<ActionResult> GetAllByObjectiveIdAsync(int objectiveId)
        {
            return Ok(await _service.GetAllByObjectiveIdAsync(objectiveId));
        }
        [HttpGet]
        public async Task<ActionResult> GetAllByObjectiveIdAsTreeAsync(int objectiveId)
        {
            return Ok(await _service.GetAllByObjectiveIdAsTreeAsync(objectiveId));
        }
        [HttpGet]
        public async Task<ActionResult> GetAllInCurrentQuarterByObjectiveIdAsync(int objectiveId)
        {
            return Ok(await _service.GetAllInCurrentQuarterByObjectiveIdAsync(objectiveId));
        }
        [HttpPost]
        public async Task<ActionResult> AddAsync([FromBody] ToDoListDto model)
        {
            var accessToken = HttpContext.Request.Headers["Authorization"];
            int accountId = JWTExtensions.GetDecodeTokenById(accessToken);

            model.CreatedBy = accountId;
            return StatusCodeResult(await _service.AddAsync(model));
        }

        [HttpPut]
        public async Task<ActionResult> UpdateAsync([FromBody] ToDoListDto model)
        {
            var accessToken = HttpContext.Request.Headers["Authorization"];
            int accountId = JWTExtensions.GetDecodeTokenById(accessToken);

            model.ModifiedBy = accountId;
            return StatusCodeResult(await _service.UpdateAsync(model));
        }

        [HttpPost]
        public async Task<ActionResult> AddRangeAsync([FromBody] List<ToDoListDto> model)
        {
            var accessToken = HttpContext.Request.Headers["Authorization"];
            int accountId = JWTExtensions.GetDecodeTokenById(accessToken);
            foreach (var item in model)
            {
                item.CreatedBy = accountId;
            }
            return StatusCodeResult(await _service.AddRangeAsync(model));
        }

        [HttpPut]
        public async Task<ActionResult> UpdateRangeAsync([FromBody] List<ToDoListDto> model)
        {
            var accessToken = HttpContext.Request.Headers["Authorization"];
            int accountId = JWTExtensions.GetDecodeTokenById(accessToken);
            foreach (var item in model)
            {
                item.CreatedBy = accountId;
            }
            return StatusCodeResult(await _service.UpdateRangeAsync(model));
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
        [HttpPost]
        public async Task<ActionResult> Import([FromForm] IFormFile file2)
        {
            IFormFile file = Request.Form.Files["UploadedFile"];
            object uploadBy = Request.Form["UploadBy"];
            var datasList = new List<ImportExcelFHO>();
            //var datasList2 = new List<UploadDataVM2>();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            if ((file != null) && (file.Length > 0) && !string.IsNullOrEmpty(file.FileName))
            {
                string fileName = file.FileName;
                int userid = uploadBy.ToInt();
                using (var package = new ExcelPackage(file.OpenReadStream()))
                {
                    var currentSheet = package.Workbook.Worksheets;
                    var workSheet = currentSheet.First();
                    var noOfCol = workSheet.Dimension.Columns;
                    var noOfRow = workSheet.Dimension.Rows;

                    for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                    {
                        datasList.Add(new ImportExcelFHO()
                        {
                            KPIObjective = workSheet.Cells[rowIterator, 1].Value.ToSafetyString(),
                            UserList = workSheet.Cells[rowIterator, 2].Value.ToSafetyString(),
                        });
                    }
                }
                var list = new List<ObjectiveDto>();
                foreach (var item in datasList)
                {
                    var accountIds = new List<int>();
                    if (item.UserList.IsNullOrEmpty() == false)
                    {
                        var accountList = item.UserList.Split(',').Select(x=>x.Trim()).ToArray();
                        foreach (var username in accountList)
                        {
                            var account = await _serviceAccount.GetByUsername(username);
                            if (account != null)
                            {
                                accountIds.Add(account.Id);
                            }
                        }
                    }
                    list.Add(new ObjectiveDto { Topic = item.KPIObjective, CreatedBy = userid, AccountIdList = accountIds });
                }
                var check = await _serviceObjective.AddRangeAsync(list);
                if (check.Success == true)
                    return Ok();
                else
                {
                    return BadRequest(check);
                }
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpGet]
        public async Task<IActionResult> ExcelExport()
        {
            string filename = "FHOTemplate.xlsx";
            if (filename == null)
                return Content("filename not present");

            var path = Path.Combine(
                           Directory.GetCurrentDirectory(),
                           "wwwroot/excelTemplate", filename);

            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, GetContentType(path), Path.GetFileName(path));
        }
        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }
        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/octet-stream"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"}
            };
        }

        [HttpPut]
        public async Task<ActionResult> Reject([FromBody] ReleaseRequestDto requestDto)
        {
            return StatusCodeResult(await _service.Reject(requestDto.Ids));
        }
        [HttpPut]
        public async Task<ActionResult> DisableReject([FromBody] ReleaseRequestDto requestDto)
        {
            return StatusCodeResult(await _service.DisableReject(requestDto.Ids));
        }
        [HttpPut]
        public async Task<ActionResult> Release([FromBody] RejectRequestDto requestDto)
        {
            return StatusCodeResult(await _service.Release(requestDto.Ids));
        }
    }
}
