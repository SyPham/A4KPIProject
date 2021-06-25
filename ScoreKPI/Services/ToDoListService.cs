using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NetUtility;
using ScoreKPI.Constants;
using ScoreKPI.Data;
using ScoreKPI.DTO;
using ScoreKPI.Helpers;
using ScoreKPI.Models;
using ScoreKPI.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
namespace ScoreKPI.Services
{
    public interface IToDoListService : IServiceBase<ToDoList, ToDoListDto>
    {
        Task<object> GetAllInCurrentQuarterByObjectiveIdAsync(int objectiveId);
        /// <summary>
        /// Lấy objective list PICS
        /// Nếu quyền là L1, L2, FHO, GHR, GM thì sẽ để trống
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        Task<object> L0(int accountId, DateTime currentTime);
        /// <summary>
        /// Chấm điểm KPI và điểm thái độ của những người mình đã giao nhiệm vụ
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        Task<object> L1(int accountId, DateTime currentTime);

        /// <summary>
        /// Lấy tất cả cấp dưới của mình để chấm điểm
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        Task<object> L2(int accountId, DateTime currentTime);
        /// <summary>
        /// Lấy tất cả user để chấm điểm( gán ở bảng oc user)
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        Task<object> GHR(int accountId, DateTime currentTime);
        /// <summary>
        /// Lấy tất cả user để chấm điểm( gán ở bảng oc user)
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        Task<object> FHO(int accountId);
        /// <summary>
        /// Lấy tất cả user để chấm điểm( gán ở bảng oc user)
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        Task<object> GM(int accountId, DateTime currentTime);

        Task<List<ToDoListDto>> GetAllByObjectiveIdAsync(int objectiveId);
        Task<List<ToDoListByLevelL1L2Dto>> GetAllInCurrentQuarterByAccountGroup(int accountId);
        Task<List<ToDoListByLevelL1L2Dto>> GetAllKPIScoreByAccountId(int accountId);
        Task<object> GetAllKPIScoreL1L2ByAccountId(int accountId);
        Task<object> GetAllAttitudeScoreL1L2ByAccountId(int accountId);
        Task<object> GetAllKPISelfScoreByObjectiveId(int objectiveId, int accountId);
        Task<object> GetAllObjectiveByL1L2();

        Task<object> GetQuarterlySetting();

    }
    public class ToDoListService : ServiceBase<ToDoList, ToDoListDto>, IToDoListService
    {
        private OperationResult operationResult;
        private readonly IRepositoryBase<ToDoList> _repo;
        private readonly IRepositoryBase<Account> _repoAccount;
        private readonly IRepositoryBase<AccountGroupAccount> _repoAccountGroupAccount;
        private readonly IRepositoryBase<Objective> _repoObjective;
        private readonly IRepositoryBase<ResultOfMonth> _repoResultOfMonth;
        private readonly IRepositoryBase<KPIScore> _repoKPIScore;
        private readonly IRepositoryBase<PeriodType> _repoPeriodType;
        private readonly IRepositoryBase<OCUser> _repoOCUser;
        private readonly IRepositoryBase<OC> _repoOC;
        private readonly IRepositoryBase<Period> _repoPeriod;
        private readonly IRepositoryBase<AttitudeScore> _repoAttitudeScore;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        public ToDoListService(
            IRepositoryBase<ToDoList> repo,
            IRepositoryBase<Account> repoAccount,
            IRepositoryBase<AccountGroupAccount> repoAccountGroupAccount,
            IRepositoryBase<Objective> repoObjective,
            IRepositoryBase<ResultOfMonth> repoResultOfMonth,
            IRepositoryBase<KPIScore> repoKPIScore,
            IRepositoryBase<PeriodType> repoPeriodType,
            IRepositoryBase<OCUser> repoOCUser,
            IRepositoryBase<OC> repoOC,
            IRepositoryBase<Period> repoPeriod,
            IRepositoryBase<AttitudeScore> repoAttitudeScore,
            IHttpContextAccessor httpContextAccessor,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            MapperConfiguration configMapper
            )
            : base(repo, unitOfWork, mapper, configMapper)
        {
            _repo = repo;
            _repoAccount = repoAccount;
            _repoAccountGroupAccount = repoAccountGroupAccount;
            _repoObjective = repoObjective;
            _repoResultOfMonth = repoResultOfMonth;
            _repoKPIScore = repoKPIScore;
            _repoPeriodType = repoPeriodType;
            _repoOCUser = repoOCUser;
            _repoOC = repoOC;
            _repoPeriod = repoPeriod;
            _repoAttitudeScore = repoAttitudeScore;
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configMapper = configMapper;
        }
        public async Task<List<ToDoListDto>> GetAllByObjectiveIdAsync(int objectiveId)
        {
            return await _repo.FindAll(x => x.ObjectiveId == objectiveId).ProjectTo<ToDoListDto>(_configMapper).ToListAsync();

        }

        public async Task<List<ToDoListByLevelL1L2Dto>> GetAllKPIScoreByAccountId(int accountId)
        {
            int currentQuarter = (DateTime.Now.Month + 2) / 3;
            var quarterly = await _repoPeriod.FindAll(x => x.PeriodType.Code == SystemPeriod.Quarterly && x.Value == currentQuarter).FirstOrDefaultAsync();
            if (!quarterly.Months.Contains(','))
            {
                return new List<ToDoListByLevelL1L2Dto> { };
            }
            var monthlist = quarterly.Months.Split(',').Select(int.Parse).OrderBy(x => x).ToList();

            var data = await _repoObjective.FindAll().Select(x => new ToDoListByLevelL1L2Dto
            {
                Id = x.Id,
                Objective = x.Topic,
                L0TargetList = x.ToDoList.Select(x => x.YourObjective).ToList(),
                L0ActionList = x.ToDoList.Select(x => x.Action).ToList(),
            }).ToListAsync();
            return data;

        }

        public async Task<object> GetAllKPISelfScoreByObjectiveId(int objectiveId, int accountId)
        {
            int currentQuarter = (DateTime.Now.Month + 2) / 3;
            var quarterly = await _repoPeriod.FindAll(x => x.PeriodType.Code == SystemPeriod.Quarterly && x.Value == currentQuarter).FirstOrDefaultAsync();
            if (!quarterly.Months.Contains(','))
            {
                return new List<dynamic> { };
            }
            var monthlist = quarterly.Months.Split(',').Select(int.Parse).OrderBy(x => x).ToList();

            var data = await _repo.FindAll(x => x.CreatedBy == accountId).SelectMany(x =>
                x.Objective.ResultOfMonth.Where(x => x.ObjectiveId == objectiveId)
            ).ToListAsync();
            var model = (from a in monthlist
                         join b in data.DistinctBy(x => new { x.Month, x.ObjectiveId }).ToList() on a equals b.Month into ab
                         from c in ab.DefaultIfEmpty()
                         select new
                         {
                             Month = a,
                             ObjectiveList = c != null ? c.Objective.ToDoList.Select(x => x.YourObjective).ToList() : new List<string>(),
                             ResultOfMonth = c != null ? c.Title : "N/A",
                             Period = quarterly.Value,
                             PeriodTypeId = quarterly.PeriodTypeId
                         }).ToList();

            return model;

        }
        public async Task<object> GetAllInCurrentQuarterByObjectiveIdAsync(int objectiveId)
        {

            int quarter = (DateTime.Now.Month + 2) / 3;
            var listMonthOfQuarter = new List<int[]>()
            {
                new int[]{2,3,4},
                new int[]{5,6,7},
                new int[]{8,9,10},
                new int[]{11,12,1}
            };
            var monthlist = listMonthOfQuarter[quarter - 1];
            var result = from a in _repo.FindAll(x => x.ObjectiveId == objectiveId)
                         join b in _repoResultOfMonth.FindAll(x => monthlist.Contains(x.Month)) on a.ObjectiveId equals b.CreatedBy
                         select new
                         {
                             YourObjective = a.CreatedTime.Month == b.Month ? a.YourObjective : "N/A",
                             ResultOfMonth = b.Title ?? "N/A",
                             ResultOfMonthId = b.Id,
                             b.Month
                         };

            return await result.OrderBy(x => x.Month).ToListAsync();
        }
        /// <summary>
        /// Chỉnh sửa thành vừa cập nhật vừa thêm mới
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public override async Task<OperationResult> AddRangeAsync(List<ToDoListDto> model)
        {
            var itemList = _mapper.Map<List<ToDoList>>(model);
            var updateList = itemList.Where(x => x.Id > 0).ToList();
            var addList = itemList.Where(x => x.Id == 0).ToList();
            _repo.UpdateRange(updateList);
            _repo.AddRange(addList);
            try
            {
                await _unitOfWork.SaveChangeAsync();
                operationResult = new OperationResult
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = MessageReponse.AddSuccess,
                    Success = true,
                    Data = itemList
                };
            }
            catch (Exception ex)
            {
                operationResult = ex.GetMessageError();
            }
            return operationResult;
        }

        public async Task<object> GetAllObjectiveByL1L2()
        {
            var kpiResult = from a in _repoObjective.FindAll()
                            select new
                            {
                                Id = a.Id,
                                Objective = a.Topic,
                                Type = "KPI"
                            };
            var attitudeResult = from a in _repoObjective.FindAll()
                                 select new
                                 {
                                     Id = a.Id,
                                     Objective = a.Topic,
                                     Type = "Attitude"
                                 };
            var data1 = await kpiResult.ToListAsync();
            var data2 = await attitudeResult.ToListAsync();
            return data1.Concat(data2).ToList();
        }

        public async Task<object> L0(int accountId, DateTime currentTime)
        {
            var date = DateTime.Today;
            var month = date.Month;
            var quarterlyModel = (await _repoPeriod.FindAll(x => x.PeriodType.Code == SystemPeriod.Quarterly).ToListAsync())
              .Select(x => new
              {
                  x.Title,
                  x.PeriodTypeId,
                  x.PeriodType.DisplayBefore,
                  x.Value,
                  x.ReportTime,
                  Months = x.Months.Split(",").Select(int.Parse).ToList()
              }).ToList();
            var quarterly = quarterlyModel.Where(x => x.Months.Contains(month)).FirstOrDefault();
            var displayBeforQuarterly = quarterly.DisplayBefore;
            var reportTimeQuarterly = quarterly.ReportTime.AddDays(-displayBeforQuarterly);

            var monthlyModel = (await _repoPeriod.FindAll(x => x.PeriodType.Code == SystemPeriod.Monthly).ToListAsync())
             .Select(x => new
             {
                 x.Title,
                 x.PeriodTypeId,
                 x.PeriodType.DisplayBefore,
                 x.Value,
                 x.ReportTime
             }).ToList();
            var monthly = monthlyModel.Where(x => x.Value.Equals(currentTime.Month)).FirstOrDefault();

            var displayBeformonthly = monthly.DisplayBefore;
            var reportTimemonthly = monthly.ReportTime.AddDays(-displayBeformonthly);

            return await _repoObjective.FindAll().Where(x => x.PICs.Any(x => x.AccountId == accountId))
          .Select(x => new
          {
              Topic = x.Topic,
              Id = x.Id,
              Period = quarterly.Value,
              PeriodTypeId = quarterly.PeriodTypeId,
              IsDisplayUploadResult = currentTime.Date >= reportTimemonthly.Date,
              IsDisplaySelfScore = currentTime.Date >= reportTimeQuarterly.Date,
          }).ToListAsync();

        }

        public async Task<object> L1(int accountId, DateTime currentTime)
        {

            var accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            int accountID = JWTExtensions.GetDecodeTokenById(accessToken);
            var date = DateTime.Today;
            var month = date.Month;
            int currentHalfYear = month <= 6 && month >= 1 ? 1 : 2;

            // Lấy settings của quý và nửa năm
            var quarterlyModel = (await _repoPeriod.FindAll(x => x.PeriodType.Code == SystemPeriod.Quarterly).ToListAsync())
                .Select(x => new
                {
                    x.Title,
                    x.PeriodTypeId,
                    x.Value,
                    x.ReportTime,
                    x.PeriodType.DisplayBefore,
                    Months = x.Months.Split(",").Select(int.Parse).ToList()
                }).ToList();

            var halfYearModel = (await _repoPeriod.FindAll(x => x.PeriodType.Code == SystemPeriod.HalfYear && x.Value == currentHalfYear).ToListAsync())
               .Select(x => new
               {
                   x.Title,
                   x.PeriodTypeId,
                   x.PeriodType.DisplayBefore,
                   x.Value,
                   x.ReportTime,
                   Months = x.Months.Split(",").Select(int.Parse).ToList()
               }).ToList();
            var quarterly = quarterlyModel.Where(x => x.Months.Contains(month)).FirstOrDefault();
            var halfYear = halfYearModel.Where(x => x.Months.Contains(month)).FirstOrDefault();
            var halfYearSettings = halfYear.Months;
            var quarterlySettings = quarterly.Months;

            var displayBeforQuarterly = quarterly.DisplayBefore;
            var reportTimeQuarterly = quarterly.ReportTime.AddDays(-displayBeforQuarterly);

            var displayBeforHalfYear = quarterly.DisplayBefore;
            var reportTimeHalfYear = halfYear.ReportTime.AddDays(-displayBeforHalfYear);


            // Lấy tất cả các user đã giao nhiệm vụ cho họ
            var data = (await _repoObjective.FindAll(x => accountID == x.CreatedBy)
                .SelectMany(x => x.PICs)
                .ToListAsync())
                .DistinctBy(x => x.AccountId)
                .ToList();
            var kpi = data.Select(x => new
            {
                Id = x.AccountId,
                Objective = $"{quarterly.Title} - {x.Account.FullName}",
                DueDate = quarterly.ReportTime,
                Type = "KPI",
                Period = quarterly.Value,
                PeriodTypeId = quarterly.PeriodTypeId,
                Settings = quarterlySettings,
                IsDisplayKPIScore = true,
                IsDisplayAttitude = false,
            }).ToList();
            var attitude = data.Select(x => new
            {
                Id = x.AccountId,
                Objective = $"{halfYear.Title} - {x.Account.FullName}",
                DueDate = halfYear.ReportTime,
                Type = "Attitude",
                Period = halfYear.Value,
                PeriodTypeId = halfYear.PeriodTypeId,
                Settings = halfYearSettings,
                IsDisplayKPIScore = false,
                IsDisplayAttitude = true
            }).ToList();
            if (currentTime.Date >= reportTimeQuarterly.Date)
            {
                return kpi.ToList();
            }

            if (currentTime.Date >= reportTimeHalfYear.Date)
            {
                return attitude.ToList();

            }

            if (currentTime.Date >= reportTimeQuarterly.Date && currentTime.Date == reportTimeHalfYear.Date)
            {
                return kpi.Concat(attitude).ToList();
            }
            return new List<dynamic>();
        }

        public async Task<object> L2(int accountId, DateTime currentTime)
        {
            var accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            int accountID = JWTExtensions.GetDecodeTokenById(accessToken);
            // tim oc cua usser login
            var ocuser = await _repoOCUser.FindAll(x => x.UserID == accountID).FirstOrDefaultAsync();
            var checkRole = await _repoAccountGroupAccount.FindAll(x => x.AccountId == accountID).Select(x => x.AccountGroup.Position).AnyAsync(x => SystemRole.L2 == x);
            if (checkRole == false) return new List<dynamic>();

            if (ocuser == null) return new List<dynamic>();
            // Lay tat ca con cua oc
            var oc = _repoOC.FindAll().AsHierarchy(x => x.Id, y => y.ParentId, ocuser.OCID).ToList();
            var ocs = oc.Flatten(x => x.ChildNodes).Select(x => x.Entity.Id).ToList();
            // vao ocUser tim theo ocId list 
            var accountIds = await _repoOCUser.FindAll(x => ocs.Contains(x.OCID)).Select(x => x.UserID).Distinct().ToListAsync();

            var date = DateTime.Today;
            var month = date.Month;
            int currentHalfYear = month <= 6 && month >= 1 ? 1 : 2;

            // Lấy settings của quý và nửa năm
            var quarterlyModel = (await _repoPeriod.FindAll(x => x.PeriodType.Code == SystemPeriod.Quarterly).ToListAsync())
                .Select(x => new
                {
                    x.Title,
                    x.PeriodTypeId,
                    x.PeriodType.DisplayBefore,
                    x.Value,
                    x.ReportTime,
                    Months = x.Months.Split(",").Select(int.Parse).ToList()
                }).ToList();

            var halfYearModel = (await _repoPeriod.FindAll(x => x.PeriodType.Code == SystemPeriod.HalfYear && x.Value == currentHalfYear).ToListAsync())
               .Select(x => new
               {
                   x.Title,
                   x.PeriodTypeId,
                   x.PeriodType.DisplayBefore,
                   x.Value,
                   x.ReportTime,
                   Months = x.Months.Split(",").Select(int.Parse).ToList()
               }).ToList();
            var quarterly = quarterlyModel.Where(x => x.Months.Contains(month)).FirstOrDefault();
            var halfYear = halfYearModel.Where(x => x.Months.Contains(month)).FirstOrDefault();
            var halfYearSettings = halfYear.Months;
            var quarterlySettings = quarterly.Months;


            var displayBeforQuarterly = quarterly.DisplayBefore;
            var reportTimeQuarterly = quarterly.ReportTime.AddDays(-displayBeforQuarterly);

            var displayBeforHalfYear = quarterly.DisplayBefore;
            var reportTimeHalfYear = halfYear.ReportTime.AddDays(-displayBeforHalfYear);

            // Lấy tất cả các user đã giao nhiệm vụ cho họ
            var data = (await _repoObjective.FindAll(x => accountIds.Contains(x.CreatedBy))
                .SelectMany(x => x.PICs)
                .ToListAsync())
                .DistinctBy(x => x.AccountId)
                .ToList();
            var kpi = data.Select(x => new
            {
                Id = x.AccountId,
                Objective = $"{quarterly.Title} - {x.Account.FullName}",
                DueDate = quarterly.ReportTime,
                Type = "KPI",
                Period = quarterly.Value,
                PeriodTypeId = quarterly.PeriodTypeId,
                Settings = quarterlySettings
            }).ToList();
            var attitude = data.Select(x => new
            {
                Id = x.AccountId,
                Objective = $"{halfYear.Title} - {x.Account.FullName}",
                DueDate = halfYear.ReportTime,
                Type = "Attitude",
                Period = halfYear.Value,
                PeriodTypeId = halfYear.PeriodTypeId,
                Settings = halfYearSettings
            }).ToList();
            if (currentTime.Date >= reportTimeQuarterly.Date)
            {
                return kpi.ToList();
            }

            if (currentTime.Date >= reportTimeHalfYear.Date)
            {
                return attitude.ToList();

            }

            if (currentTime.Date >= reportTimeQuarterly.Date && currentTime.Date >= reportTimeHalfYear.Date)
            {
                return kpi.Concat(attitude).ToList();
            }
            return new List<dynamic>();
        }

        public async Task<object> GHR(int accountId, DateTime currentTime)
        {
            var accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            int accountID = JWTExtensions.GetDecodeTokenById(accessToken);
            var checkRole = await _repoAccountGroupAccount.FindAll(x => x.AccountId == accountID)
                .Select(x => x.AccountGroup.Position).AnyAsync(x => SystemRole.GHR == x);
            if (checkRole == false) return new List<dynamic>();

            // tim oc cua usser login
            var ocuser = await _repoOCUser.FindAll(x => x.UserID == accountID).FirstOrDefaultAsync();
            if (ocuser == null) return new List<dynamic>();
            // Lay tat ca con cua oc
            var oc = _repoOC.FindAll().AsHierarchy(x => x.Id, y => y.ParentId, ocuser.OCID).ToList();
            var ocs = oc.Flatten(x => x.ChildNodes).Select(x => x.Entity.Id).ToList();
            // vao ocUser tim theo ocId list 
            var accountIds = await _repoOCUser.FindAll(x => ocs.Contains(x.OCID)).Select(x => x.UserID).Distinct().ToListAsync();

            var date = DateTime.Today;
            var month = date.Month;
            int currentHalfYear = month <= 6 && month >= 1 ? 1 : 2;

            // Lấy settings của quý và nửa năm
            var quarterlyModel = (await _repoPeriod.FindAll(x => x.PeriodType.Code == SystemPeriod.Quarterly).ToListAsync())
                .Select(x => new
                {
                    x.Title,
                    x.PeriodTypeId,
                    x.PeriodType.DisplayBefore,
                    x.Value,
                    x.ReportTime,
                    Months = x.Months.Split(",").Select(int.Parse).ToList()
                }).ToList();

            var halfYearModel = (await _repoPeriod.FindAll(x => x.PeriodType.Code == SystemPeriod.HalfYear && x.Value == currentHalfYear).ToListAsync())
               .Select(x => new
               {
                   x.Title,
                   x.PeriodTypeId,
                   x.PeriodType.DisplayBefore,
                   x.Value,
                   x.ReportTime,
                   Months = x.Months.Split(",").Select(int.Parse).ToList()
               }).ToList();
            var quarterly = quarterlyModel.Where(x => x.Months.Contains(month)).FirstOrDefault();
            var halfYear = halfYearModel.Where(x => x.Months.Contains(month)).FirstOrDefault();
            var halfYearSettings = halfYear.Months;
            var quarterlySettings = quarterly.Months;


            var displayBeforQuarterly = quarterly.DisplayBefore;
            var reportTimeQuarterly = quarterly.ReportTime.AddDays(-displayBeforQuarterly);

            var displayBeforHalfYear = quarterly.DisplayBefore;
            var reportTimeHalfYear = halfYear.ReportTime.AddDays(-displayBeforHalfYear);


            // Lấy tất cả các user đã giao nhiệm vụ cho họ
            var data = (await _repoObjective.FindAll(x => accountIds.Contains(x.CreatedBy))
                .SelectMany(x => x.PICs)
                .ToListAsync())
                .DistinctBy(x => x.AccountId)
                .ToList();
            var kpi = data.Select(x => new
            {
                Id = x.AccountId,
                Objective = $"{quarterly.Title} - {x.Account.FullName}",
                DueDate = quarterly.ReportTime,
                Type = "KPI",
                Period = quarterly.Value,
                PeriodTypeId = quarterly.PeriodTypeId,
                Settings = quarterlySettings
            }).ToList();
            var attitude = data.Select(x => new
            {
                Id = x.AccountId,
                Objective = $"{halfYear.Title} - {x.Account.FullName}",
                DueDate = halfYear.ReportTime,
                Type = "Attitude",
                Period = halfYear.Value,
                PeriodTypeId = halfYear.PeriodTypeId,
                Settings = halfYearSettings
            }).ToList();
            if (currentTime.Date >= reportTimeQuarterly.Date)
            {
                return kpi.ToList();
            }

            if (currentTime.Date >= reportTimeHalfYear.Date)
            {
                return attitude.ToList();

            }

            if (currentTime.Date >= reportTimeQuarterly.Date && currentTime.Date >= reportTimeHalfYear.Date)
            {
                return kpi.Concat(attitude).ToList();
            }
            return new List<dynamic>();
        }
        public async Task<object> FHO(int accountId)
        {
            var date = DateTime.Today;
            var month = date.Month;
            var quarterlyModel = (await _repoPeriod.FindAll(x => x.PeriodType.Code == SystemPeriod.Quarterly).ToListAsync())
              .Select(x => new
              {
                  x.Title,
                  x.PeriodTypeId,
                  x.Value,
                  x.ReportTime,
                  Months = x.Months.Split(",").Select(int.Parse).ToList()
              }).ToList();
            var quarterly = quarterlyModel.Where(x => x.Months.Contains(month)).FirstOrDefault();

            return await _repoObjective.FindAll().Where(x => x.PICs.Any(x => x.AccountId == accountId))
                .Select(x => new
                {
                    Topic = x.Topic,
                    Id = x.Id,
                    Period = quarterly.Value,
                    PeriodTypeId = quarterly.PeriodTypeId
                }).ToListAsync();
        }

        public async Task<object> GM(int accountId, DateTime currentTime)
        {
            var accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            int accountID = JWTExtensions.GetDecodeTokenById(accessToken);
            var checkRole = await _repoAccountGroupAccount.FindAll(x => x.AccountId == accountID)
                .Select(x => x.AccountGroup.Position).AnyAsync(x => SystemRole.GM == x);
            if (checkRole == false) return new List<dynamic>();

            // tim oc cua usser login
            var ocuser = await _repoOCUser.FindAll(x => x.UserID == accountID).FirstOrDefaultAsync();
            if (ocuser == null) return new List<dynamic>();
            // Lay tat ca con cua oc
            var oc = _repoOC.FindAll().AsHierarchy(x => x.Id, y => y.ParentId, ocuser.OCID).ToList();
            var ocs = oc.Flatten(x => x.ChildNodes).Select(x => x.Entity.Id).ToList();
            // vao ocUser tim theo ocId list 
            var accountIds = await _repoOCUser.FindAll(x => ocs.Contains(x.OCID)).Select(x => x.UserID).Distinct().ToListAsync();

            var date = DateTime.Today;
            var month = date.Month;
            int currentHalfYear = month <= 6 && month >= 1 ? 1 : 2;

            // Lấy settings của quý và nửa năm
            var quarterlyModel = (await _repoPeriod.FindAll(x => x.PeriodType.Code == SystemPeriod.Quarterly).ToListAsync())
                .Select(x => new
                {
                    x.Title,
                    x.PeriodTypeId,
                    x.PeriodType.DisplayBefore,
                    x.Value,
                    x.ReportTime,
                    Months = x.Months.Split(",").Select(int.Parse).ToList()
                }).ToList();

            var halfYearModel = (await _repoPeriod.FindAll(x => x.PeriodType.Code == SystemPeriod.HalfYear && x.Value == currentHalfYear).ToListAsync())
               .Select(x => new
               {
                   x.Title,
                   x.PeriodTypeId,
                   x.PeriodType.DisplayBefore,
                   x.Value,
                   x.ReportTime,
                   Months = x.Months.Split(",").Select(int.Parse).ToList()
               }).ToList();
            var quarterly = quarterlyModel.Where(x => x.Months.Contains(month)).FirstOrDefault();
            var halfYear = halfYearModel.Where(x => x.Months.Contains(month)).FirstOrDefault();
            var halfYearSettings = halfYear.Months;
            var quarterlySettings = quarterly.Months;


            var displayBeforQuarterly = quarterly.DisplayBefore;
            var reportTimeQuarterly = quarterly.ReportTime.AddDays(-displayBeforQuarterly);

            var displayBeforHalfYear = quarterly.DisplayBefore;
            var reportTimeHalfYear = halfYear.ReportTime.AddDays(-displayBeforHalfYear);
            // Lấy tất cả các user đã giao nhiệm vụ cho họ
            var data = (await _repoObjective.FindAll(x => accountIds.Contains(x.CreatedBy))
                .SelectMany(x => x.PICs)
                .ToListAsync())
                .DistinctBy(x => x.AccountId)
                .ToList();
            var kpi = data.Select(x => new
            {
                Id = x.AccountId,
                Objective = $"{quarterly.Title} - {x.Account.FullName}",
                DueDate = quarterly.ReportTime,
                Type = "KPI",
                Period = quarterly.Value,
                PeriodTypeId = quarterly.PeriodTypeId,
                Settings = quarterlySettings
            }).ToList();

            if (currentTime.Date >= reportTimeQuarterly.Date)
            {
                return kpi.ToList();
            }

            return new List<dynamic>();
        }
        public Task<List<ToDoListByLevelL1L2Dto>> GetAllInCurrentQuarterByAccountGroup(int accountId)
        {
            throw new NotImplementedException();
        }

        public async Task<object> GetAllAttitudeScoreL1L2ByAccountId(int accountId)
        {
            var date = DateTime.Today;
            var month = date.Month;
            int currentHalfYear = month <= 6 && month >= 1 ? 1 : 2;

            // Lấy settings của quý và nửa năm
            var halfYearModel = (await _repoPeriod.FindAll(x => x.PeriodType.Code == SystemPeriod.HalfYear && x.Value == currentHalfYear).ToListAsync())
             .Select(x => new
             {
                 x.Title,
                 x.PeriodTypeId,
                 x.Value,
                 x.ReportTime,
                 Months = x.Months.Split(",").Select(int.Parse).ToList()
             }).ToList();
            var halfYear = halfYearModel.Where(x => x.Months.Contains(month)).FirstOrDefault();
            var monthsOfCurrentHalfYear = halfYear.Months;

            var data = await _repoObjective.FindAll(x => x.PICs.Select(a => a.AccountId).Contains(accountId))
                .Where(x => x.ResultOfMonth.Any(a => monthsOfCurrentHalfYear.Contains(a.Month)))
                .Select(x => new
                {
                    Objective = x.Topic,
                    L0TargetList = x.ToDoList.Select(a => a.YourObjective).ToList(),
                    L0ActionList = x.ToDoList.Select(a => a.Action).ToList(),
                    ResultOfMonth = x.ResultOfMonth.Select(a => new { a.Month, a.Title, a.ObjectiveId, a.CreatedBy }),
                    Settings = monthsOfCurrentHalfYear,
                    PeriodTypeId = halfYear.PeriodTypeId,
                    Period = halfYear.Value
                })
                .ToListAsync();


            return data;
        }
        public async Task<object> GetAllKPIScoreL1L2ByAccountId(int accountId)
        {
            var date = DateTime.Today;
            var month = date.Month;
            // Lấy settings của quý và nửa năm
            var quarterlyModel = (await _repoPeriod.FindAll(x => x.PeriodType.Code == SystemPeriod.Quarterly).ToListAsync())
              .Select(x => new
              {
                  x.Title,
                  x.PeriodTypeId,
                  x.Value,
                  x.ReportTime,
                  Months = x.Months.Split(",").Select(int.Parse).ToList()
              }).ToList();
            var quarterly = quarterlyModel.Where(x => x.Months.Contains(month)).FirstOrDefault();
            var monthsOfCurrentQuarter = quarterly.Months.ToList();

            var data = await _repoObjective.FindAll(x => x.PICs.Select(a => a.AccountId).Contains(accountId))
                .Where(x => x.ResultOfMonth.Any(a => monthsOfCurrentQuarter.Contains(a.Month)))
                .Select(x => new
                {
                    Objective = x.Topic,
                    L0TargetList = x.ToDoList.Select(a => a.YourObjective).ToList(),
                    L0ActionList = x.ToDoList.Select(a => a.Action).ToList(),
                    ResultOfMonth = x.ResultOfMonth.Select(a => new { a.Month, a.Title, a.ObjectiveId, a.CreatedBy }),
                    Settings = monthsOfCurrentQuarter,
                    PeriodTypeId = quarterly.PeriodTypeId,
                    Period = quarterly.Value
                })
                .ToListAsync();


            return data;
        }

        public async Task<object> GetQuarterlySetting()
        {
            var date = DateTime.Today;
            var month = date.Month;
            // Lấy settings của quý và nửa năm
            var quarterlyModel = (await _repoPeriod.FindAll(x => x.PeriodType.Code == SystemPeriod.Quarterly).ToListAsync())
              .Select(x => new
              {
                  x.Title,
                  x.ReportTime,
                  Months = x.Months.Split(",").Select(int.Parse).ToList()
              }).ToList();
            var quarterly = quarterlyModel.Where(x => x.Months.Contains(month)).FirstOrDefault();
            var monthsOfCurrentQuarter = quarterly.Months.ToList();
            return monthsOfCurrentQuarter;
        }

      
    }
}
