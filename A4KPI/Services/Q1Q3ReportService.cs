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
using OfficeOpenXml;
using System.IO;
using OfficeOpenXml.Style;
using System.Drawing;
using System.Collections;

namespace A4KPI.Services
{
    public interface IQ1Q3Service
    {

        Task<object> GetQ1Q3Data();
        Task<object> GetQ1Q3DataByLeo(DateTime currentTime);
        Task<Q1Q3ReportDto> GetReportInfo(int accountId);

        Task<Byte[]> ExportExcel(int accountId);
        Task<Byte[]> ExportExcelByLeo(DateTime currentTime);
    }
    public class Q1Q3Service : IQ1Q3Service
    {
        private OperationResult operationResult;
        private readonly IRepositoryBase<ToDoList> _repo;
        private readonly IRepositoryBase<Account> _repoAccount;
        private readonly IRepositoryBase<AccountGroupAccount> _repoAccountGroupAccount;
        private readonly IRepositoryBase<Objective> _repoObjective;
        private readonly IRepositoryBase<ResultOfMonth> _repoResultOfMonth;
        private readonly IRepositoryBase<KPIScore> _repoKPIScore;
        private readonly IRepositoryBase<PeriodType> _repoPeriodType;
        private readonly IRepositoryBase<OCAccount> _repoOCAccount;
        private readonly IRepositoryBase<PIC> _repoPIC;
        private readonly IRepositoryBase<Comment> _repoComment;
        private readonly IRepositoryBase<OC> _repoOC;
        private readonly IRepositoryBase<Period> _repoPeriod;
        private readonly IRepositoryBase<AttitudeScore> _repoAttitudeScore;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        public Q1Q3Service(
            IRepositoryBase<ToDoList> repo,
            IRepositoryBase<Account> repoAccount,
            IRepositoryBase<AccountGroupAccount> repoAccountGroupAccount,
            IRepositoryBase<Objective> repoObjective,
            IRepositoryBase<ResultOfMonth> repoResultOfMonth,
            IRepositoryBase<KPIScore> repoKPIScore,
            IRepositoryBase<PeriodType> repoPeriodType,
            IRepositoryBase<OCAccount> repoOCAccount,
            IRepositoryBase<PIC> repoPIC,
            IRepositoryBase<Comment> repoComment,
            IRepositoryBase<OC> repoOC,
            IRepositoryBase<Period> repoPeriod,
            IRepositoryBase<AttitudeScore> repoAttitudeScore,
            IHttpContextAccessor httpContextAccessor,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            MapperConfiguration configMapper
            )
        {
            _repo = repo;
            _repoAccount = repoAccount;
            _repoAccountGroupAccount = repoAccountGroupAccount;
            _repoObjective = repoObjective;
            _repoResultOfMonth = repoResultOfMonth;
            _repoKPIScore = repoKPIScore;
            _repoPeriodType = repoPeriodType;
            _repoOCAccount = repoOCAccount;
            _repoPIC = repoPIC;
            _repoComment = repoComment;
            _repoOC = repoOC;
            _repoPeriod = repoPeriod;
            _repoAttitudeScore = repoAttitudeScore;
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configMapper = configMapper;
        }
        public async Task<object> GetQ1Q3Data()
        {
            var accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            int accountID = JWTExtensions.GetDecodeTokenById(accessToken);
            // tim oc cua usser login
            var ocuser = await _repoOCAccount.FindAll(x => x.AccountId == accountID).FirstOrDefaultAsync();

            if (ocuser == null) return new List<dynamic>();
            // Lay tat ca con cua oc
            var oc = _repoOC.FindAll().AsHierarchy(x => x.Id, y => y.ParentId, ocuser.OCId).ToList();
            var ocs = oc.Flatten(x => x.ChildNodes).Select(x => x.Entity.Id).ToList();
            // vao ocUser tim theo ocId list 
            var accountIds = await _repoOCAccount.FindAll(x => ocs.Contains(x.OCId)).Select(x => x.AccountId).Distinct().ToListAsync();
            var pics = await _repoPIC.FindAll(x => accountIds.Contains(x.AccountId)).Select(x => x.AccountId).Distinct().ToListAsync();

            var model = await _repoOCAccount.FindAll(x => pics.Contains(x.AccountId))
                .Select(x => new
                {
                    Id = x.AccountId,
                    FullName = x.Account.FullName,
                }).ToListAsync();

            return model;
        }

        public async Task<object> GetQ1Q3DataByLeo(DateTime currentTime)
        {
            var accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            int accountID = JWTExtensions.GetDecodeTokenById(accessToken);
            // tim oc cua usser login
            var ocuser = await _repoOCAccount.FindAll(x => x.AccountId == accountID).FirstOrDefaultAsync();

            if (ocuser == null) return new List<dynamic>();
            // Lay tat ca con cua oc
            var oc = _repoOC.FindAll().AsHierarchy(x => x.Id, y => y.ParentId, ocuser.OCId).ToList();
            var ocs = oc.Flatten(x => x.ChildNodes).Select(x => x.Entity.Id).ToList();
            // vao ocUser tim theo ocId list 
            var accountIds = await _repoOCAccount.FindAll(x => ocs.Contains(x.OCId)).Select(x => x.AccountId).Distinct().ToListAsync();
            var pics = await _repoPIC.FindAll(x => accountIds.Contains(x.AccountId)).Select(x => x.AccountId).Distinct().ToListAsync();

            var model = await _repoOCAccount.FindAll(x => accountIds.Contains(x.AccountId))
                .Select(x => new
                {
                    Id = x.AccountId,
                    FullName = x.Account.FullName,
                }).Where(y => y.Id != accountID).ToListAsync();
          

            var quarterlyModel2 = (await _repoPeriod.FindAll(x => x.PeriodType.Code == SystemPeriod.Quarterly).Select(
                x => new
                {
                    x.Value,
                    x.Title,
                    x.Months
                }).ToListAsync()).Select(x => new
                {
                    x.Value,
                    x.Title,
                    Months = x.Months.Split(",").Select(int.Parse).ToList()
                    
                });
            //var currentDate = DateTime.Today;
            var currentDate = currentTime;
            var currentMonth = currentDate.Month;
            var currentYear = currentDate.Year;
            var currentQuarter = quarterlyModel2.FirstOrDefault(x => x.Months.Contains(currentMonth)).Value;
            var data = new List<Q1Q3ReportDto>();
            if (currentQuarter == Quarter.Q1 || currentQuarter == Quarter.Q3)
            {
                foreach (var item in model)
                {
                    var ocuser2 = await _repoOCAccount.FindAll(x => x.AccountId == item.Id)
                    .Select(x => new
                    {
                        OC = x.OC.Name,
                        OCId = x.OC.ParentId,
                        FullName = x.Account.FullName
                    }).FirstOrDefaultAsync();
                        if (ocuser == null) return new Q1Q3ReportDto();

                        //var currentDate1 = DateTime.Today;
                        //var currentMonth1 = currentDate.Month + "";
                        //var currentYear1 = currentDate.Year;

                        var quarterly = await _repoPeriodType.FindAll(x => x.Code == SystemPeriod.Quarterly).FirstOrDefaultAsync();
                        var periods = quarterly.Periods.Select(x => new
                        {
                            x.Title,
                            x.Value,
                            x.ReportTime,
                            Months = x.Months.Split(',').ToList()
                        }).Where(x => x.Months.Contains(currentMonth.ToString())).FirstOrDefault();

                        var l1 = await _repoAccount.FindAll(x => x.Id == item.Id).FirstOrDefaultAsync();
                        if (l1 == null) return new Q1Q3ReportDto();
                        double? l1ScoreKPI = await _repoKPIScore.FindAll(x =>
                                                     x.PeriodTypeId == SystemPeriodType.Quarterly
                                                    && x.CreatedTime.Year == DateTime.Today.Year
                                                    && x.Period == periods.Value
                                                    && x.ScoreType == ScoreType.L1
                                                    && item.Id == x.AccountId
                                                    && l1.Manager == x.ScoreBy
                                                    && x.AccountId != l1.Manager)
                                                .Select(x => x.Point)
                                                .FirstOrDefaultAsync();
                        var l1Comment = await _repoComment.FindAll(x =>
                                                   x.PeriodTypeId == SystemPeriodType.Quarterly
                                                  && x.CreatedTime.Year == DateTime.Today.Year
                                                  && x.Period == periods.Value
                                                  && x.ScoreType == ScoreType.L1
                                                  && item.Id == x.AccountId
                                                  && l1.Manager == x.CreatedBy
                                                  && x.AccountId != l1.Manager)
                                                .Select(x => x.Content)
                                              .FirstOrDefaultAsync();

                        double? l2ScoreKPI = await _repoKPIScore.FindAll(x =>
                                                   x.PeriodTypeId == SystemPeriodType.Quarterly
                                                  && x.CreatedTime.Year == DateTime.Today.Year
                                                  && x.Period == periods.Value
                                                  && x.ScoreType == ScoreType.L2
                                                  && item.Id == x.AccountId)
                                                .Select(x => x.Point)
                                              .FirstOrDefaultAsync();
                        var l2Comment = await _repoComment.FindAll(x =>
                                                   x.PeriodTypeId == SystemPeriodType.Quarterly
                                                  && x.CreatedTime.Year == DateTime.Today.Year
                                                  && x.Period == periods.Value
                                                  && x.ScoreType == ScoreType.L2
                                                  && item.Id == x.AccountId)
                                                .Select(x => x.Content)
                                              .FirstOrDefaultAsync();

                        double? ghrScore = await _repoKPIScore.FindAll(x =>
                                                  x.PeriodTypeId == SystemPeriodType.Quarterly
                                                 && x.CreatedTime.Year == DateTime.Today.Year
                                                 && x.Period == periods.Value
                                                 && x.ScoreType == ScoreType.GHR
                                                 && item.Id == x.AccountId)
                                               .Select(x => x.Point)
                                             .FirstOrDefaultAsync();
                   var data2 = new Q1Q3ReportDto(periods.Value, currentYear)
                    {
                        FullName = ocuser2.FullName,
                        OC = ocuser2.OC,
                        L1Score = l1ScoreKPI ?? 0,
                        L1Comment = l1Comment ?? "",
                        L2Score = l2ScoreKPI ?? 0,
                        L2Comment = l2Comment ?? "",
                        SmartScore = ghrScore ?? 0,

                    };
                    data.Add(data2);
                }
                // do somthing
            } else
            {
                return new Q1Q3ReportDto();
            }
            return new {
                data,
                currentQuarter
            };
        }

        public async Task<object> GetQ1Q3DataByLeoExcel(DateTime currentTime)
        {
            var accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            int accountID = JWTExtensions.GetDecodeTokenById(accessToken);
            // tim oc cua usser login
            var ocuser = await _repoOCAccount.FindAll(x => x.AccountId == accountID).FirstOrDefaultAsync();

            if (ocuser == null) return new List<dynamic>();
            // Lay tat ca con cua oc
            var oc = _repoOC.FindAll().AsHierarchy(x => x.Id, y => y.ParentId, ocuser.OCId).ToList();
            var ocs = oc.Flatten(x => x.ChildNodes).Select(x => x.Entity.Id).ToList();
            // vao ocUser tim theo ocId list 
            var accountIds = await _repoOCAccount.FindAll(x => ocs.Contains(x.OCId)).Select(x => x.AccountId).Distinct().ToListAsync();
            var pics = await _repoPIC.FindAll(x => accountIds.Contains(x.AccountId)).Select(x => x.AccountId).Distinct().ToListAsync();

            var model = await _repoOCAccount.FindAll(x => accountIds.Contains(x.AccountId))
                .Select(x => new
                {
                    Id = x.AccountId,
                    FullName = x.Account.FullName,
                }).Where(y => y.Id != accountID).ToListAsync();


            var quarterlyModel2 = (await _repoPeriod.FindAll(x => x.PeriodType.Code == SystemPeriod.Quarterly).Select(
                x => new
                {
                    x.Value,
                    x.Title,
                    x.Months
                }).ToListAsync()).Select(x => new
                {
                    x.Value,
                    x.Title,
                    Months = x.Months.Split(",").Select(int.Parse).ToList()
                });
            //var currentDate = DateTime.Today;
            var currentDate = currentTime;
            var currentMonth = currentDate.Month ;
            var currentYear = currentDate.Year;
            var currentQuarter = quarterlyModel2.FirstOrDefault(x => x.Months.Contains(currentMonth)).Value;

            var data = new List<Q1Q3ReportDto>();
            if (currentQuarter == Quarter.Q1 || currentQuarter == Quarter.Q3)
            {
                foreach (var item in model)
                {
                    var ocuser2 = await _repoOCAccount.FindAll(x => x.AccountId == item.Id)
                    .Select(x => new
                    {
                        OC = x.OC.Name,
                        OCId = x.OC.ParentId,
                        FullName = x.Account.FullName
                    }).FirstOrDefaultAsync();
                    if (ocuser == null) return new Q1Q3ReportDto();

                    //var currentDate1 = DateTime.Today;
                    //var currentMonth1 = currentDate.Month + "";
                    //var currentYear1 = currentDate.Year;

                    var quarterly = await _repoPeriodType.FindAll(x => x.Code == SystemPeriod.Quarterly).FirstOrDefaultAsync();
                    var periods = quarterly.Periods.Select(x => new
                    {
                        x.Title,
                        x.Value,
                        x.ReportTime,
                        Months = x.Months.Split(',').ToList()
                    }).Where(x => x.Months.Contains(currentMonth.ToString())).FirstOrDefault();

                    var l1 = await _repoAccount.FindAll(x => x.Id == item.Id).FirstOrDefaultAsync();
                    if (l1 == null) return new Q1Q3ReportDto();
                    double? l1ScoreKPI = await _repoKPIScore.FindAll(x =>
                                                 x.PeriodTypeId == SystemPeriodType.Quarterly
                                                && x.CreatedTime.Year == DateTime.Today.Year
                                                && x.Period == periods.Value
                                                && x.ScoreType == ScoreType.L1
                                                && item.Id == x.AccountId
                                                && l1.Manager == x.ScoreBy
                                                && x.AccountId != l1.Manager)
                                            .Select(x => x.Point)
                                            .FirstOrDefaultAsync();
                    var l1Comment = await _repoComment.FindAll(x =>
                                               x.PeriodTypeId == SystemPeriodType.Quarterly
                                              && x.CreatedTime.Year == DateTime.Today.Year
                                              && x.Period == periods.Value
                                              && x.ScoreType == ScoreType.L1
                                              && item.Id == x.AccountId
                                              && l1.Manager == x.CreatedBy
                                              && x.AccountId != l1.Manager)
                                            .Select(x => x.Content)
                                          .FirstOrDefaultAsync();

                    double? l2ScoreKPI = await _repoKPIScore.FindAll(x =>
                                               x.PeriodTypeId == SystemPeriodType.Quarterly
                                              && x.CreatedTime.Year == DateTime.Today.Year
                                              && x.Period == periods.Value
                                              && x.ScoreType == ScoreType.L2
                                              && item.Id == x.AccountId)
                                            .Select(x => x.Point)
                                          .FirstOrDefaultAsync();
                    var l2Comment = await _repoComment.FindAll(x =>
                                               x.PeriodTypeId == SystemPeriodType.Quarterly
                                              && x.CreatedTime.Year == DateTime.Today.Year
                                              && x.Period == periods.Value
                                              && x.ScoreType == ScoreType.L2
                                              && item.Id == x.AccountId)
                                            .Select(x => x.Content)
                                          .FirstOrDefaultAsync();

                    double? ghrScore = await _repoKPIScore.FindAll(x =>
                                              x.PeriodTypeId == SystemPeriodType.Quarterly
                                             && x.CreatedTime.Year == DateTime.Today.Year
                                             && x.Period == periods.Value
                                             && x.ScoreType == ScoreType.GHR
                                             && item.Id == x.AccountId)
                                           .Select(x => x.Point)
                                         .FirstOrDefaultAsync();
                    var data2 = new Q1Q3ReportDto(periods.Value, currentYear)
                    {
                        FullName = ocuser2.FullName,
                        OC = ocuser2.OC,
                        L1Score = l1ScoreKPI ?? 0,
                        L1Comment = l1Comment ?? "",
                        L2Score = l2ScoreKPI ?? 0,
                        L2Comment = l2Comment ?? "",
                        SmartScore = ghrScore ?? 0,

                    };
                    data.Add(data2);
                }
                // do somthing
            }
            else
            {
                return new Q1Q3ReportDto();
            }
            return data.ToList();
            
        }
        public async Task<Q1Q3ReportDto> GetReportInfo(int accountId)
        {
            var ocuser = await _repoOCAccount.FindAll(x => x.AccountId == accountId)
             .Select(x => new
             {
                 OC = x.OC.Name,
                 OCId = x.OC.ParentId,
                 FullName = x.Account.FullName
             }).FirstOrDefaultAsync();
            if (ocuser == null) return new Q1Q3ReportDto();

            var currentDate = DateTime.Today;
            var currentMonth = currentDate.Month + 1 + "";
            var currentYear = currentDate.Year;

            var quarterly = await _repoPeriodType.FindAll(x => x.Code == SystemPeriod.Quarterly).FirstOrDefaultAsync();
            var periods = quarterly.Periods.Select(x => new
            {
                x.Title,
                x.Value,
                x.ReportTime,
                Months = x.Months.Split(',').ToList()
            }).Where(x => x.Months.Contains(currentMonth)).FirstOrDefault();

            var l1 = await _repoAccount.FindAll(x => x.Id == accountId).FirstOrDefaultAsync();
            if (l1 == null) return new Q1Q3ReportDto();
            double? l1ScoreKPI = await _repoKPIScore.FindAll(x =>
                                         x.PeriodTypeId == SystemPeriodType.Quarterly
                                        && x.CreatedTime.Year == DateTime.Today.Year
                                        && x.Period == periods.Value
                                        && x.ScoreType == ScoreType.L1
                                        && accountId == x.AccountId
                                        && l1.Manager == x.ScoreBy
                                        && x.AccountId != l1.Manager)
                                    .Select(x => x.Point)
                                    .FirstOrDefaultAsync();
            var l1Comment = await _repoComment.FindAll(x =>
                                       x.PeriodTypeId == SystemPeriodType.Quarterly
                                      && x.CreatedTime.Year == DateTime.Today.Year
                                      && x.Period == periods.Value
                                      && x.ScoreType == ScoreType.L1
                                      && accountId == x.AccountId
                                      && l1.Manager == x.CreatedBy
                                      && x.AccountId != l1.Manager)
                                    .Select(x => x.Content)
                                  .FirstOrDefaultAsync();

            double? l2ScoreKPI = await _repoKPIScore.FindAll(x =>
                                       x.PeriodTypeId == SystemPeriodType.Quarterly
                                      && x.CreatedTime.Year == DateTime.Today.Year
                                      && x.Period == periods.Value
                                      && x.ScoreType == ScoreType.L2
                                      && accountId == x.AccountId)
                                    .Select(x => x.Point)
                                  .FirstOrDefaultAsync();
            var l2Comment = await _repoComment.FindAll(x =>
                                       x.PeriodTypeId == SystemPeriodType.Quarterly
                                      && x.CreatedTime.Year == DateTime.Today.Year
                                      && x.Period == periods.Value
                                      && x.ScoreType == ScoreType.L2
                                      && accountId == x.AccountId)
                                    .Select(x => x.Content)
                                  .FirstOrDefaultAsync();

            double? ghrScore = await _repoKPIScore.FindAll(x =>
                                      x.PeriodTypeId == SystemPeriodType.Quarterly
                                     && x.CreatedTime.Year == DateTime.Today.Year
                                     && x.Period == periods.Value
                                     && x.ScoreType == ScoreType.GHR
                                     && accountId == x.AccountId)
                                   .Select(x => x.Point)
                                 .FirstOrDefaultAsync();
            var data = new Q1Q3ReportDto(periods.Value, currentYear)
            {
                FullName = ocuser.FullName,
                OC = ocuser.OC,
                L1Score = l1ScoreKPI ?? 0,
                L1Comment = l1Comment ?? "",
                L2Score = l2ScoreKPI ?? 0,
                L2Comment = l2Comment ?? "",
                SmartScore = ghrScore ?? 0,

            };
            
            return data;
        }

        public async Task<Q1Q3ReportDto> GetReportInfo2(int accountId)
        {
            var ocuser = await _repoOCAccount.FindAll(x => x.AccountId == accountId)
             .Select(x => new
             {
                 OC = x.OC.Name,
                 OCId = x.OC.ParentId,
                 FullName = x.Account.FullName
             }).FirstOrDefaultAsync();
            if (ocuser == null) return new Q1Q3ReportDto();

            var currentDate = DateTime.Today;
            var currentMonth = currentDate.Month + "1";
            var currentYear = currentDate.Year;

            var quarterly = await _repoPeriodType.FindAll(x => x.Code == SystemPeriod.Quarterly).FirstOrDefaultAsync();
            var quarterlyModel2 = (await _repoPeriod.FindAll(x => x.PeriodType.Code == SystemPeriod.Quarterly).Select(
              x=> new
              {
                  x.Value,
                  x.Title,
                  x.Months
              }).ToListAsync()).Select (x=> new
              {
                  x.Value,
                  x.Title,
                  Months = x.Months.Split(",").Select(int.Parse).ToList()
              });

            var currentMonthQuarter = DateTime.Today.Month;
            var currentQuarter = quarterlyModel2.FirstOrDefault(x=> x.Months.Contains(currentMonthQuarter)).Value;
            if (currentQuarter == Quarter.Q1|| currentQuarter == Quarter.Q3)
            {
                // do somthing
            }
            var periods = quarterly.Periods.Select(x => new
            {
                x.Title,
                x.Value,
                x.ReportTime,
                Months = x.Months.Split(',').ToList()
            }).Where(x => x.Months.Contains(currentMonth)).FirstOrDefault();

            var l1 = await _repoAccount.FindAll().FirstOrDefaultAsync();
            if (l1 == null) return new Q1Q3ReportDto();
            double? l1ScoreKPI = await _repoKPIScore.FindAll(x =>
                                         x.PeriodTypeId == SystemPeriodType.Quarterly
                                        && x.CreatedTime.Year == DateTime.Today.Year
                                        && x.Period == periods.Value
                                        && x.ScoreType == ScoreType.L1
                                        && accountId == x.AccountId
                                        && l1.Manager == x.ScoreBy
                                        && x.AccountId != l1.Manager)
                                    .Select(x => x.Point)
                                    .FirstOrDefaultAsync();
            var l1Comment = await _repoComment.FindAll(x =>
                                       x.PeriodTypeId == SystemPeriodType.Quarterly
                                      && x.CreatedTime.Year == DateTime.Today.Year
                                      && x.Period == periods.Value
                                      && x.ScoreType == ScoreType.L1
                                      && accountId == x.AccountId
                                      && l1.Manager == x.CreatedBy
                                      && x.AccountId != l1.Manager)
                                    .Select(x => x.Content)
                                  .FirstOrDefaultAsync();

            double? l2ScoreKPI = await _repoKPIScore.FindAll(x =>
                                       x.PeriodTypeId == SystemPeriodType.Quarterly
                                      && x.CreatedTime.Year == DateTime.Today.Year
                                      && x.Period == periods.Value
                                      && x.ScoreType == ScoreType.L2
                                      && accountId == x.AccountId)
                                    .Select(x => x.Point)
                                  .FirstOrDefaultAsync();
            var l2Comment = await _repoComment.FindAll(x =>
                                       x.PeriodTypeId == SystemPeriodType.Quarterly
                                      && x.CreatedTime.Year == DateTime.Today.Year
                                      && x.Period == periods.Value
                                      && x.ScoreType == ScoreType.L2
                                      && accountId == x.AccountId)
                                    .Select(x => x.Content)
                                  .FirstOrDefaultAsync();

            double? ghrScore = await _repoKPIScore.FindAll(x =>
                                      x.PeriodTypeId == SystemPeriodType.Quarterly
                                     && x.CreatedTime.Year == DateTime.Today.Year
                                     && x.Period == periods.Value
                                     && x.ScoreType == ScoreType.GHR
                                     && accountId == x.AccountId)
                                   .Select(x => x.Point)
                                 .FirstOrDefaultAsync();
            var data = new Q1Q3ReportDto(periods.Value, currentYear)
            {
                FullName = ocuser.FullName,
                OC = ocuser.OC,
                L1Score = l1ScoreKPI ?? 0,
                L1Comment = l1Comment ?? "",
                L2Score = l2ScoreKPI ?? 0,
                L2Comment = l2Comment ?? "",
                SmartScore = ghrScore ?? 0,

            };
            
            return data;
        }
        public async Task<Byte[]> ExportExcel(int accountId)
        {
            var model = await GetReportInfo(accountId);
            try
            {
                var currentTime = DateTime.Now;
                ExcelPackage.LicenseContext = LicenseContext.Commercial;
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                var memoryStream = new MemoryStream();
                using (ExcelPackage p = new ExcelPackage(memoryStream))
                {
                    // đặt tên người tạo file
                    p.Workbook.Properties.Author = "Henry Pham";

                    // đặt tiêu đề cho file
                    p.Workbook.Properties.Title = "Q1,Q3 Report 季報表";
                    //Tạo một sheet để làm việc trên đó
                    p.Workbook.Worksheets.Add("Q1,Q3 Report 季報表");

                    // lấy sheet vừa add ra để thao tác
                    ExcelWorksheet ws = p.Workbook.Worksheets["Q1,Q3 Report 季報表"];

                    // đặt tên cho sheet
                    ws.Name = "Q1,Q3 Report 季報表";
                    // fontsize mặc định cho cả sheet
                    ws.Cells.Style.Font.Size = 11;
                    // font family mặc định cho cả sheet
                    ws.Cells.Style.Font.Name = "微軟正黑體";
                    var headerArray = new List<string>()
                    {
                        "部門單位 Dept.",
                        "姓名 Full Name",
                        "直屬主管評分 L1 score",
                        "直屬主管評語 L1 comment",
                        "二階主管評分L2 score",
                        "二階主管評語 L2 comment",
                        "SMART 分數",
                    };
                    ws.Cells["C3"].Value = "人事季報表";
                    ws.Cells["C3:I3"].Merge = true;
                    ws.Cells["C3:I3"].Style.Font.Size = 18;
                    ws.Cells["C4"].Value = "年度: " + model.Year;
                    ws.Cells["C4"].Style.Font.Size = 14;

                    ws.Cells["D4"].Value = "廠區 SHC";
                    ws.Cells["D4"].Style.Font.Size = 14;

                    ws.Cells["E4"].Value = "季度 Quarter: Q" + model.Quarter ;
                    ws.Cells["E4"].Style.Font.Size = 14;


                    ws.Cells["C5"].Value = headerArray[0];
                    ws.Cells["D5"].Value = headerArray[1];
                    ws.Cells["E5"].Value = headerArray[2];
                    ws.Cells["F5"].Value = headerArray[3];
                    ws.Cells["G5"].Value = headerArray[4];
                    ws.Cells["H5"].Value = headerArray[5];
                    ws.Cells["I5"].Value = headerArray[6];

                    ws.Cells["E5"].Style.WrapText = true;
                    ws.Cells["G5"].Style.WrapText = true;
                    ws.Cells["I5"].Style.WrapText = true;

                    ws.Cells["C6"].Value = model.OC;
                    ws.Cells["D6"].Value = model.FullName;
                    ws.Cells["E6"].Value = model.L1Score;
                    ws.Cells["F6"].Value = model.L1Comment;
                    ws.Cells["G6"].Value = model.L2Score;
                    ws.Cells["H6"].Value = model.L2Comment;
                    ws.Cells["I6"].Value = model.SmartScore;

                    //Make all text fit the cells
                    //ws.Cells[ws.Dimension.Address].AutoFitColumns();
                    ws.Cells[ws.Dimension.Address].Style.Font.Bold = true;
                    ws.Cells[ws.Dimension.Address].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells[ws.Dimension.Address].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Column(3).Width = 20;
                    ws.Column(4).Width = 30;
                    ws.Column(5).Width = 10;
                    ws.Column(6).Width = 40;
                    ws.Column(7).Width = 10;
                    ws.Column(8).Width = 40;
                    ws.Column(9).Width = 10;
                    //make the borders of cell F6 thick
                    ws.Cells["C3:I6"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    ws.Cells["C3:I6"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    ws.Cells["C3:I6"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    ws.Cells["C3:I6"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                  
                    //Lưu file lại
                    Byte[] bin = p.GetAsByteArray();
                    return bin;
                }
            }
            catch (Exception ex)
            {
                var mes = ex.Message;
                Console.WriteLine(mes);
                return new Byte[] { };
            }
        }

        public async Task<Byte[]> ExportExcelByLeo(DateTime currentTimes)
        {
            var model = await GetQ1Q3DataByLeoExcel(currentTimes);
            try
            {
                //var currentTime = DateTime.Now;// test
                //var currentTime = currentTimes;
                ExcelPackage.LicenseContext = LicenseContext.Commercial;
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                var memoryStream = new MemoryStream();
                using (ExcelPackage p = new ExcelPackage(memoryStream))
                {
                    // đặt tên người tạo file
                    p.Workbook.Properties.Author = "Henry Pham";

                    // đặt tiêu đề cho file
                    p.Workbook.Properties.Title = "Q1,Q3 Report 季報表";
                    //Tạo một sheet để làm việc trên đó
                    p.Workbook.Worksheets.Add("Q1,Q3 Report 季報表");

                    // lấy sheet vừa add ra để thao tác
                    ExcelWorksheet ws = p.Workbook.Worksheets["Q1,Q3 Report 季報表"];

                    // đặt tên cho sheet
                    ws.Name = "Q1,Q3 Report 季報表";
                    // fontsize mặc định cho cả sheet
                    ws.Cells.Style.Font.Size = 11;
                    // font family mặc định cho cả sheet
                    ws.Cells.Style.Font.Name = "微軟正黑體";
                    var headerArray = new List<string>()
                    {
                        "部門單位 Dept.",
                        "姓名 Full Name",
                        "直屬主管評分 L1 score",
                        "直屬主管評語 L1 comment",
                        "二階主管評分L2 score",
                        "二階主管評語 L2 comment",
                        "SMART 分數",
                    };
                    int colIndex = 3;
                    int rowIndex = 5;
                    foreach (var item in (dynamic)(model))
                    {
                        // bắt đầu ghi từ cột 1. Excel bắt đầu từ 1 không phải từ 0 #c0514d
                        colIndex = 3;

                        // rowIndex tương ứng từng dòng dữ liệu
                        rowIndex++;
                        ws.Cells["C3"].Value = "人事季報表";
                        ws.Cells["C3:I3"].Merge = true;
                        ws.Cells["C3:I3"].Style.Font.Size = 18;
                        ws.Cells["C4"].Value = "年度: " + item.Year;
                        ws.Cells["C4"].Style.Font.Size = 14;

                        ws.Cells["D4"].Value = "廠區 SHC";
                        ws.Cells["D4"].Style.Font.Size = 14;

                        ws.Cells["E4"].Value = "季度 Quarter: Q" + item.Quarter;
                        ws.Cells["E4"].Style.Font.Size = 14;



                        ws.Cells["C5"].Value = headerArray[0];
                        ws.Cells["D5"].Value = headerArray[1];
                        ws.Cells["E5"].Value = headerArray[2];
                        ws.Cells["F5"].Value = headerArray[3];
                        ws.Cells["G5"].Value = headerArray[4];
                        ws.Cells["H5"].Value = headerArray[5];
                        ws.Cells["I5"].Value = headerArray[6];

                        ws.Cells["E5"].Style.WrapText = true;
                        ws.Cells["G5"].Style.WrapText = true;
                        ws.Cells["I5"].Style.WrapText = true;


                        ws.Cells[rowIndex, colIndex++].Value = item.OC;
                        ws.Cells[rowIndex, colIndex++].Value = item.FullName;
                        ws.Cells[rowIndex, colIndex++].Value = item.L1Score;
                        ws.Cells[rowIndex, colIndex++].Value = item.L1Comment;
                        ws.Cells[rowIndex, colIndex++].Value = item.L2Score;
                        ws.Cells[rowIndex, colIndex++].Value = item.L2Comment;
                        ws.Cells[rowIndex, colIndex++].Value = item.SmartScore;

                        //Make all text fit the cells
                        //ws.Cells[ws.Dimension.Address].AutoFitColumns();
                        ws.Cells[ws.Dimension.Address].Style.Font.Bold = true;
                        ws.Cells[ws.Dimension.Address].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        ws.Cells[ws.Dimension.Address].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Column(3).Width = 20;
                        ws.Column(4).Width = 30;
                        ws.Column(5).Width = 10;
                        ws.Column(6).Width = 40;
                        ws.Column(7).Width = 10;
                        ws.Column(8).Width = 40;
                        ws.Column(9).Width = 10;
                        //make the borders of cell F6 thick
                        ws.Cells["C3:I6"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        ws.Cells["C3:I6"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        ws.Cells["C3:I6"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        ws.Cells["C3:I6"].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                    }
                    Byte[] bin = p.GetAsByteArray();
                    //Lưu file lại
                    return bin;
                }
            }
            catch (Exception ex)
            {
                var mes = ex.Message;
                Console.WriteLine(mes);
                return new Byte[] { };
            }
        }
    }
}
