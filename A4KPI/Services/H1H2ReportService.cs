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
            var quarterly = await _repoPeriodType.FindAll(x => x.Code == SystemPeriod.HalfYear).FirstOrDefaultAsync();
            var periods = quarterly.Periods.Select(x => new
            {
                x.Title,
                x.Value,
                x.ReportTime,
                Months = x.Months.Split(',').ToList()
            }).Where(x => x.Months.Contains(currentMonth)).FirstOrDefault();

            var l1 = await _repoAccount.FindAll(x => x.Id == accountId).FirstOrDefaultAsync();
            if (l1 == null) return new Q1Q3ReportDto();
            double? l1ScoreKPI = await _repoAttitudeScore.FindAll(x =>
                                         x.PeriodTypeId == SystemPeriodType.HalfYear
                                        && x.CreatedTime.Year == DateTime.Today.Year
                                        && x.Period == periods.Value
                                        && x.ScoreType == ScoreType.L1
                                        && accountId == x.AccountId
                                        && l1.Manager == x.ScoreBy
                                        && x.AccountId != l1.Manager)
                                    .Select(x => x.Point)
                                    .FirstOrDefaultAsync();
            var l1Comment = await _repoComment.FindAll(x =>
                                       x.PeriodTypeId == SystemPeriodType.HalfYear
                                      && x.CreatedTime.Year == DateTime.Today.Year
                                      && x.Period == periods.Value
                                      && x.ScoreType == ScoreType.L1
                                      && accountId == x.AccountId
                                      && l1.Manager == x.CreatedBy
                                      && x.AccountId != l1.Manager)
                                    .Select(x => x.Content)
                                  .FirstOrDefaultAsync();

            double? l2ScoreKPI = await _repoAttitudeScore.FindAll(x =>
                                       x.PeriodTypeId == SystemPeriodType.HalfYear
                                      && x.CreatedTime.Year == DateTime.Today.Year
                                      && x.Period == periods.Value
                                      && x.ScoreType == ScoreType.L2
                                      && accountId == x.AccountId)
                                    .Select(x => x.Point)
                                  .FirstOrDefaultAsync();
            var l2Comment = await _repoComment.FindAll(x =>
                                       x.PeriodTypeId == SystemPeriodType.HalfYear
                                      && x.CreatedTime.Year == DateTime.Today.Year
                                      && x.Period == periods.Value
                                      && x.ScoreType == ScoreType.L2
                                      && accountId == x.AccountId)
                                    .Select(x => x.Content)
                                  .FirstOrDefaultAsync();

            double? ghrScore = await _repoKPIScore.FindAll(x =>
                                      x.PeriodTypeId == SystemPeriodType.HalfYear
                                     && x.CreatedTime.Year == DateTime.Today.Year
                                     && x.Period == periods.Value
                                     && x.ScoreType == ScoreType.GHR
                                     && accountId == x.AccountId)
                                   .Select(x => x.Point)
                                 .FirstOrDefaultAsync();
            double? FLScore = await _repoAttitudeScore.FindAll(x =>
                                      x.PeriodTypeId == SystemPeriodType.HalfYear
                                     && x.CreatedTime.Year == DateTime.Today.Year
                                     && x.Period == periods.Value
                                     && x.ScoreType == ScoreType.FunctionalLeader
                                     && leader_id == x.ScoreBy)
                                   .Select(x => x.Point)
                                 .FirstOrDefaultAsync();
            var data = new H1H2ReportDto(periods.Value, currentYear)
            {
                FullName = ocuser.FullName,
                OC = ocuser.OC,
                L1Score = l1ScoreKPI ?? 0,
                L1Comment = l1Comment ?? "",
                L2Score = l2ScoreKPI ?? 0,
                L2Comment = l2Comment ?? "",
                SmartScore = ghrScore ?? 0,
                FLScore = FLScore ?? 0

            };

            return data;
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
