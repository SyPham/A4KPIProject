using AutoMapper;
using Microsoft.AspNetCore.Http;
using A4KPI.Data;
using A4KPI.DTO;
using A4KPI.Models;
using System;
using System.Threading.Tasks;
using A4KPI.Helpers;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using ESS_API.Helpers;
using System.Linq;
using A4KPI.Constants;
using NetUtility;
using System.IO;
using Aspose.Cells;
using Microsoft.AspNetCore.Hosting;

namespace A4KPI.Services
{
    public interface IH1H2Service
    {
      
        Task<object> GetH1H2Data();
        Task<object> GetReportInfo(int accountId);
        Task<Byte[]> ExportExcel(int accountId);
        Task<object> GetAllKPIScoreL0ByPeriod(int period);
    }
    public class H1H2Service : IH1H2Service
    {
        private OperationResult operationResult;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IRepositoryBase<ToDoList> _repo;
        private readonly IRepositoryBase<Account> _repoAccount;
        private readonly IRepositoryBase<AccountGroupAccount> _repoAccountGroupAccount;
        private readonly IRepositoryBase<Objective> _repoObjective;
        private readonly IRepositoryBase<ResultOfMonth> _repoResultOfMonth;
        private readonly IRepositoryBase<KPIScore> _repoKPIScore;
        private readonly IRepositoryBase<PeriodType> _repoPeriodType;
        private readonly IRepositoryBase<OCAccount> _repoOCAccount;
        private readonly IRepositoryBase<PIC> _repoPIC;
        private readonly IRepositoryBase<OC> _repoOC;
        private readonly IRepositoryBase<Period> _repoPeriod;
        private readonly IRepositoryBase<AttitudeScore> _repoAttitudeScore;
        private readonly IRepositoryBase<SpecialContributionScore> _repoSpecial;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IRepositoryBase<Models.Comment> _repoComment;
        private readonly MapperConfiguration _configMapper;
        public H1H2Service(
            IRepositoryBase<ToDoList> repo,
            IRepositoryBase<Account> repoAccount,
            IRepositoryBase<AccountGroupAccount> repoAccountGroupAccount,
            IRepositoryBase<Objective> repoObjective,
            IRepositoryBase<ResultOfMonth> repoResultOfMonth,
            IRepositoryBase<KPIScore> repoKPIScore,
            IRepositoryBase<PeriodType> repoPeriodType,
            IRepositoryBase<OCAccount> repoOCAccount,
            IRepositoryBase<PIC> repoPIC,
            IRepositoryBase<OC> repoOC,
            IRepositoryBase<Period> repoPeriod,
            IRepositoryBase<Models.Comment> repoComment,
            IRepositoryBase<AttitudeScore> repoAttitudeScore,
            IRepositoryBase<SpecialContributionScore> repoSpecial,
            IHttpContextAccessor httpContextAccessor,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IWebHostEnvironment webHostEnvironment,
            MapperConfiguration configMapper
            )
        {
            _repo = repo;
            _repoComment = repoComment;
            _repoAccount = repoAccount;
            _repoAccountGroupAccount = repoAccountGroupAccount;
            _repoSpecial = repoSpecial;
            _repoObjective = repoObjective;
            _repoResultOfMonth = repoResultOfMonth;
            _repoKPIScore = repoKPIScore;
            _repoPeriodType = repoPeriodType;
            _repoOCAccount = repoOCAccount;
            _repoPIC = repoPIC;
            _repoOC = repoOC;
            _repoPeriod = repoPeriod;
            _repoAttitudeScore = repoAttitudeScore;
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
            _configMapper = configMapper;
        }


        public async Task<Byte[]> ExportExcel(int accountId)
        {
            var data = await GetReportInfo2(accountId);
            var H1 = data.H1;
            var H2 = data.H2;
            var avg = data.avg;
            var path = Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot\\excelTemplate\\H1H2REPORT.xlsx");
            WorkbookDesigner designer = new WorkbookDesigner();
            designer.Workbook = new Workbook(path);
            Worksheet ws = designer.Workbook.Worksheets[0];
            ws.Name = "H1,H2 Report ";
            ws.Cells["J2"].PutValue("Dept. " + H1.OC);
            ws.Cells["L2"].PutValue("Name " + H1.FullName);
            //H1
            ws.Cells["D5"].PutValue("H1得分:  " + H1.A_total);
            ws.Cells["F5"].PutValue(H1.L1Score);
            ws.Cells["G5"].PutValue(H1.L2Score);
            ws.Cells["H5"].PutValue(H1.FLScore);
            ws.Cells["D8"].PutValue("Q1 KPI: " + H1.L1Q1Comment);
            ws.Cells["D9"].PutValue("Q2 KPI: " + H1.L1Q2Comment);
            ws.Cells["D10"].PutValue("H1 Attitude: " + H1.L1HComment);

            ws.Cells["D11"].PutValue("Q1 KPI: " + H1.L2Q1Comment);
            ws.Cells["D12"].PutValue("Q2 KPI: " + H1.L2Q2Comment);
            ws.Cells["D13"].PutValue("H1 Attitude: " + H1.L2HComment);

            ws.Cells["D14"].PutValue("H1 Attitude: " + H1.FLHComment);
            ws.Cells["D17"].PutValue("H1得分: " + H1.B_total);

            ws.Cells["F17"].PutValue(H1.selfScore);
            ws.Cells["H17"].PutValue(H1.L1);
            ws.Cells["I17"].PutValue(H1.L2);
            ws.Cells["J17"].PutValue(H1.Smart);

            ws.Cells["F18"].PutValue(H1.B_selfScore);
            ws.Cells["H18"].PutValue(H1.B_L1);
            ws.Cells["I18"].PutValue(H1.B_L2);
            ws.Cells["J18"].PutValue(H1.B_Smart);
            ws.Cells["D20"].PutValue("H1: " + H1.C_total);
            ws.Cells["F20"].PutValue(H1.SpecialScore);
            ws.Cells["E24"].PutValue("H1得分: " + H1.D_total);

            //endH1

            //H2
            ws.Cells["N5"].PutValue("H2得分:  " + H2.A_total);
            ws.Cells["P5"].PutValue(H2.L1Score);
            ws.Cells["Q5"].PutValue(H2.L2Score);
            ws.Cells["R5"].PutValue(H2.FLScore);
            ws.Cells["N8"].PutValue("Q3 KPI: " + H2.L1Q1Comment);
            ws.Cells["N9"].PutValue("Q4 KPI: " + H2.L1Q2Comment);
            ws.Cells["N10"].PutValue("H2 Attitude: " + H2.L1HComment);

            ws.Cells["N11"].PutValue("Q3 KPI: " + H2.L2Q1Comment);
            ws.Cells["N12"].PutValue("Q4 KPI: " + H2.L2Q2Comment);
            ws.Cells["N13"].PutValue("H1 Attitude: " + H2.L2HComment);

            ws.Cells["N14"].PutValue("H2 Attitude: " + H2.FLHComment);
            ws.Cells["N17"].PutValue("H2得分: " + H2.B_total);

            ws.Cells["P17"].PutValue(H2.selfScore);
            ws.Cells["R17"].PutValue(H2.L1);
            ws.Cells["S17"].PutValue(H2.L2);
            ws.Cells["T17"].PutValue(H2.Smart);

            ws.Cells["P18"].PutValue(H2.B_selfScore);
            ws.Cells["R18"].PutValue(H2.B_L1);
            ws.Cells["S18"].PutValue(H2.B_L2);
            ws.Cells["T18"].PutValue(H2.B_Smart);
            ws.Cells["N20"].PutValue("H2: " + H2.C_total);
            ws.Cells["P20"].PutValue(H2.SpecialScore);
            ws.Cells["O24"].PutValue("H2得分: " + H2.D_total);

            //endH2
            ws.Cells["E26"].PutValue("年度得分: " + avg);
            var stream = new MemoryStream();
            designer.Workbook.Save(stream, SaveFormat.Xlsx);
            Byte[] bin = stream.ToArray();
            //Lưu file lại
            return bin; ;
        }
        public async Task<DataReportDto> GetReportInfo2(int accountId)
        {
            var ocuser = await _repoOCAccount.FindAll(x => x.AccountId == accountId)
             .Select(x => new
             {
                 OC = x.OC.Name,
                 OCId = x.OC.ParentId,
                 FullName = x.Account.FullName
             }).FirstOrDefaultAsync();
            if (ocuser == null) return new DataReportDto();

            var currentDate = DateTime.Today;
            var currentMonth = currentDate.Month + "";
            var currentYear = currentDate.Year;
            var leader_id = _repoAccount.FindById(accountId).Leader;
            var quarterly = await _repoPeriodType.FindAll(x => x.Code == SystemPeriod.Quarterly).FirstOrDefaultAsync();
            var half_year = await _repoPeriodType.FindAll(x => x.Code == SystemPeriod.HalfYear).FirstOrDefaultAsync();
            var periods_halfyear = half_year.Periods.Select(x => new
            {
                x.Title,
                x.Value,
                x.ReportTime,
                Months = x.Months.Split(',').ToList()
            }).Where(x => x.Months.Contains(currentMonth)).FirstOrDefault();

            var periods_quarterly = quarterly.Periods.Select(x => new
            {
                x.Title,
                x.Value,
                x.ReportTime,
                Months = x.Months.Split(',').ToList()
            }).Where(x => x.Months.Contains(currentMonth)).FirstOrDefault();

            var l1 = await _repoAccount.FindAll(x => x.Id == accountId).FirstOrDefaultAsync();
            if (l1 == null) return new DataReportDto();
            var dataH1 = new H1H2ReportDto();
            var dataH2 = new H1H2ReportDto();
            var setting_halfYear = new List<int> { HalfYear.H1, HalfYear.H2 };
            foreach (var item in setting_halfYear)
            {
                if (item == HalfYear.H1)
                {

                    double? l1ScoreKPI = await _repoAttitudeScore.FindAll(x =>
                                                 x.PeriodTypeId == SystemPeriodType.HalfYear
                                                && x.CreatedTime.Year == DateTime.Today.Year
                                                && x.Period == item
                                                && x.ScoreType == ScoreType.L1
                                                && accountId == x.AccountId
                                                && l1.Manager == x.ScoreBy
                                                && x.AccountId != l1.Manager)
                                            .Select(x => x.Point)
                                            .FirstOrDefaultAsync();
                    //start
                    double? l0ScoreKPI = await _repoKPIScore.FindAll(x =>
                                                 x.PeriodTypeId == SystemPeriodType.Quarterly
                                                && x.CreatedTime.Year == DateTime.Today.Year
                                                && x.Period == Quarter.Q2
                                                && x.ScoreType == ScoreType.L0
                                                && accountId == x.AccountId
                                                && l1.Manager == x.ScoreBy
                                                && x.AccountId != l1.Manager)
                                            .Select(x => x.Point)
                                            .FirstOrDefaultAsync();
                    double? l1Q1ScoreKPI = await _repoKPIScore.FindAll(x =>
                                                 x.PeriodTypeId == SystemPeriodType.Quarterly
                                                && x.CreatedTime.Year == DateTime.Today.Year
                                                && x.Period == Quarter.Q1
                                                && x.ScoreType == ScoreType.L1
                                                && accountId == x.AccountId
                                                && l1.Manager == x.ScoreBy
                                                && x.AccountId != l1.Manager)
                                            .Select(x => x.Point)
                                            .FirstOrDefaultAsync();
                    double? l1Q2ScoreKPI = await _repoKPIScore.FindAll(x =>
                                                 x.PeriodTypeId == SystemPeriodType.Quarterly
                                                && x.CreatedTime.Year == DateTime.Today.Year
                                                && x.Period == Quarter.Q2
                                                && x.ScoreType == ScoreType.L1
                                                && accountId == x.AccountId
                                                && l1.Manager == x.ScoreBy
                                                && x.AccountId != l1.Manager)
                                            .Select(x => x.Point)
                                            .FirstOrDefaultAsync();
                    double? l2Q1ScoreKPI = await _repoKPIScore.FindAll(x =>
                                                 x.PeriodTypeId == SystemPeriodType.Quarterly
                                                && x.CreatedTime.Year == DateTime.Today.Year
                                                && x.Period == Quarter.Q1
                                                && x.ScoreType == ScoreType.L2
                                                && accountId == x.AccountId
                                                && l1.Manager == x.ScoreBy
                                                && x.AccountId != l1.Manager)
                                            .Select(x => x.Point)
                                            .FirstOrDefaultAsync();
                    double? l2Q2ScoreKPI = await _repoKPIScore.FindAll(x =>
                                                 x.PeriodTypeId == SystemPeriodType.Quarterly
                                                && x.CreatedTime.Year == DateTime.Today.Year
                                                && x.Period == Quarter.Q2
                                                && x.ScoreType == ScoreType.L2
                                                && accountId == x.AccountId
                                                && l1.Manager == x.ScoreBy
                                                && x.AccountId != l1.Manager)
                                            .Select(x => x.Point)
                                            .FirstOrDefaultAsync();
                    double? GHRQ1ScoreKPI = await _repoKPIScore.FindAll(x =>
                                                 x.PeriodTypeId == SystemPeriodType.Quarterly
                                                && x.CreatedTime.Year == DateTime.Today.Year
                                                && x.Period == Quarter.Q1
                                                && x.ScoreType == ScoreType.GHR
                                                && accountId == x.AccountId
                                                && l1.Manager == x.ScoreBy
                                                && x.AccountId != l1.Manager)
                                            .Select(x => x.Point)
                                            .FirstOrDefaultAsync();

                    double? GHRQ2ScoreKPI = await _repoKPIScore.FindAll(x =>
                                                 x.PeriodTypeId == SystemPeriodType.Quarterly
                                                && x.CreatedTime.Year == DateTime.Today.Year
                                                && x.Period == Quarter.Q2
                                                && x.ScoreType == ScoreType.GHR
                                                && accountId == x.AccountId
                                                && l1.Manager == x.ScoreBy
                                                && x.AccountId != l1.Manager)
                                            .Select(x => x.Point)
                                            .FirstOrDefaultAsync();

                    double? Score_special = await _repoSpecial.FindAll(x =>
                                                 x.PeriodTypeId == SystemPeriodType.HalfYear
                                                && x.CreatedTime.Year == DateTime.Today.Year
                                                && x.Period == Quarter.Q2
                                                && x.ScoreType == ScoreType.L2
                                                && accountId == x.AccountId
                                                && l1.Manager == x.ScoreBy
                                                && x.AccountId != l1.Manager)
                                            .Select(x => x.Point)
                                            .FirstOrDefaultAsync();
                    //end

                    var l1Comment = await _repoComment.FindAll(x =>
                                               x.PeriodTypeId == SystemPeriodType.HalfYear
                                              && x.CreatedTime.Year == DateTime.Today.Year
                                              && x.Period == item
                                              && x.ScoreType == ScoreType.L1
                                              && accountId == x.AccountId
                                              && l1.Manager == x.CreatedBy
                                              && x.AccountId != l1.Manager)
                                            .Select(x => x.Content)
                                          .FirstOrDefaultAsync();
                    var l1Q1Comment = await _repoComment.FindAll(x =>
                                               x.PeriodTypeId == SystemPeriodType.Quarterly
                                              && x.CreatedTime.Year == DateTime.Today.Year
                                              && x.Period == Quarter.Q1
                                              && x.ScoreType == ScoreType.L1
                                              && accountId == x.AccountId
                                              && l1.Manager == x.CreatedBy
                                              && x.AccountId != l1.Manager)
                                            .Select(x => x.Content)
                                          .FirstOrDefaultAsync();
                    var l1Q2Comment = await _repoComment.FindAll(x =>
                                               x.PeriodTypeId == SystemPeriodType.Quarterly
                                              && x.CreatedTime.Year == DateTime.Today.Year
                                              && x.Period == Quarter.Q2
                                              && x.ScoreType == ScoreType.L1
                                              && accountId == x.AccountId
                                              && l1.Manager == x.CreatedBy
                                              && x.AccountId != l1.Manager)
                                            .Select(x => x.Content)
                                          .FirstOrDefaultAsync();
                    var l1H1Comment = await _repoComment.FindAll(x =>
                                               x.PeriodTypeId == SystemPeriodType.HalfYear
                                              && x.CreatedTime.Year == DateTime.Today.Year
                                              && x.Period == item
                                              && x.ScoreType == ScoreType.L1
                                              && accountId == x.AccountId
                                              && l1.Manager == x.CreatedBy
                                              && x.AccountId != l1.Manager)
                                            .Select(x => x.Content)
                                          .FirstOrDefaultAsync();

                    double? l2ScoreKPI = await _repoAttitudeScore.FindAll(x =>
                                               x.PeriodTypeId == SystemPeriodType.HalfYear
                                              && x.CreatedTime.Year == DateTime.Today.Year
                                              && x.Period == item
                                              && x.ScoreType == ScoreType.L2
                                              && accountId == x.AccountId)
                                            .Select(x => x.Point)
                                          .FirstOrDefaultAsync();
                    var l2Comment = await _repoComment.FindAll(x =>
                                               x.PeriodTypeId == SystemPeriodType.HalfYear
                                              && x.CreatedTime.Year == DateTime.Today.Year
                                              && x.Period == item
                                              && x.ScoreType == ScoreType.L2
                                              && accountId == x.AccountId)
                                            .Select(x => x.Content)
                                          .FirstOrDefaultAsync();
                    var l2H1Comment = await _repoComment.FindAll(x =>
                                               x.PeriodTypeId == SystemPeriodType.HalfYear
                                              && x.CreatedTime.Year == DateTime.Today.Year
                                              && x.Period == item
                                              && x.ScoreType == ScoreType.L2
                                              && accountId == x.AccountId)
                                            .Select(x => x.Content)
                                          .FirstOrDefaultAsync();

                    var FLH1Comment = await _repoComment.FindAll(x =>
                                               x.PeriodTypeId == SystemPeriodType.HalfYear
                                              && x.CreatedTime.Year == DateTime.Today.Year
                                              && x.Period == item
                                              && x.ScoreType == ScoreType.FunctionalLeader
                                              && accountId == x.AccountId)
                                            .Select(x => x.Content)
                                          .FirstOrDefaultAsync();
                    var l2Q1Comment = await _repoComment.FindAll(x =>
                                               x.PeriodTypeId == SystemPeriodType.Quarterly
                                              && x.CreatedTime.Year == DateTime.Today.Year
                                              && x.Period == Quarter.Q1
                                              && x.ScoreType == ScoreType.L2
                                              && accountId == x.AccountId)
                                            .Select(x => x.Content)
                                          .FirstOrDefaultAsync();
                    var l2Q2Comment = await _repoComment.FindAll(x =>
                                               x.PeriodTypeId == SystemPeriodType.Quarterly
                                              && x.CreatedTime.Year == DateTime.Today.Year
                                              && x.Period == Quarter.Q2
                                              && x.ScoreType == ScoreType.L2
                                              && accountId == x.AccountId)
                                            .Select(x => x.Content)
                                          .FirstOrDefaultAsync();

                    double? ghrScore = await _repoKPIScore.FindAll(x =>
                                              x.PeriodTypeId == SystemPeriodType.HalfYear
                                             && x.CreatedTime.Year == DateTime.Today.Year
                                             && x.Period == item
                                             && x.ScoreType == ScoreType.GHR
                                             && accountId == x.AccountId)
                                           .Select(x => x.Point)
                                         .FirstOrDefaultAsync();
                    double? FLScore = await _repoAttitudeScore.FindAll(x =>
                                              x.PeriodTypeId == SystemPeriodType.HalfYear
                                             && x.CreatedTime.Year == DateTime.Today.Year
                                             && x.Period == item
                                             && x.ScoreType == ScoreType.FunctionalLeader
                                             && leader_id == x.ScoreBy)
                                           .Select(x => x.Point)
                                         .FirstOrDefaultAsync();
                    double A_total = Math.Round(await ConvertAtotal(l1ScoreKPI ?? 0, l2ScoreKPI ?? 0, FLScore ?? 0), 2);
                    double L1 = Math.Round((l1Q1ScoreKPI + l1Q2ScoreKPI) / 2 ?? 0, 2);
                    double L2 = Math.Round((l2Q1ScoreKPI + l2Q2ScoreKPI) / 2 ?? 0 , 2);
                    double Smart = Math.Round((GHRQ1ScoreKPI + GHRQ2ScoreKPI) / 2 ?? 0, 2);
                    double B_selfScore = Math.Round(await BSelfScore(l0ScoreKPI ?? 0),2);
                    double B_L1 = Math.Round(await BL1((l1Q1ScoreKPI + l1Q2ScoreKPI) / 2 ?? 0),2);
                    double B_L2 = Math.Round(await BL2((l2Q1ScoreKPI + l2Q2ScoreKPI) / 2 ?? 0), 2);
                    double B_Smart = Math.Round(await BSmart((GHRQ1ScoreKPI + GHRQ2ScoreKPI) / 2 ?? 0),2);
                    dataH1 = new H1H2ReportDto(item, periods_quarterly.Value, currentYear)
                    {
                        FullName = ocuser.FullName,
                        OC = ocuser.OC,
                        L1Score = l1ScoreKPI ?? 0,
                        L1Comment = l1Comment ?? "",
                        L1Q1Comment = l1Q1Comment ?? "",
                        L1Q2Comment = l1Q2Comment ?? "",
                        L2Score = l2ScoreKPI ?? 0,
                        L2Comment = l2Comment ?? "",
                        L2Q1Comment = l2Q1Comment ?? "",
                        L2Q2Comment = l2Q2Comment ?? "",
                        SmartScore = ghrScore ?? 0,
                        L1HComment= l1H1Comment ?? "",
                        L2HComment= l2H1Comment ?? "",
                        FLHComment= FLH1Comment ?? "",
                        FLScore = FLScore ?? 0,
                        selfScore = l0ScoreKPI ?? 0,
                        SpecialScore = Score_special ?? 0,
                        L1 = L1,
                        L2 = L2,
                        Smart = Smart,
                        B_selfScore = B_selfScore,
                        B_L1 = B_L1,
                        B_L2 = B_L2,
                        B_Smart = B_Smart,
                        A_total = A_total,
                        B_total = B_selfScore + B_L1 + B_L2 + B_Smart,
                        C_total = Score_special ?? 0,
                        D_total = Math.Round(A_total + (B_selfScore + B_L1 + B_L2 + B_Smart) + (Score_special ?? 0), 2)
                    };

                } else
                {
                    double? l1ScoreKPI = await _repoAttitudeScore.FindAll(x =>
                                                 x.PeriodTypeId == SystemPeriodType.HalfYear
                                                && x.CreatedTime.Year == DateTime.Today.Year
                                                && x.Period == item
                                                && x.ScoreType == ScoreType.L1
                                                && accountId == x.AccountId
                                                && l1.Manager == x.ScoreBy
                                                && x.AccountId != l1.Manager)
                                            .Select(x => x.Point)
                                            .FirstOrDefaultAsync();
                    //start
                    double? l0ScoreKPI = await _repoKPIScore.FindAll(x =>
                                                 x.PeriodTypeId == SystemPeriodType.Quarterly
                                                && x.CreatedTime.Year == DateTime.Today.Year
                                                && x.Period == Quarter.Q4
                                                && x.ScoreType == ScoreType.L0
                                                && accountId == x.AccountId
                                                && l1.Manager == x.ScoreBy
                                                && x.AccountId != l1.Manager)
                                            .Select(x => x.Point)
                                            .FirstOrDefaultAsync();
                    double? l1Q3ScoreKPI = await _repoKPIScore.FindAll(x =>
                                                 x.PeriodTypeId == SystemPeriodType.Quarterly
                                                && x.CreatedTime.Year == DateTime.Today.Year
                                                && x.Period == Quarter.Q3
                                                && x.ScoreType == ScoreType.L1
                                                && accountId == x.AccountId
                                                && l1.Manager == x.ScoreBy
                                                && x.AccountId != l1.Manager)
                                            .Select(x => x.Point)
                                            .FirstOrDefaultAsync();
                    double? l1Q4ScoreKPI = await _repoKPIScore.FindAll(x =>
                                                 x.PeriodTypeId == SystemPeriodType.Quarterly
                                                && x.CreatedTime.Year == DateTime.Today.Year
                                                && x.Period == Quarter.Q4
                                                && x.ScoreType == ScoreType.L1
                                                && accountId == x.AccountId
                                                && l1.Manager == x.ScoreBy
                                                && x.AccountId != l1.Manager)
                                            .Select(x => x.Point)
                                            .FirstOrDefaultAsync();
                    double? l2Q3ScoreKPI = await _repoKPIScore.FindAll(x =>
                                                 x.PeriodTypeId == SystemPeriodType.Quarterly
                                                && x.CreatedTime.Year == DateTime.Today.Year
                                                && x.Period == Quarter.Q3
                                                && x.ScoreType == ScoreType.L2
                                                && accountId == x.AccountId
                                                && l1.Manager == x.ScoreBy
                                                && x.AccountId != l1.Manager)
                                            .Select(x => x.Point)
                                            .FirstOrDefaultAsync();
                    double? l2Q4ScoreKPI = await _repoKPIScore.FindAll(x =>
                                                 x.PeriodTypeId == SystemPeriodType.Quarterly
                                                && x.CreatedTime.Year == DateTime.Today.Year
                                                && x.Period == Quarter.Q4
                                                && x.ScoreType == ScoreType.L2
                                                && accountId == x.AccountId
                                                && l1.Manager == x.ScoreBy
                                                && x.AccountId != l1.Manager)
                                            .Select(x => x.Point)
                                            .FirstOrDefaultAsync();
                    double? GHRQ3ScoreKPI = await _repoKPIScore.FindAll(x =>
                                                 x.PeriodTypeId == SystemPeriodType.Quarterly
                                                && x.CreatedTime.Year == DateTime.Today.Year
                                                && x.Period == Quarter.Q3
                                                && x.ScoreType == ScoreType.GHR
                                                && accountId == x.AccountId
                                                && l1.Manager == x.ScoreBy
                                                && x.AccountId != l1.Manager)
                                            .Select(x => x.Point)
                                            .FirstOrDefaultAsync();

                    double? GHRQ4ScoreKPI = await _repoKPIScore.FindAll(x =>
                                                 x.PeriodTypeId == SystemPeriodType.Quarterly
                                                && x.CreatedTime.Year == DateTime.Today.Year
                                                && x.Period == Quarter.Q4
                                                && x.ScoreType == ScoreType.GHR
                                                && accountId == x.AccountId
                                                && l1.Manager == x.ScoreBy
                                                && x.AccountId != l1.Manager)
                                            .Select(x => x.Point)
                                            .FirstOrDefaultAsync();

                    double? Score_special = await _repoSpecial.FindAll(x =>
                                                 x.PeriodTypeId == SystemPeriodType.HalfYear
                                                && x.CreatedTime.Year == DateTime.Today.Year
                                                && x.Period == Quarter.Q4
                                                && x.ScoreType == ScoreType.L2
                                                && accountId == x.AccountId
                                                && l1.Manager == x.ScoreBy
                                                && x.AccountId != l1.Manager)
                                            .Select(x => x.Point)
                                            .FirstOrDefaultAsync();
                    //end
                    var l1Comment = await _repoComment.FindAll(x =>
                                               x.PeriodTypeId == SystemPeriodType.HalfYear
                                              && x.CreatedTime.Year == DateTime.Today.Year
                                              && x.Period == item
                                              && x.ScoreType == ScoreType.L1
                                              && accountId == x.AccountId
                                              && l1.Manager == x.CreatedBy
                                              && x.AccountId != l1.Manager)
                                            .Select(x => x.Content)
                                          .FirstOrDefaultAsync();
                    var l1H2Comment = await _repoComment.FindAll(x =>
                                               x.PeriodTypeId == SystemPeriodType.HalfYear
                                              && x.CreatedTime.Year == DateTime.Today.Year
                                              && x.Period == item
                                              && x.ScoreType == ScoreType.L1
                                              && accountId == x.AccountId
                                              && l1.Manager == x.CreatedBy
                                              && x.AccountId != l1.Manager)
                                            .Select(x => x.Content)
                                          .FirstOrDefaultAsync();

                    var FLH2Comment = await _repoComment.FindAll(x =>
                                               x.PeriodTypeId == SystemPeriodType.HalfYear
                                              && x.CreatedTime.Year == DateTime.Today.Year
                                              && x.Period == item
                                              && x.ScoreType == ScoreType.FunctionalLeader
                                              && accountId == x.AccountId)
                                            .Select(x => x.Content)
                                          .FirstOrDefaultAsync();
                    var l1Q3Comment = await _repoComment.FindAll(x =>
                                               x.PeriodTypeId == SystemPeriodType.Quarterly
                                              && x.CreatedTime.Year == DateTime.Today.Year
                                              && x.Period == Quarter.Q3
                                              && x.ScoreType == ScoreType.L1
                                              && accountId == x.AccountId
                                              && l1.Manager == x.CreatedBy
                                              && x.AccountId != l1.Manager)
                                            .Select(x => x.Content)
                                          .FirstOrDefaultAsync();
                    var l1Q4Comment = await _repoComment.FindAll(x =>
                                               x.PeriodTypeId == SystemPeriodType.Quarterly
                                              && x.CreatedTime.Year == DateTime.Today.Year
                                              && x.Period == Quarter.Q4
                                              && x.ScoreType == ScoreType.L1
                                              && accountId == x.AccountId
                                              && l1.Manager == x.CreatedBy
                                              && x.AccountId != l1.Manager)
                                            .Select(x => x.Content)
                                          .FirstOrDefaultAsync();

                    double? l2ScoreKPI = await _repoAttitudeScore.FindAll(x =>
                                               x.PeriodTypeId == SystemPeriodType.HalfYear
                                              && x.CreatedTime.Year == DateTime.Today.Year
                                              && x.Period == item
                                              && x.ScoreType == ScoreType.L2
                                              && accountId == x.AccountId)
                                            .Select(x => x.Point)
                                          .FirstOrDefaultAsync();
                    var l2Comment = await _repoComment.FindAll(x =>
                                               x.PeriodTypeId == SystemPeriodType.HalfYear
                                              && x.CreatedTime.Year == DateTime.Today.Year
                                              && x.Period == item
                                              && x.ScoreType == ScoreType.L2
                                              && accountId == x.AccountId)
                                            .Select(x => x.Content)
                                          .FirstOrDefaultAsync();
                    var l2H2Comment = await _repoComment.FindAll(x =>
                                               x.PeriodTypeId == SystemPeriodType.HalfYear
                                              && x.CreatedTime.Year == DateTime.Today.Year
                                              && x.Period == item
                                              && x.ScoreType == ScoreType.L2
                                              && accountId == x.AccountId)
                                            .Select(x => x.Content)
                                          .FirstOrDefaultAsync();
                    var l2Q3Comment = await _repoComment.FindAll(x =>
                                               x.PeriodTypeId == SystemPeriodType.Quarterly
                                              && x.CreatedTime.Year == DateTime.Today.Year
                                              && x.Period == Quarter.Q3
                                              && x.ScoreType == ScoreType.L2
                                              && accountId == x.AccountId)
                                            .Select(x => x.Content)
                                          .FirstOrDefaultAsync();
                    var l2Q4Comment = await _repoComment.FindAll(x =>
                                               x.PeriodTypeId == SystemPeriodType.Quarterly
                                              && x.CreatedTime.Year == DateTime.Today.Year
                                              && x.Period == Quarter.Q4
                                              && x.ScoreType == ScoreType.L2
                                              && accountId == x.AccountId)
                                            .Select(x => x.Content)
                                          .FirstOrDefaultAsync();

                    double? ghrScore = await _repoKPIScore.FindAll(x =>
                                              x.PeriodTypeId == SystemPeriodType.HalfYear
                                             && x.CreatedTime.Year == DateTime.Today.Year
                                             && x.Period == item
                                             && x.ScoreType == ScoreType.GHR
                                             && accountId == x.AccountId)
                                           .Select(x => x.Point)
                                         .FirstOrDefaultAsync();
                    double? FLScore = await _repoAttitudeScore.FindAll(x =>
                                              x.PeriodTypeId == SystemPeriodType.HalfYear
                                             && x.CreatedTime.Year == DateTime.Today.Year
                                             && x.Period == item
                                             && x.ScoreType == ScoreType.FunctionalLeader
                                             && leader_id == x.ScoreBy)
                                           .Select(x => x.Point)
                                         .FirstOrDefaultAsync();
                    double A_total = Math.Round(await ConvertAtotal(l1ScoreKPI ?? 0, l2ScoreKPI ?? 0, FLScore ?? 0), 2);
                    double L1 = Math.Round((l1Q3ScoreKPI + l1Q4ScoreKPI) / 2 ?? 0, 2);
                    double L2 = Math.Round((l2Q3ScoreKPI + l2Q4ScoreKPI) / 2 ?? 0, 2);
                    double Smart = Math.Round((GHRQ3ScoreKPI + GHRQ4ScoreKPI) / 2 ?? 0, 2);
                    double B_selfScore = Math.Round(await BSelfScore(l0ScoreKPI ?? 0), 2);
                    double B_L1 = Math.Round(await BL1((l1Q3ScoreKPI + l1Q4ScoreKPI) / 2 ?? 0), 2);
                    double B_L2 = Math.Round(await BL2((l2Q3ScoreKPI + l2Q4ScoreKPI) / 2 ?? 0), 2);
                    double B_Smart = Math.Round(await BSmart((GHRQ3ScoreKPI + GHRQ4ScoreKPI) / 2 ?? 0), 2);
                    dataH2 = new H1H2ReportDto(item, periods_quarterly.Value, currentYear)
                    {
                        FullName = ocuser.FullName,
                        OC = ocuser.OC,
                        L1Score = l1ScoreKPI ?? 0,
                        L1Comment = l1Comment ?? "",
                        L1Q1Comment = l1Q3Comment ?? "",
                        L1Q2Comment = l1Q4Comment ?? "",
                        L2Score = l2ScoreKPI ?? 0,
                        L2Comment = l2Comment ?? "",
                        L2Q1Comment = l2Q3Comment ?? "",
                        L2Q2Comment = l2Q4Comment ?? "",
                        L1HComment = l1H2Comment ?? "",
                        L2HComment = l2H2Comment ?? "",
                        FLHComment = FLH2Comment ?? "",
                        SmartScore = ghrScore ?? 0,
                        FLScore = FLScore ?? 0,
                        selfScore = l0ScoreKPI ?? 0,
                        SpecialScore = Score_special ?? 0,
                        L1 = L1,
                        L2 = L2,
                        Smart = Smart,
                        B_selfScore = B_selfScore,
                        B_L1 = B_L1,
                        B_L2 = B_L2,
                        B_Smart = B_Smart,
                        A_total = A_total,
                        B_total = B_selfScore + B_L1 + B_L2 + B_Smart,
                        C_total = Score_special ?? 0,
                        D_total = Math.Round(A_total + (B_selfScore + B_L1 + B_L2 + B_Smart) + (Score_special ?? 0), 2)

                    };
                }
            }

            return new DataReportDto
            { 
                H1 = dataH1,
                H2 = dataH2,
                avg = Math.Round((dataH1.D_total + dataH2.D_total) / 2, 2)
            };
        }

        public async Task<object> GetReportInfo(int accountId)
        {
            var ocuser = await _repoOCAccount.FindAll(x => x.AccountId == accountId)
             .Select(x => new
             {
                 OC = x.OC.Name,
                 OCId = x.OC.ParentId,
                 FullName = x.Account.FullName
             }).FirstOrDefaultAsync();
            if (ocuser == null) return new H1H2ReportDto();

            var currentDate = DateTime.Today;
            var currentMonth = currentDate.Month + "";
            var currentYear = currentDate.Year;
            var leader_id = _repoAccount.FindById(accountId).Leader;
            var quarterly = await _repoPeriodType.FindAll(x => x.Code == SystemPeriod.Quarterly).FirstOrDefaultAsync();
            var half_year = await _repoPeriodType.FindAll(x => x.Code == SystemPeriod.HalfYear).FirstOrDefaultAsync();
            var periods_halfyear = half_year.Periods.Select(x => new
            {
                x.Title,
                x.Value,
                x.ReportTime,
                Months = x.Months.Split(',').ToList()
            }).Where(x => x.Months.Contains(currentMonth)).FirstOrDefault();

            var periods_quarterly = quarterly.Periods.Select(x => new
            {
                x.Title,
                x.Value,
                x.ReportTime,
                Months = x.Months.Split(',').ToList()
            }).Where(x => x.Months.Contains(currentMonth)).FirstOrDefault();

            var l1 = await _repoAccount.FindAll(x => x.Id == accountId).FirstOrDefaultAsync();
            if (l1 == null) return new Q1Q3ReportDto();
            var dataH1 = new H1H2ReportDto();
            var dataH2 = new H1H2ReportDto();
            var setting_halfYear = new List<int> { HalfYear.H1, HalfYear.H2 };
            foreach (var item in setting_halfYear)
            {
                if (item == HalfYear.H1)
                {

                    double? l1ScoreKPI = await _repoAttitudeScore.FindAll(x =>
                                                 x.PeriodTypeId == SystemPeriodType.HalfYear
                                                && x.CreatedTime.Year == DateTime.Today.Year
                                                && x.Period == item
                                                && x.ScoreType == ScoreType.L1
                                                && accountId == x.AccountId
                                                && l1.Manager == x.ScoreBy
                                                && x.AccountId != l1.Manager)
                                            .Select(x => x.Point)
                                            .FirstOrDefaultAsync();
                    //start
                    double? l0ScoreKPI = await _repoKPIScore.FindAll(x =>
                                                 x.PeriodTypeId == SystemPeriodType.Quarterly
                                                && x.CreatedTime.Year == DateTime.Today.Year
                                                && x.Period == Quarter.Q2
                                                && x.ScoreType == ScoreType.L0
                                                && accountId == x.AccountId
                                                && l1.Manager == x.ScoreBy
                                                && x.AccountId != l1.Manager)
                                            .Select(x => x.Point)
                                            .FirstOrDefaultAsync();
                    double? l1Q1ScoreKPI = await _repoKPIScore.FindAll(x =>
                                                 x.PeriodTypeId == SystemPeriodType.Quarterly
                                                && x.CreatedTime.Year == DateTime.Today.Year
                                                && x.Period == Quarter.Q1
                                                && x.ScoreType == ScoreType.L1
                                                && accountId == x.AccountId
                                                && l1.Manager == x.ScoreBy
                                                && x.AccountId != l1.Manager)
                                            .Select(x => x.Point)
                                            .FirstOrDefaultAsync();
                    double? l1Q2ScoreKPI = await _repoKPIScore.FindAll(x =>
                                                 x.PeriodTypeId == SystemPeriodType.Quarterly
                                                && x.CreatedTime.Year == DateTime.Today.Year
                                                && x.Period == Quarter.Q2
                                                && x.ScoreType == ScoreType.L1
                                                && accountId == x.AccountId
                                                && l1.Manager == x.ScoreBy
                                                && x.AccountId != l1.Manager)
                                            .Select(x => x.Point)
                                            .FirstOrDefaultAsync();
                    double? l2Q1ScoreKPI = await _repoKPIScore.FindAll(x =>
                                                 x.PeriodTypeId == SystemPeriodType.Quarterly
                                                && x.CreatedTime.Year == DateTime.Today.Year
                                                && x.Period == Quarter.Q1
                                                && x.ScoreType == ScoreType.L2
                                                && accountId == x.AccountId
                                                && l1.Manager == x.ScoreBy
                                                && x.AccountId != l1.Manager)
                                            .Select(x => x.Point)
                                            .FirstOrDefaultAsync();
                    double? l2Q2ScoreKPI = await _repoKPIScore.FindAll(x =>
                                                 x.PeriodTypeId == SystemPeriodType.Quarterly
                                                && x.CreatedTime.Year == DateTime.Today.Year
                                                && x.Period == Quarter.Q2
                                                && x.ScoreType == ScoreType.L2
                                                && accountId == x.AccountId
                                                && l1.Manager == x.ScoreBy
                                                && x.AccountId != l1.Manager)
                                            .Select(x => x.Point)
                                            .FirstOrDefaultAsync();
                    double? GHRQ1ScoreKPI = await _repoKPIScore.FindAll(x =>
                                                 x.PeriodTypeId == SystemPeriodType.Quarterly
                                                && x.CreatedTime.Year == DateTime.Today.Year
                                                && x.Period == Quarter.Q1
                                                && x.ScoreType == ScoreType.GHR
                                                && accountId == x.AccountId
                                                && l1.Manager == x.ScoreBy
                                                && x.AccountId != l1.Manager)
                                            .Select(x => x.Point)
                                            .FirstOrDefaultAsync();

                    double? GHRQ2ScoreKPI = await _repoKPIScore.FindAll(x =>
                                                 x.PeriodTypeId == SystemPeriodType.Quarterly
                                                && x.CreatedTime.Year == DateTime.Today.Year
                                                && x.Period == Quarter.Q2
                                                && x.ScoreType == ScoreType.GHR
                                                && accountId == x.AccountId
                                                && l1.Manager == x.ScoreBy
                                                && x.AccountId != l1.Manager)
                                            .Select(x => x.Point)
                                            .FirstOrDefaultAsync();

                    double? Score_special = await _repoSpecial.FindAll(x =>
                                                 x.PeriodTypeId == SystemPeriodType.HalfYear
                                                && x.CreatedTime.Year == DateTime.Today.Year
                                                && x.Period == Quarter.Q2
                                                && x.ScoreType == ScoreType.L2
                                                && accountId == x.AccountId
                                                && l1.Manager == x.ScoreBy
                                                && x.AccountId != l1.Manager)
                                            .Select(x => x.Point)
                                            .FirstOrDefaultAsync();
                    //end

                    var l1Comment = await _repoComment.FindAll(x =>
                                               x.PeriodTypeId == SystemPeriodType.HalfYear
                                              && x.CreatedTime.Year == DateTime.Today.Year
                                              && x.Period == item
                                              && x.ScoreType == ScoreType.L1
                                              && accountId == x.AccountId
                                              && l1.Manager == x.CreatedBy
                                              && x.AccountId != l1.Manager)
                                            .Select(x => x.Content)
                                          .FirstOrDefaultAsync();
                    var l1Q1Comment = await _repoComment.FindAll(x =>
                                               x.PeriodTypeId == SystemPeriodType.Quarterly
                                              && x.CreatedTime.Year == DateTime.Today.Year
                                              && x.Period == Quarter.Q1
                                              && x.ScoreType == ScoreType.L1
                                              && accountId == x.AccountId
                                              && l1.Manager == x.CreatedBy
                                              && x.AccountId != l1.Manager)
                                            .Select(x => x.Content)
                                          .FirstOrDefaultAsync();
                    var l1Q2Comment = await _repoComment.FindAll(x =>
                                               x.PeriodTypeId == SystemPeriodType.Quarterly
                                              && x.CreatedTime.Year == DateTime.Today.Year
                                              && x.Period == Quarter.Q2
                                              && x.ScoreType == ScoreType.L1
                                              && accountId == x.AccountId
                                              && l1.Manager == x.CreatedBy
                                              && x.AccountId != l1.Manager)
                                            .Select(x => x.Content)
                                          .FirstOrDefaultAsync();
                    var l1H1Comment = await _repoComment.FindAll(x =>
                                               x.PeriodTypeId == SystemPeriodType.HalfYear
                                              && x.CreatedTime.Year == DateTime.Today.Year
                                              && x.Period == item
                                              && x.ScoreType == ScoreType.L1
                                              && accountId == x.AccountId
                                              && l1.Manager == x.CreatedBy
                                              && x.AccountId != l1.Manager)
                                            .Select(x => x.Content)
                                          .FirstOrDefaultAsync();

                    double? l2ScoreKPI = await _repoAttitudeScore.FindAll(x =>
                                               x.PeriodTypeId == SystemPeriodType.HalfYear
                                              && x.CreatedTime.Year == DateTime.Today.Year
                                              && x.Period == item
                                              && x.ScoreType == ScoreType.L2
                                              && accountId == x.AccountId)
                                            .Select(x => x.Point)
                                          .FirstOrDefaultAsync();
                    var l2Comment = await _repoComment.FindAll(x =>
                                               x.PeriodTypeId == SystemPeriodType.HalfYear
                                              && x.CreatedTime.Year == DateTime.Today.Year
                                              && x.Period == item
                                              && x.ScoreType == ScoreType.L2
                                              && accountId == x.AccountId)
                                            .Select(x => x.Content)
                                          .FirstOrDefaultAsync();
                    var l2H1Comment = await _repoComment.FindAll(x =>
                                               x.PeriodTypeId == SystemPeriodType.HalfYear
                                              && x.CreatedTime.Year == DateTime.Today.Year
                                              && x.Period == item
                                              && x.ScoreType == ScoreType.L2
                                              && accountId == x.AccountId)
                                            .Select(x => x.Content)
                                          .FirstOrDefaultAsync();

                    var FLH1Comment = await _repoComment.FindAll(x =>
                                               x.PeriodTypeId == SystemPeriodType.HalfYear
                                              && x.CreatedTime.Year == DateTime.Today.Year
                                              && x.Period == item
                                              && x.ScoreType == ScoreType.FunctionalLeader
                                              && accountId == x.AccountId)
                                            .Select(x => x.Content)
                                          .FirstOrDefaultAsync();
                    var l2Q1Comment = await _repoComment.FindAll(x =>
                                               x.PeriodTypeId == SystemPeriodType.Quarterly
                                              && x.CreatedTime.Year == DateTime.Today.Year
                                              && x.Period == Quarter.Q1
                                              && x.ScoreType == ScoreType.L2
                                              && accountId == x.AccountId)
                                            .Select(x => x.Content)
                                          .FirstOrDefaultAsync();
                    var l2Q2Comment = await _repoComment.FindAll(x =>
                                               x.PeriodTypeId == SystemPeriodType.Quarterly
                                              && x.CreatedTime.Year == DateTime.Today.Year
                                              && x.Period == Quarter.Q2
                                              && x.ScoreType == ScoreType.L2
                                              && accountId == x.AccountId)
                                            .Select(x => x.Content)
                                          .FirstOrDefaultAsync();

                    double? ghrScore = await _repoKPIScore.FindAll(x =>
                                              x.PeriodTypeId == SystemPeriodType.HalfYear
                                             && x.CreatedTime.Year == DateTime.Today.Year
                                             && x.Period == item
                                             && x.ScoreType == ScoreType.GHR
                                             && accountId == x.AccountId)
                                           .Select(x => x.Point)
                                         .FirstOrDefaultAsync();
                    double? FLScore = await _repoAttitudeScore.FindAll(x =>
                                              x.PeriodTypeId == SystemPeriodType.HalfYear
                                             && x.CreatedTime.Year == DateTime.Today.Year
                                             && x.Period == item
                                             && x.ScoreType == ScoreType.FunctionalLeader
                                             && leader_id == x.ScoreBy)
                                           .Select(x => x.Point)
                                         .FirstOrDefaultAsync();
                    double A_total = Math.Round(await ConvertAtotal(l1ScoreKPI ?? 0, l2ScoreKPI ?? 0, FLScore ?? 0), 2);
                    double L1 = Math.Round((l1Q1ScoreKPI + l1Q2ScoreKPI) / 2 ?? 0, 2);
                    double L2 = Math.Round((l2Q1ScoreKPI + l2Q2ScoreKPI) / 2 ?? 0, 2);
                    double Smart = Math.Round((GHRQ1ScoreKPI + GHRQ2ScoreKPI) / 2 ?? 0, 2);
                    double B_selfScore = Math.Round(await BSelfScore(l0ScoreKPI ?? 0), 2);
                    double B_L1 = Math.Round(await BL1((l1Q1ScoreKPI + l1Q2ScoreKPI) / 2 ?? 0), 2);
                    double B_L2 = Math.Round(await BL2((l2Q1ScoreKPI + l2Q2ScoreKPI) / 2 ?? 0), 2);
                    double B_Smart = Math.Round(await BSmart((GHRQ1ScoreKPI + GHRQ2ScoreKPI) / 2 ?? 0), 2);
                    dataH1 = new H1H2ReportDto(item, periods_quarterly.Value, currentYear)
                    {
                        FullName = ocuser.FullName,
                        OC = ocuser.OC,
                        L1Score = l1ScoreKPI ?? 0,
                        L1Comment = l1Comment ?? "",
                        L1Q1Comment = l1Q1Comment ?? "",
                        L1Q2Comment = l1Q2Comment ?? "",
                        L2Score = l2ScoreKPI ?? 0,
                        L2Comment = l2Comment ?? "",
                        L2Q1Comment = l2Q1Comment ?? "",
                        L2Q2Comment = l2Q2Comment ?? "",
                        SmartScore = ghrScore ?? 0,
                        L1HComment = l1H1Comment ?? "",
                        L2HComment = l2H1Comment ?? "",
                        FLHComment = FLH1Comment ?? "",
                        FLScore = FLScore ?? 0,
                        selfScore = l0ScoreKPI ?? 0,
                        SpecialScore = Score_special ?? 0,
                        L1 = L1,
                        L2 = L2,
                        Smart = Smart,
                        B_selfScore = B_selfScore,
                        B_L1 = B_L1,
                        B_L2 = B_L2,
                        B_Smart = B_Smart,
                        A_total = A_total,
                        B_total = B_selfScore + B_L1 + B_L2 + B_Smart,
                        C_total = Score_special ?? 0,
                        D_total = Math.Round(A_total + (B_selfScore + B_L1 + B_L2 + B_Smart) + (Score_special ?? 0), 2)
                    };

                }
                else
                {
                    double? l1ScoreKPI = await _repoAttitudeScore.FindAll(x =>
                                                 x.PeriodTypeId == SystemPeriodType.HalfYear
                                                && x.CreatedTime.Year == DateTime.Today.Year
                                                && x.Period == item
                                                && x.ScoreType == ScoreType.L1
                                                && accountId == x.AccountId
                                                && l1.Manager == x.ScoreBy
                                                && x.AccountId != l1.Manager)
                                            .Select(x => x.Point)
                                            .FirstOrDefaultAsync();
                    //start
                    double? l0ScoreKPI = await _repoKPIScore.FindAll(x =>
                                                 x.PeriodTypeId == SystemPeriodType.Quarterly
                                                && x.CreatedTime.Year == DateTime.Today.Year
                                                && x.Period == Quarter.Q4
                                                && x.ScoreType == ScoreType.L0
                                                && accountId == x.AccountId
                                                && l1.Manager == x.ScoreBy
                                                && x.AccountId != l1.Manager)
                                            .Select(x => x.Point)
                                            .FirstOrDefaultAsync();
                    double? l1Q3ScoreKPI = await _repoKPIScore.FindAll(x =>
                                                 x.PeriodTypeId == SystemPeriodType.Quarterly
                                                && x.CreatedTime.Year == DateTime.Today.Year
                                                && x.Period == Quarter.Q3
                                                && x.ScoreType == ScoreType.L1
                                                && accountId == x.AccountId
                                                && l1.Manager == x.ScoreBy
                                                && x.AccountId != l1.Manager)
                                            .Select(x => x.Point)
                                            .FirstOrDefaultAsync();
                    double? l1Q4ScoreKPI = await _repoKPIScore.FindAll(x =>
                                                 x.PeriodTypeId == SystemPeriodType.Quarterly
                                                && x.CreatedTime.Year == DateTime.Today.Year
                                                && x.Period == Quarter.Q4
                                                && x.ScoreType == ScoreType.L1
                                                && accountId == x.AccountId
                                                && l1.Manager == x.ScoreBy
                                                && x.AccountId != l1.Manager)
                                            .Select(x => x.Point)
                                            .FirstOrDefaultAsync();
                    double? l2Q3ScoreKPI = await _repoKPIScore.FindAll(x =>
                                                 x.PeriodTypeId == SystemPeriodType.Quarterly
                                                && x.CreatedTime.Year == DateTime.Today.Year
                                                && x.Period == Quarter.Q3
                                                && x.ScoreType == ScoreType.L2
                                                && accountId == x.AccountId
                                                && l1.Manager == x.ScoreBy
                                                && x.AccountId != l1.Manager)
                                            .Select(x => x.Point)
                                            .FirstOrDefaultAsync();
                    double? l2Q4ScoreKPI = await _repoKPIScore.FindAll(x =>
                                                 x.PeriodTypeId == SystemPeriodType.Quarterly
                                                && x.CreatedTime.Year == DateTime.Today.Year
                                                && x.Period == Quarter.Q4
                                                && x.ScoreType == ScoreType.L2
                                                && accountId == x.AccountId
                                                && l1.Manager == x.ScoreBy
                                                && x.AccountId != l1.Manager)
                                            .Select(x => x.Point)
                                            .FirstOrDefaultAsync();
                    double? GHRQ3ScoreKPI = await _repoKPIScore.FindAll(x =>
                                                 x.PeriodTypeId == SystemPeriodType.Quarterly
                                                && x.CreatedTime.Year == DateTime.Today.Year
                                                && x.Period == Quarter.Q3
                                                && x.ScoreType == ScoreType.GHR
                                                && accountId == x.AccountId
                                                && l1.Manager == x.ScoreBy
                                                && x.AccountId != l1.Manager)
                                            .Select(x => x.Point)
                                            .FirstOrDefaultAsync();

                    double? GHRQ4ScoreKPI = await _repoKPIScore.FindAll(x =>
                                                 x.PeriodTypeId == SystemPeriodType.Quarterly
                                                && x.CreatedTime.Year == DateTime.Today.Year
                                                && x.Period == Quarter.Q4
                                                && x.ScoreType == ScoreType.GHR
                                                && accountId == x.AccountId
                                                && l1.Manager == x.ScoreBy
                                                && x.AccountId != l1.Manager)
                                            .Select(x => x.Point)
                                            .FirstOrDefaultAsync();

                    double? Score_special = await _repoSpecial.FindAll(x =>
                                                 x.PeriodTypeId == SystemPeriodType.HalfYear
                                                && x.CreatedTime.Year == DateTime.Today.Year
                                                && x.Period == Quarter.Q4
                                                && x.ScoreType == ScoreType.L2
                                                && accountId == x.AccountId
                                                && l1.Manager == x.ScoreBy
                                                && x.AccountId != l1.Manager)
                                            .Select(x => x.Point)
                                            .FirstOrDefaultAsync();
                    //end
                    var l1Comment = await _repoComment.FindAll(x =>
                                               x.PeriodTypeId == SystemPeriodType.HalfYear
                                              && x.CreatedTime.Year == DateTime.Today.Year
                                              && x.Period == item
                                              && x.ScoreType == ScoreType.L1
                                              && accountId == x.AccountId
                                              && l1.Manager == x.CreatedBy
                                              && x.AccountId != l1.Manager)
                                            .Select(x => x.Content)
                                          .FirstOrDefaultAsync();
                    var l1H2Comment = await _repoComment.FindAll(x =>
                                               x.PeriodTypeId == SystemPeriodType.HalfYear
                                              && x.CreatedTime.Year == DateTime.Today.Year
                                              && x.Period == item
                                              && x.ScoreType == ScoreType.L1
                                              && accountId == x.AccountId
                                              && l1.Manager == x.CreatedBy
                                              && x.AccountId != l1.Manager)
                                            .Select(x => x.Content)
                                          .FirstOrDefaultAsync();

                    var FLH2Comment = await _repoComment.FindAll(x =>
                                               x.PeriodTypeId == SystemPeriodType.HalfYear
                                              && x.CreatedTime.Year == DateTime.Today.Year
                                              && x.Period == item
                                              && x.ScoreType == ScoreType.FunctionalLeader
                                              && accountId == x.AccountId)
                                            .Select(x => x.Content)
                                          .FirstOrDefaultAsync();
                    var l1Q3Comment = await _repoComment.FindAll(x =>
                                               x.PeriodTypeId == SystemPeriodType.Quarterly
                                              && x.CreatedTime.Year == DateTime.Today.Year
                                              && x.Period == Quarter.Q3
                                              && x.ScoreType == ScoreType.L1
                                              && accountId == x.AccountId
                                              && l1.Manager == x.CreatedBy
                                              && x.AccountId != l1.Manager)
                                            .Select(x => x.Content)
                                          .FirstOrDefaultAsync();
                    var l1Q4Comment = await _repoComment.FindAll(x =>
                                               x.PeriodTypeId == SystemPeriodType.Quarterly
                                              && x.CreatedTime.Year == DateTime.Today.Year
                                              && x.Period == Quarter.Q4
                                              && x.ScoreType == ScoreType.L1
                                              && accountId == x.AccountId
                                              && l1.Manager == x.CreatedBy
                                              && x.AccountId != l1.Manager)
                                            .Select(x => x.Content)
                                          .FirstOrDefaultAsync();

                    double? l2ScoreKPI = await _repoAttitudeScore.FindAll(x =>
                                               x.PeriodTypeId == SystemPeriodType.HalfYear
                                              && x.CreatedTime.Year == DateTime.Today.Year
                                              && x.Period == item
                                              && x.ScoreType == ScoreType.L2
                                              && accountId == x.AccountId)
                                            .Select(x => x.Point)
                                          .FirstOrDefaultAsync();
                    var l2Comment = await _repoComment.FindAll(x =>
                                               x.PeriodTypeId == SystemPeriodType.HalfYear
                                              && x.CreatedTime.Year == DateTime.Today.Year
                                              && x.Period == item
                                              && x.ScoreType == ScoreType.L2
                                              && accountId == x.AccountId)
                                            .Select(x => x.Content)
                                          .FirstOrDefaultAsync();
                    var l2H2Comment = await _repoComment.FindAll(x =>
                                               x.PeriodTypeId == SystemPeriodType.HalfYear
                                              && x.CreatedTime.Year == DateTime.Today.Year
                                              && x.Period == item
                                              && x.ScoreType == ScoreType.L2
                                              && accountId == x.AccountId)
                                            .Select(x => x.Content)
                                          .FirstOrDefaultAsync();
                    var l2Q3Comment = await _repoComment.FindAll(x =>
                                               x.PeriodTypeId == SystemPeriodType.Quarterly
                                              && x.CreatedTime.Year == DateTime.Today.Year
                                              && x.Period == Quarter.Q3
                                              && x.ScoreType == ScoreType.L2
                                              && accountId == x.AccountId)
                                            .Select(x => x.Content)
                                          .FirstOrDefaultAsync();
                    var l2Q4Comment = await _repoComment.FindAll(x =>
                                               x.PeriodTypeId == SystemPeriodType.Quarterly
                                              && x.CreatedTime.Year == DateTime.Today.Year
                                              && x.Period == Quarter.Q4
                                              && x.ScoreType == ScoreType.L2
                                              && accountId == x.AccountId)
                                            .Select(x => x.Content)
                                          .FirstOrDefaultAsync();

                    double? ghrScore = await _repoKPIScore.FindAll(x =>
                                              x.PeriodTypeId == SystemPeriodType.HalfYear
                                             && x.CreatedTime.Year == DateTime.Today.Year
                                             && x.Period == item
                                             && x.ScoreType == ScoreType.GHR
                                             && accountId == x.AccountId)
                                           .Select(x => x.Point)
                                         .FirstOrDefaultAsync();
                    double? FLScore = await _repoAttitudeScore.FindAll(x =>
                                              x.PeriodTypeId == SystemPeriodType.HalfYear
                                             && x.CreatedTime.Year == DateTime.Today.Year
                                             && x.Period == item
                                             && x.ScoreType == ScoreType.FunctionalLeader
                                             && leader_id == x.ScoreBy)
                                           .Select(x => x.Point)
                                         .FirstOrDefaultAsync();
                    double A_total = Math.Round(await ConvertAtotal(l1ScoreKPI ?? 0, l2ScoreKPI ?? 0, FLScore ?? 0), 2);
                    double L1 = Math.Round((l1Q3ScoreKPI + l1Q4ScoreKPI) / 2 ?? 0, 2);
                    double L2 = Math.Round((l2Q3ScoreKPI + l2Q4ScoreKPI) / 2 ?? 0, 2);
                    double Smart = Math.Round((GHRQ3ScoreKPI + GHRQ4ScoreKPI) / 2 ?? 0, 2);
                    double B_selfScore = Math.Round(await BSelfScore(l0ScoreKPI ?? 0), 2);
                    double B_L1 = Math.Round(await BL1((l1Q3ScoreKPI + l1Q4ScoreKPI) / 2 ?? 0), 2);
                    double B_L2 = Math.Round(await BL2((l2Q3ScoreKPI + l2Q4ScoreKPI) / 2 ?? 0), 2);
                    double B_Smart = Math.Round(await BSmart((GHRQ3ScoreKPI + GHRQ4ScoreKPI) / 2 ?? 0), 2);
                    dataH2 = new H1H2ReportDto(item, periods_quarterly.Value, currentYear)
                    {
                        FullName = ocuser.FullName,
                        OC = ocuser.OC,
                        L1Score = l1ScoreKPI ?? 0,
                        L1Comment = l1Comment ?? "",
                        L1Q1Comment = l1Q3Comment ?? "",
                        L1Q2Comment = l1Q4Comment ?? "",
                        L2Score = l2ScoreKPI ?? 0,
                        L2Comment = l2Comment ?? "",
                        L2Q1Comment = l2Q3Comment ?? "",
                        L2Q2Comment = l2Q4Comment ?? "",
                        L1HComment = l1H2Comment ?? "",
                        L2HComment = l2H2Comment ?? "",
                        FLHComment = FLH2Comment ?? "",
                        SmartScore = ghrScore ?? 0,
                        FLScore = FLScore ?? 0,
                        selfScore = l0ScoreKPI ?? 0,
                        SpecialScore = Score_special ?? 0,
                        L1 = L1,
                        L2 = L2,
                        Smart = Smart,
                        B_selfScore = B_selfScore,
                        B_L1 = B_L1,
                        B_L2 = B_L2,
                        B_Smart = B_Smart,
                        A_total = A_total,
                        B_total = B_selfScore + B_L1 + B_L2 + B_Smart,
                        C_total = Score_special ?? 0,
                        D_total = Math.Round(A_total + (B_selfScore + B_L1 + B_L2 + B_Smart) + (Score_special ?? 0), 2)

                    };
                }
            }

            return new
            {
                H1 = dataH1,
                H2 = dataH2,
                avg = Math.Round((dataH1.D_total + dataH2.D_total) / 2, 2)
            };
        }
        public async Task<double> BSelfScore(double number)
        {
            double result = 0;
            try
            {
                if (number == 5)
                {
                    result = 10;
                }
                if (number == 0)
                {
                    return result;
                }
                if (number > 0  && number < 5)
                {
                    result = number * 10 / 5;
                }
                
                return result;
            }
            catch (Exception ex)
            {

                return result;
            }

        }
        public async Task<double> BL1(double number)
        {
            double result = 0;
            try
            {
                if (number == 5)
                {
                    result = 30;
                }
                if (number == 0)
                {
                    return result;
                }
                if (number > 0 && number < 5)
                {
                    result = number * 30 / 5;
                }

                return result;
            }
            catch (Exception ex)
            {

                return result;
            }

        }
        public async Task<double> BSmart(double number)
        {
            double result = 0;
            try
            {
                if (number == 5)
                {
                    result = 20;
                }
                if (number == 0)
                {
                    return result;
                }
                if (number > 0 && number < 5)
                {
                    result = number * 20 / 5;
                }

                return result;
            }
            catch (Exception ex)
            {

                return result;
            }

        }

        public async Task<double> BL2(double number)
        {
            double result = 0;
            try
            {
                if (number == 5)
                {
                    result = 20;
                }
                if (number == 0)
                {
                    return result;
                }
                if (number > 0 && number < 5)
                {
                    result = number * 20 / 5;
                }

                return result;
            }
            catch (Exception ex)
            {

                return result;
            }

        }

        public async Task<double> ConvertAtotal(double l1ScoreKPI, double l2ScoreKPI , double FLScore)
        {
             double result = 0;  
            try
            {
                if (FLScore == 0)
                {
                    result = (l1ScoreKPI + l2ScoreKPI) / 2;
                } else
                {
                    result = (l1ScoreKPI + l2ScoreKPI + FLScore) *20 / 30;
                }
                return result;
            }
            catch (Exception ex)
            {

                return result;
            }

        }

        public async Task<object> GetH1H2Data()
        {
            //var accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            //int accountID = JWTExtensions.GetDecodeTokenById(accessToken);
            //// tim oc cua usser login
            //var ocuser = await _repoOCAccount.FindAll(x => x.AccountId == accountID).FirstOrDefaultAsync();
         
            //if (ocuser == null) return new List<dynamic>();
            //// Lay tat ca con cua oc
            //var oc = _repoOC.FindAll().AsHierarchy(x => x.Id, y => y.ParentId, ocuser.OCId).ToList();
            //var ocs = oc.Flatten(x => x.ChildNodes).Select(x => x.Entity.Id).ToList();
            //// vao ocUser tim theo ocId list 
            //var accountIds = await _repoOCAccount.FindAll(x => ocs.Contains(x.OCId)).Select(x => x.AccountId).Distinct().ToListAsync();
            //var pics = await _repoPIC.FindAll(x => accountIds.Contains(x.AccountId)).Select(x=>x.AccountId).Distinct().ToListAsync();

            //var model = await _repoOCAccount.FindAll(x => pics.Contains(x.AccountId))
            //    .Select(x => new
            //    {
            //        Id = x.AccountId,
            //        FullName = x.Account.FullName,
            //    }).ToListAsync();

            //return model;
            throw new NotImplementedException();
        }

        public Task<object> GetAllKPIScoreL0ByPeriod(int period)
        {
            throw new NotImplementedException();
        }
    }
}
