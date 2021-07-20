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

namespace A4KPI.Services
{
    public interface IH1H2Service
    {
      
        Task<object> GetH1H2Data();
        Task<object> GetReportInfo(int accountId);

        Task<object> GetAllKPIScoreL0ByPeriod(int period);
    }
    public class H1H2Service : IH1H2Service
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
        private readonly IRepositoryBase<OC> _repoOC;
        private readonly IRepositoryBase<Period> _repoPeriod;
        private readonly IRepositoryBase<AttitudeScore> _repoAttitudeScore;
        private readonly IRepositoryBase<SpecialContributionScore> _repoSpecial;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IRepositoryBase<Comment> _repoComment;
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
            IRepositoryBase<Comment> repoComment,
            IRepositoryBase<AttitudeScore> repoAttitudeScore,
            IRepositoryBase<SpecialContributionScore> repoSpecial,
            IHttpContextAccessor httpContextAccessor,
            IUnitOfWork unitOfWork,
            IMapper mapper,
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
            _configMapper = configMapper;
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
                    double A_total = await ConvertAtotal(l1ScoreKPI ?? 0, l2ScoreKPI ?? 0, FLScore ?? 0);
                    double L1 = (l1Q1ScoreKPI + l1Q2ScoreKPI) / 2 ?? 0;
                    double L2 = (l2Q1ScoreKPI + l2Q2ScoreKPI) / 2 ?? 0;
                    double Smart = (GHRQ1ScoreKPI + GHRQ2ScoreKPI) / 2 ?? 0;
                    double B_selfScore = await BSelfScore(l0ScoreKPI ?? 0);
                    double B_L1 = await BL1((l1Q1ScoreKPI + l1Q2ScoreKPI) / 2 ?? 0);
                    double B_L2 = await BL2((l2Q1ScoreKPI + l2Q2ScoreKPI) / 2 ?? 0);
                    double B_Smart = await BSmart((GHRQ1ScoreKPI + GHRQ2ScoreKPI) / 2 ?? 0);
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
                        D_total = A_total + (B_selfScore + B_L1 + B_L2 + B_Smart) + (Score_special ?? 0)
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
                    dataH2 = new H1H2ReportDto(item, periods_quarterly.Value, currentYear)
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
                        FLScore = FLScore ?? 0

                    };
                }
            }

            return new { 
                H1 = dataH1,
                H2 = dataH2
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
            var pics = await _repoPIC.FindAll(x => accountIds.Contains(x.AccountId)).Select(x=>x.AccountId).Distinct().ToListAsync();

            var model = await _repoOCAccount.FindAll(x => pics.Contains(x.AccountId))
                .Select(x => new
                {
                    Id = x.AccountId,
                    FullName = x.Account.FullName,
                }).ToListAsync();

            return model;
        }

        public Task<object> GetAllKPIScoreL0ByPeriod(int period)
        {
            throw new NotImplementedException();
        }
    }
}
