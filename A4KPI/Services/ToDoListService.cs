using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NetUtility;
using A4KPI.Constants;
using A4KPI.Data;
using A4KPI.DTO;
using A4KPI.Helpers;
using A4KPI.Models;
using A4KPI.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
namespace A4KPI.Services
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
        Task<object> FunctionalLeader(int accountId, DateTime currentTime);

        /// <summary>
        /// Lấy tất cả cấp dưới của mình để chấm điểm
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        Task<object> L2(int accountId, DateTime currentTime);
        /// <summary>
        /// Chấm điểm tất cả
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

        Task<object> GetAllByObjectiveIdAsync(int objectiveId);
        Task<List<ToDoListByLevelL1L2Dto>> GetAllInCurrentQuarterByAccountGroup(int accountId);
        Task<List<ToDoListByLevelL1L2Dto>> GetAllKPIScoreByAccountId(int accountId);
        Task<object> GetAllKPIScoreL1ByAccountId(int accountId, DateTime currentTime);
        Task<object> GetAllKPIScoreL2ByAccountId(int accountId, DateTime currentTime);
        Task<object> GetAllKPIScoreGHRByAccountId(int accountId, DateTime currentTime);
        Task<object> GetAllAttitudeScoreL1ByAccountId(int accountId);
        Task<object> GetAllAttitudeScoreL2ByAccountId(int accountId);
        Task<object> GetAllAttitudeScoreGHRByAccountId(int accountId);
        Task<object> GetAllAttitudeScoreGFLByAccountId(int accountId);
        Task<object> GetAllKPISelfScoreByObjectiveId(int objectiveId, int accountId);
        Task<object> GetAllObjectiveByL1L2();
        Task<object> GetQuarterlySetting();
        Task<object> GetAllByObjectiveIdAsTreeAsync(int objectiveId);

        Task<object> GetAllKPIScoreL0ByPeriod(int period);
        Task<object> GetAllAttitudeScoreByFunctionalLeader();
        Task<OperationResult> Reject(List<int> ids);
        Task<OperationResult> DisableReject(List<int> ids);
        Task<OperationResult> Release(List<int> ids);

        Task<OperationResult> ReportQ1orQ3(Q1OrQ3Request request);

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
        private readonly IRepositoryBase<OCAccount> _repoOCAccount;
        private readonly IRepositoryBase<PIC> _repoPIC;
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
            IRepositoryBase<OCAccount> repoOCAccount,
            IRepositoryBase<PIC> repoPIC,
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
        public async Task<object> GetAllByObjectiveIdAsync(int objectiveId)
        {
            var accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            int accountId = JWTExtensions.GetDecodeTokenById(accessToken);
            return await _repo.FindAll(x => x.ObjectiveId == objectiveId && accountId == x.CreatedBy ).ProjectTo<ToDoListDto>(_configMapper).ToListAsync();
        }
        public async Task<object> GetAllByObjectiveIdAsTreeAsync(int objectiveId)
        {
            var accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            int accountId = JWTExtensions.GetDecodeTokenById(accessToken);
            return (await _repo.FindAll(x => x.ObjectiveId == objectiveId && accountId == x.CreatedBy).ProjectTo<ToDoListDto>(_configMapper).ToListAsync()).AsHierarchy(x => x.Id, y => y.ParentId);
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
                L0TargetList = x.ToDoList.FirstOrDefault(x => x.Level == ToDoListLevel.Target).Action,
                L0ActionList = x.ToDoList.Where(x => x.Level == ToDoListLevel.Action).Select(x => x.Action).ToList(),
            }).ToListAsync();
            return data;

        }

        public async Task<object> GetAllKPISelfScoreByObjectiveId(int objectiveId, int accountId)
        {
            //var currentDate = DateTime.Today.Date;
            var currentDate = new DateTime(2021, 8, 1).Date;
            var currentMonth = currentDate.Month + "";
            // Lay cai dat quy
            var quarterly = await _repoPeriodType.FindAll(x => x.Code == SystemPeriod.Quarterly).FirstOrDefaultAsync();
            var periods = quarterly.Periods.Select(x => new
            {
                x.Title,
                x.Value,
                x.ReportTime,
                Months = x.Months.Split(',').ToList()
            }).ToList();
            var beforeDay = quarterly.DisplayBefore;
            var currentDate2 = currentDate.AddDays(beforeDay);
            var currentQuarter = periods.Where(x => x.ReportTime.Date <= currentDate2).FirstOrDefault();
            if (currentQuarter == null) return new List<dynamic>();
            // lay thoi gian chuan bi

            var monthlist = currentQuarter.Months.Select(int.Parse).OrderBy(x => x).ToList();

            var data = await _repo.FindAll(x => x.CreatedBy == accountId).SelectMany(x =>
                x.Objective.ResultOfMonth.Where(x => x.ObjectiveId == objectiveId)
            ).ToListAsync();
            var model = (from a in monthlist
                         join b in data.DistinctBy(x => new { x.Month, x.ObjectiveId }).ToList() on a equals b.Month into ab
                         from c in ab.DefaultIfEmpty()
                         select new
                         {
                             Month = a,
                             ObjectiveList = c != null && c.Objective.ToDoList.Any(x => x.Level == ToDoListLevel.Objective) ? c.Objective.ToDoList.FirstOrDefault(x => x.Level == ToDoListLevel.Objective).Action : "N/A",
                             ResultOfMonth = c != null ? c.Title : "N/A",
                             Period = currentQuarter.Value,
                             PeriodTypeId = quarterly.Id
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
                             YourObjective = a.CreatedTime.Month == b.Month ? a.Action : "N/A",
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
            // add objective
            var objectiveId = 0;
            var objective = model.FirstOrDefault(x => x.Level == ToDoListLevel.Objective);
            var objectiveItem = _mapper.Map<ToDoList>(objective);
            objectiveItem.ParentId = null;
            if (objective == null)
                return new OperationResult
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = MessageReponse.AddError,
                    Success = true,
                    Data = null
                };
            if (objectiveItem.Id > 0) _repo.Update(objectiveItem); else _repo.Add(objectiveItem);
            await _unitOfWork.SaveChangeAsync();
            objectiveId = objectiveItem.Id;

            // add target
            var targetId = 0;
            var target = model.FirstOrDefault(x => x.Level == ToDoListLevel.Target);
            var targetItem = _mapper.Map<ToDoList>(target);
            targetItem.ParentId = objectiveId;
            if (target == null)
                return new OperationResult
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = MessageReponse.AddError,
                    Success = true,
                    Data = null
                };
            if (targetItem.Id > 0) _repo.Update(targetItem); else _repo.Add(targetItem);
            await _unitOfWork.SaveChangeAsync();
            targetId = targetItem.Id;

            // add or update action
            var itemList = _mapper.Map<List<ToDoList>>(model.Where(x => x.Level == ToDoListLevel.Action).ToList());
            foreach (var item in itemList)
            {
                item.ParentId = targetId;
            }
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
            var accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            int accountID = JWTExtensions.GetDecodeTokenById(accessToken);
            var account = await _repoAccount.FindAll(x => x.Id == accountID).FirstOrDefaultAsync();
            if (account == null) return new List<L0Dto>();

            var checkRole = await _repoAccountGroupAccount.FindAll(x => x.AccountId == accountID).Select(x => x.AccountGroup.Position).AnyAsync(x => SystemRole.L0 == x);
            if (checkRole == false) return new List<L0Dto>();
            var date = currentTime;
            var month = date.Month;
            var selfScore = new List<L0Dto>();
            var resultOfMonth = new List<L0Dto>();
            var action = new List<L0Dto>();

            var quarterType = await _repoPeriodType.FindAll(x => x.Code == SystemPeriod.Quarterly).FirstOrDefaultAsync();
            var displayTimeQuarter = date.AddDays(quarterType.DisplayBefore).Date;
            var quarterlyModel = await _repoPeriod.FindAll(x => x.PeriodType.Code == SystemPeriod.Quarterly && x.ReportTime.Date >= displayTimeQuarter).OrderBy(x => x.ReportTime).FirstOrDefaultAsync();
            var quarterSettings = new List<int> { Quarter.Q2, Quarter.Q4 }; // Quý 2 và Quý 4 L0 sẽ tự chấm điểm cho bản thân -> user demand
            var settingsForQuater2 = new List<int> { Quarter.Q1, Quarter.Q2 }; // Nếu rơi vào quý 2 thì sẽ lấy những tháng của quý 1 và quý 2
            var settingsForQuater4 = new List<int> { Quarter.Q3, Quarter.Q4 }; // Nếu rơi vào quý 4 thì sẽ lấy những tháng của quý 3 và quý 4
          
            if (quarterlyModel != null && quarterSettings.Contains(quarterlyModel.Value))
            {

                var quarterlySettings = new List<int> { };
                if (quarterlyModel.Value == Quarter.Q2)
                {
                    quarterlySettings = (await _repoPeriod.FindAll(x => x.PeriodType.Code == SystemPeriod.Quarterly && settingsForQuater2.Contains(x.Value))
                        .Select(x => new
                        {
                            x.Months
                        })
                        .ToListAsync()).Select(x => new
                        {
                            Months = x.Months.Split(",").Select(int.Parse).ToList()
                        }).SelectMany(x => x.Months).OrderBy(x => x).ToList();

                }
                if (quarterlyModel.Value == Quarter.Q4)
                {
                    quarterlySettings = (await _repoPeriod.FindAll(x => x.PeriodType.Code == SystemPeriod.Quarterly && settingsForQuater4.Contains(x.Value))
                        .Select(x => new
                        {
                            x.Months
                        })
                        .ToListAsync()).Select(x => new
                        {
                            Months = x.Months.Split(",").Select(int.Parse).ToList()
                        }).SelectMany(x => x.Months).OrderBy(x => x).ToList();

                }

                // Tự chấm điểm theo quý 2
                selfScore.Add(new L0Dto
                {
                    Topic = "Achievement Rate - " + quarterlyModel.Title,
                    Period = quarterlyModel.Value,
                    PeriodTypeId = SystemPeriodType.Quarterly,
                    HalfYearId = SystemPeriodType.HalfYear,
                    Settings = quarterlySettings,
                    IsDisplaySelfScore = true,
                    HasFunctionalLeader = account.Leader.HasValue
                });
            }
            var monthlyType = await _repoPeriodType.FindAll(x => x.Code == SystemPeriod.Monthly).FirstOrDefaultAsync();
            var monthlyModel = await _repoPeriod.FindAll(x => x.PeriodType.Code == SystemPeriod.Monthly && x.ReportTime.Date >= displayTimeQuarter).OrderBy(x => x.ReportTime).FirstOrDefaultAsync();
            if (monthlyModel != null)
            {
                var monthValue = monthlyModel.Value;
                var quarter = (await _repoPeriod.FindAll(x => x.PeriodType.Code == SystemPeriod.Quarterly).OrderBy(x => x.ReportTime).Select(x => new
                {
                    Value = x.Value,
                    PeriodTypeId = x.PeriodTypeId,
                    Months = x.Months
                }).ToListAsync()).Select(x => new
                {
                    x.Value,
                    QuarterPeriodTypeId = x.PeriodTypeId,
                    Months = x.Months.Split(",").Select(int.Parse).ToList()
                }).FirstOrDefault(x => x.Months.Contains(monthValue));
                var monthText = new DateTime(2020, monthValue, 1).ToString("MMMM");


                resultOfMonth = await _repoObjective.FindAll().Where(x => x.ToDoList.Any(x => x.CreatedBy == accountId))
                 .Select(x => new L0Dto
                 {
                     TodolistId = x.ToDoList.Any(x => x.Level == ToDoListLevel.Target && x.CreatedBy == accountId) ? x.ToDoList.FirstOrDefault(x => x.Level == ToDoListLevel.Target && x.CreatedBy == accountId).Id : 0,
                     IsReject = x.ToDoList.Any(x => x.Level == ToDoListLevel.Target && x.CreatedBy == accountId) ? x.ToDoList.FirstOrDefault(x => x.Level == ToDoListLevel.Target && x.CreatedBy == accountId).IsReject : false,
                     IsRelease = x.ToDoList.Any(x => x.Level == ToDoListLevel.Target && x.CreatedBy == accountId) ? x.ToDoList.FirstOrDefault(x => x.Level == ToDoListLevel.Target && x.CreatedBy == accountId).IsRelease : false,
                     Topic = x.ToDoList.Any(x => x.Level == ToDoListLevel.Objective && x.CreatedBy == accountId) ? monthText + " - " + x.ToDoList.FirstOrDefault(x => x.Level == ToDoListLevel.Objective && x.CreatedBy == accountId).Action : "N/A",
                     Id = x.Id,
                     Quarter = quarter.Value,
                     QuarterPeriodTypeId = quarter.QuarterPeriodTypeId,
                     Period = monthlyModel.Value,
                     PeriodTypeId = monthlyModel.PeriodTypeId,
                     IsDisplayUploadResult = true
                 }).ToListAsync();

            }

            action = await _repoObjective.FindAll().Where(x => x.PICs.Any(x => x.AccountId == accountId))
                      .Select(x => new L0Dto
                      {
                          TodolistId = x.ToDoList.Any(x => x.Level == ToDoListLevel.Target && x.CreatedBy == accountId) ? x.ToDoList.FirstOrDefault(x => x.Level == ToDoListLevel.Target && x.CreatedBy == accountId).Id : 0,
                          IsReject = x.ToDoList.Any(x => x.Level == ToDoListLevel.Target && x.CreatedBy == accountId) ? x.ToDoList.FirstOrDefault(x => x.Level == ToDoListLevel.Target && x.CreatedBy == accountId).IsReject : false,
                          IsRelease = x.ToDoList.Any(x => x.Level == ToDoListLevel.Target && x.CreatedBy == accountId) ? x.ToDoList.FirstOrDefault(x => x.Level == ToDoListLevel.Target && x.CreatedBy == accountId).IsRelease : false,
                          Topic = x.Topic,
                          Id = x.Id,
                          IsDisplayAction = true
                      }).ToListAsync();

            var data = action.Concat(resultOfMonth).Concat(selfScore);
            return data;
        }

        public async Task<object> L1(int accountId, DateTime currentTime)
        {
            var quarterSettings = new List<int> { Quarter.Q2, Quarter.Q4 }; // Quý 1 và Quý 2 L0 sẽ tự chấm điểm cho bản thân -> user demand
            var settingsForQuater1 = new List<int> { Quarter.Q1, Quarter.Q2 }; // Nếu rơi vào quý 1 thì sẽ lấy những tháng của quý 1 và quý 2
            var settingsForQuater3 = new List<int> { Quarter.Q3, Quarter.Q4 }; // Nếu rơi vào quý 3 thì sẽ lấy những tháng của quý 3 và quý 4

            var accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            int accountID = JWTExtensions.GetDecodeTokenById(accessToken);
            var accounts = await _repoAccount.FindAll(x => x.Manager == accountID).Select(x => x.Id).Distinct().ToListAsync();
            var kpi = new List<L1Dto>();
            var attitude = new List<L1Dto>();
            if (accounts.Count == 0) return attitude;

            var date = currentTime;

            var halfYearType = await _repoPeriodType.FindAll(x => x.Code == SystemPeriod.HalfYear).FirstOrDefaultAsync();
            var displayTimeHalfYear = date.AddDays(halfYearType.DisplayBefore).Date;

            var quarterType = await _repoPeriodType.FindAll(x => x.Code == SystemPeriod.Quarterly).FirstOrDefaultAsync();
            var displayTimeQuarter = date.AddDays(quarterType.DisplayBefore).Date;
            // Lấy settings của quý và nửa năm
            var quarterlyModel = await _repoPeriod.FindAll(x => x.PeriodType.Code == SystemPeriod.Quarterly && x.ReportTime.Date >= displayTimeQuarter).OrderBy(x => x.ReportTime).FirstOrDefaultAsync();
            var halfYearModel = await _repoPeriod.FindAll(x => x.PeriodType.Code == SystemPeriod.HalfYear && x.ReportTime.Date >= displayTimeHalfYear).OrderBy(x => x.ReportTime).FirstOrDefaultAsync();

            // Lấy tất cả các user đã giao nhiệm vụ cho họ
            var data = (await _repoObjective.FindAll(x => x.ToDoList.Any(a => accounts.Contains(a.CreatedBy)))
                .SelectMany(x => x.PICs)
                .ToListAsync())
                .DistinctBy(x => x.AccountId)
                .ToList();
            // Neu chua den ngay bao cao hang quy
            if (quarterlyModel != null)
            {
                var quarterlySettings = quarterlyModel.Months.Split(",").Select(int.Parse).ToList();
                var displayBeforQuarterly = quarterlyModel.PeriodType.DisplayBefore;
                var reportTimeQuarterly = quarterlyModel.ReportTime.AddDays(-displayBeforQuarterly);

                kpi = data.Select(x => new L1Dto
                {
                    Id = x.AccountId,
                    Objective = $"{quarterlyModel.Title} - {x.Account.FullName}",
                    DueDate = quarterlyModel.ReportTime,
                    Type = "KPI",
                    Period = quarterlyModel.Value,
                    PeriodTypeId = quarterlyModel.PeriodTypeId,
                    HalfYearId = SystemPeriodType.HalfYear,
                    Settings = quarterlySettings,
                    IsDisplayKPIScore = true,
                    IsDisplayAttitude = false,
                    HasFunctionalLeader = x.Account.Leader.HasValue
                }).ToList();
            }
            if (halfYearModel != null)
            {
                var halfYearSettings = halfYearModel.Months.Split(",").Select(int.Parse).ToList();
                var displayBeforHalfYear = halfYearModel.PeriodType.DisplayBefore;
                var reportTimeHalfYear = halfYearModel.ReportTime.AddDays(-displayBeforHalfYear);

                attitude = data.Select(x => new L1Dto
                {
                    Id = x.AccountId,
                    Objective = $"{halfYearModel.Title} - {x.Account.FullName}",
                    DueDate = halfYearModel.ReportTime,
                    Type = "Attitude",
                    Period = halfYearModel.Value,
                    PeriodTypeId = halfYearModel.PeriodTypeId,
                    Settings = halfYearSettings,
                    IsDisplayKPIScore = false,
                    IsDisplayAttitude = true,
                    HasFunctionalLeader = x.Account.Leader.HasValue
                }).ToList();
            }

            return kpi.Concat(attitude).ToList();
        }
        public async Task<object> FunctionalLeader(int accountId, DateTime currentTime)
        {

            var accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            int accountID = JWTExtensions.GetDecodeTokenById(accessToken);
            var date = currentTime;
            // Lấy settings của quý
            var halfYearType = await _repoPeriodType.FindAll(x => x.Code == SystemPeriod.HalfYear).FirstOrDefaultAsync();
            var displayTimeHalfYear = date.AddDays(halfYearType.DisplayBefore).Date;
            var halfYearModel = await _repoPeriod.FindAll(x => x.PeriodType.Code == SystemPeriod.HalfYear
            && x.ReportTime.Date >= displayTimeHalfYear).OrderBy(x => x.ReportTime).FirstOrDefaultAsync();

            var attitude = new List<FunctionalLeaderDto>();
            var accounts = await _repoAccount.FindAll(x => x.Leader == accountId).Select(x => x.Id).Distinct().ToListAsync();
            if (accounts.Count == 0) return attitude;
            // Lấy tất cả các user đã giao nhiệm vụ cho họ
            var data = (await _repoObjective.FindAll(x => x.ToDoList.Any(a => accounts.Contains(a.CreatedBy)))
            .SelectMany(x => x.PICs)
            .ToListAsync())
            .DistinctBy(x => x.AccountId)
            .ToList();
            if (halfYearModel != null)
            {
                var halfYearSettings = halfYearModel.Months.Split(",").Select(int.Parse).ToList();
                var displayBeforQuarterly = halfYearModel.PeriodType.DisplayBefore;
                var reportTimeQuarterly = halfYearModel.ReportTime.AddDays(-displayBeforQuarterly);

                attitude = data.Select(x => new FunctionalLeaderDto
                {
                    Id = x.AccountId,
                    Objective = $"{halfYearModel.Title} - {x.Account.FullName}",
                    DueDate = halfYearModel.ReportTime,
                    Type = "Attitude",
                    Period = halfYearModel.Value,
                    PeriodTypeId = halfYearModel.PeriodTypeId,
                    Settings = halfYearSettings,
                    IsDisplayKPIScore = false,
                    IsDisplayAttitude = true
                }).ToList();
                return attitude.ToList();
            }

            return attitude;
        }
        public async Task<object> L2(int accountId, DateTime currentTime)
        {
            var quarterSettings = new List<int> { Quarter.Q2, Quarter.Q4 }; // Quý 1 và Quý 2 L0 sẽ tự chấm điểm cho bản thân -> user demand
            var settingsForQuater1 = new List<int> { Quarter.Q1, Quarter.Q2 }; // Nếu rơi vào quý 1 thì sẽ lấy những tháng của quý 1 và quý 2
            var settingsForQuater3 = new List<int> { Quarter.Q3, Quarter.Q4 }; // Nếu rơi vào quý 3 thì sẽ lấy những tháng của quý 3 và quý 4
            var kpi = new List<L2Dto>();
            var attitude = new List<L2Dto>();
            var accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            int accountID = JWTExtensions.GetDecodeTokenById(accessToken);
            // tim oc cua usser login
            var ocuser = await _repoOCAccount.FindAll(x => x.AccountId == accountID).FirstOrDefaultAsync();
            var checkRole = await _repoAccountGroupAccount.FindAll(x => x.AccountId == accountID).Select(x => x.AccountGroup.Position).AnyAsync(x => SystemRole.L2 == x);
            if (checkRole == false) return new List<dynamic>();

            if (ocuser == null) return new List<dynamic>();
            // Lay tat ca con cua oc
            var oc = _repoOC.FindAll().AsHierarchy(x => x.Id, y => y.ParentId, ocuser.OCId, 2).ToList();
            var ocs = oc.Flatten(x => x.ChildNodes).Select(x => x.Entity.Id).ToList();
            // vao ocUser tim theo ocId list 
            var accountIds = await _repoOCAccount.FindAll(x => ocs.Contains(x.OCId)).Select(x => x.AccountId).Distinct().ToListAsync();

            var date = currentTime;
            var month = date.Month;
            int currentHalfYear = month <= 6 && month >= 1 ? 1 : 2;
        
            var halfYearType = await _repoPeriodType.FindAll(x => x.Code == SystemPeriod.HalfYear).FirstOrDefaultAsync();
            var displayTimeHalfYear = date.AddDays(halfYearType.DisplayBefore).Date;

            var quarterType = await _repoPeriodType.FindAll(x => x.Code == SystemPeriod.Quarterly).FirstOrDefaultAsync();
            var displayTimeQuarter = date.AddDays(quarterType.DisplayBefore).Date;

            var quarterlyModel = await _repoPeriod.FindAll(x => x.PeriodType.Code == SystemPeriod.Quarterly && x.ReportTime.Date >= displayTimeQuarter).OrderBy(x => x.ReportTime).FirstOrDefaultAsync();
            var halfYearModel = await _repoPeriod.FindAll(x => x.PeriodType.Code == SystemPeriod.HalfYear && x.ReportTime.Date >= displayTimeHalfYear).OrderBy(x => x.ReportTime).FirstOrDefaultAsync();
            // Lấy tất cả các user đã giao nhiệm vụ cho họ
            var data = (await _repoObjective.FindAll(x => accountIds.Contains(x.CreatedBy))
                .SelectMany(x => x.PICs)
                .ToListAsync())
                .DistinctBy(x => x.AccountId)
                .ToList();
            // Neu chua den ngay bao cao hang quy
            if (quarterlyModel != null)
            {
                var quarterlySettings = quarterlyModel.Months.Split(",").Select(int.Parse).ToList();
                var displayBeforQuarterly = quarterlyModel.PeriodType.DisplayBefore;
                var reportTimeQuarterly = quarterlyModel.ReportTime.AddDays(-displayBeforQuarterly);

                kpi = data.Select(x => new L2Dto
                {
                    Id = x.AccountId,
                    Objective = $"{quarterlyModel.Title} - {x.Account.FullName}",
                    DueDate = quarterlyModel.ReportTime,
                    Type = "KPI",
                    Period = quarterlyModel.Value,
                    PeriodTypeId = quarterlyModel.PeriodTypeId,
                    HalfYearId = SystemPeriodType.HalfYear,
                    Settings = quarterlySettings,
                    IsDisplayKPIScore = true,
                    IsDisplayAttitude = false,
                    HasFunctionalLeader = x.Account.Leader.HasValue,
                    FullName = x.Account.FullName
                }).ToList();
            }
            if (halfYearModel != null)
            {
                var halfYearSettings = halfYearModel.Months.Split(",").Select(int.Parse).ToList();
                var displayBeforHalfYear = halfYearModel.PeriodType.DisplayBefore;
                var reportTimeHalfYear = halfYearModel.ReportTime.AddDays(-displayBeforHalfYear);

                attitude = data.Select(x => new L2Dto
                {
                    Id = x.AccountId,
                    Objective = $"{halfYearModel.Title} - {x.Account.FullName}",
                    DueDate = halfYearModel.ReportTime,
                    Type = "Attitude",
                    Period = halfYearModel.Value,
                    PeriodTypeId = halfYearModel.PeriodTypeId,
                    Settings = halfYearSettings,
                    IsDisplayKPIScore = false,
                    IsDisplayAttitude = true,
                    HasFunctionalLeader = x.Account.Leader.HasValue,
                    FullName = x.Account.FullName
                }).ToList();
            }

            return kpi.Concat(attitude).ToList();
        }

        public async Task<object> GHR(int accountId, DateTime currentTime)
        {
            var accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            int accountID = JWTExtensions.GetDecodeTokenById(accessToken);
            var checkRole = await _repoAccountGroupAccount.FindAll(x => x.AccountId == accountID)
                .Select(x => x.AccountGroup.Position).AnyAsync(x => SystemRole.GHR == x);
            if (checkRole == false) return new List<GHRDto>();

            var date = currentTime;

            // Lấy settings của quý
            var quarterType = await _repoPeriodType.FindAll(x => x.Code == SystemPeriod.Quarterly).FirstOrDefaultAsync();
            var displayTimeQuarter = date.AddDays(quarterType.DisplayBefore).Date;
            var quarterlyModel = await _repoPeriod.FindAll(x => x.PeriodType.Code == SystemPeriod.Quarterly && x.ReportTime.Date >= displayTimeQuarter).OrderBy(x => x.ReportTime).FirstOrDefaultAsync();

            var kpi = new List<GHRDto>();
            if (quarterlyModel == null)
            {
                return kpi;
            }
            var quarterlySettings = quarterlyModel.Months.Split(",").Select(int.Parse).ToList();
            var displayBeforQuarterly = quarterlyModel.PeriodType.DisplayBefore;
            var reportTimeQuarterly = quarterlyModel.ReportTime.AddDays(-displayBeforQuarterly);
            // Lấy tất cả các user đã giao nhiệm vụ cho họ
            var data = (await _repoObjective.FindAll()
                .SelectMany(x => x.PICs)
                .ToListAsync())
                .DistinctBy(x => x.AccountId)
                .ToList();
            kpi = data.Select(x => new GHRDto
            {
                Id = x.AccountId,
                Objective = $"{quarterlyModel.Title} - {x.Account.FullName}",
                DueDate = quarterlyModel.ReportTime,
                Type = "KPI",
                Period = quarterlyModel.Value,
                PeriodTypeId = quarterlyModel.PeriodTypeId,
                Settings = quarterlySettings
            }).ToList();
            return kpi;
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
            var ocuser = await _repoOCAccount.FindAll(x => x.AccountId == accountID).FirstOrDefaultAsync();
            if (ocuser == null) return new List<dynamic>();
            // Lay tat ca con cua oc
            var oc = _repoOC.FindAll().AsHierarchy(x => x.Id, y => y.ParentId, ocuser.OCId).ToList();
            var ocs = oc.Flatten(x => x.ChildNodes).Select(x => x.Entity.Id).ToList();
            // vao ocUser tim theo ocId list 
            var accountIds = await _repoOCAccount.FindAll(x => ocs.Contains(x.OCId)).Select(x => x.AccountId).Distinct().ToListAsync();

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


            var quarterly = quarterlyModel.Where(x => x.Months.Contains(month)).FirstOrDefault();
            var quarterlySettings = quarterly.Months;

            var displayBeforQuarterly = quarterly.DisplayBefore;
            var reportTimeQuarterly = quarterly.ReportTime.AddDays(-displayBeforQuarterly);

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

        public async Task<object> GetAllAttitudeScoreL1ByAccountId(int accountId)
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

            var data = await _repoObjective.FindAll(x => x.ToDoList.Select(a => a.CreatedBy).Contains(accountId))
                .Select(x => new
                {
                    Objective = x.Topic,
                    L0TargetList = x.ToDoList.FirstOrDefault(a => a.Level == ToDoListLevel.Target && a.CreatedBy == accountId).Action,
                    L0ActionList = x.ToDoList.Where(a => a.Level == ToDoListLevel.Action && a.CreatedBy == accountId).Select(a => a.Action).ToList(),
                    ResultOfMonth = x.ResultOfMonth.Where(a => a.CreatedBy == accountId).Select(a => new { a.Month, a.Title, a.ObjectiveId, a.CreatedBy }),
                    Settings = monthsOfCurrentHalfYear,
                    PeriodTypeId = halfYear.PeriodTypeId,
                    Period = halfYear.Value
                })
                .ToListAsync();


            return data;
        }
        public async Task<object> GetAllKPIScoreL1ByAccountId(int accountId, DateTime currentTime)
        {
            var date = currentTime;
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

            var data = await _repoObjective.FindAll(x => x.ToDoList.Select(a => a.CreatedBy).Contains(accountId))
                
                .Select(x => new
                {
                    Objective = x.Topic,
                    L0TargetList = x.ToDoList.FirstOrDefault(a => a.Level == ToDoListLevel.Target && a.CreatedBy == accountId).Action,
                    L0ActionList = x.ToDoList.Where(a => a.Level == ToDoListLevel.Action && a.CreatedBy == accountId).Select(a => a.Action).ToList(),
                    ResultOfMonth = x.ResultOfMonth.Where(a=> a.CreatedBy == accountId).Select(a => new { a.Month, a.Title, a.ObjectiveId, a.CreatedBy }),
                    Settings = monthsOfCurrentQuarter,
                    PeriodTypeId = quarterly.PeriodTypeId,
                    Period = quarterly.Value
                })
                .ToListAsync();


            return data;
        }
        public async Task<object> GetAllKPIScoreL2ByAccountId(int accountId, DateTime currentTime)
        {
            var date = currentTime;
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

            var data = await _repoObjective.FindAll(x => x.ToDoList.Select(a => a.CreatedBy).Contains(accountId))

                .Select(x => new
                {
                    Objective = x.Topic,
                    L0TargetList = x.ToDoList.FirstOrDefault(a => a.Level == ToDoListLevel.Target && a.CreatedBy == accountId).Action,
                    L0ActionList = x.ToDoList.Where(a => a.Level == ToDoListLevel.Action && a.CreatedBy == accountId).Select(a => a.Action).ToList(),
                    ResultOfMonth = x.ResultOfMonth.Where(a => a.CreatedBy == accountId).Select(a => new { a.Month, a.Title, a.ObjectiveId, a.CreatedBy }),
                    Settings = monthsOfCurrentQuarter,
                    PeriodTypeId = quarterly.PeriodTypeId,
                    Period = quarterly.Value
                })
                .ToListAsync();


            return data;
        }
        public async Task<object> GetAllKPIScoreGHRByAccountId(int accountId, DateTime currentTime)
        {
            var date = currentTime;
            // Lấy settings của quý
            var quarterType = await _repoPeriodType.FindAll(x => x.Code == SystemPeriod.Quarterly).FirstOrDefaultAsync();
            var displayTimeQuarter = date.AddDays(quarterType.DisplayBefore).Date;
            var quarterlyModel = await _repoPeriod.FindAll(x => x.PeriodType.Code == SystemPeriod.Quarterly && x.ReportTime.Date >= displayTimeQuarter).OrderBy(x => x.ReportTime).FirstOrDefaultAsync();

            var kpi = new List<dynamic>();

            if (quarterlyModel == null)
            {
                return kpi;
            }
            var monthsOfCurrentQuarter = quarterlyModel.Months.Split(",").Select(int.Parse).ToList();
            var displayBeforQuarterly = quarterlyModel.PeriodType.DisplayBefore;
            var reportTimeQuarterly = quarterlyModel.ReportTime.AddDays(-displayBeforQuarterly);
            var data = await _repoObjective.FindAll(x => x.ToDoList.Any(a=> a.CreatedBy == accountId))
                .Select(x => new
                {
                    Id = x.Id,
                    TodolistId = x.ToDoList.FirstOrDefault(x => x.Level == ToDoListLevel.Target && x.CreatedBy == accountId).Id,
                    IsReject = x.ToDoList.FirstOrDefault(x => x.Level == ToDoListLevel.Target && x.CreatedBy == accountId).IsReject,
                    IsRelease = x.ToDoList.FirstOrDefault(x => x.Level == ToDoListLevel.Target && x.CreatedBy == accountId).IsRelease,
                    Objective = x.Topic,
                    L0TargetList = x.ToDoList.FirstOrDefault(x => x.Level == ToDoListLevel.Target && x.CreatedBy == accountId).Action,
                    L0ActionList = x.ToDoList.Where(x => x.Level == ToDoListLevel.Action && x.CreatedBy == accountId).Select(a => a.Action).ToList(),
                    Deadline = x.ToDoList.FirstOrDefault(x => x.Level == ToDoListLevel.Target && x.CreatedBy == accountId).Deadline,
                    PeriodTypeId = quarterlyModel.PeriodTypeId,
                    Period = quarterlyModel.Value
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

        /// <summary>
        /// Lấy dữ liệu của quý 2 và quý 4
        /// </summary>
        /// <param name="period"></param>
        /// <returns></returns>
        public async Task<object> GetAllKPIScoreL0ByPeriod(int period)
        {
            var quarterSettings = new List<int> { Quarter.Q2, Quarter.Q4 }; // Quý 2 và Quý 4 L0 sẽ tự chấm điểm cho bản thân -> user demand
            var settingsForQuater2 = new List<int> { Quarter.Q1, Quarter.Q2 }; // Nếu rơi vào quý 2 thì sẽ lấy những tháng của quý 1 và quý 2
            var settingsForQuater4 = new List<int> { Quarter.Q3, Quarter.Q4 }; // Nếu rơi vào quý 4 thì sẽ lấy những tháng của quý 3 và quý 4
            var accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            int accountId = JWTExtensions.GetDecodeTokenById(accessToken);
            var date = DateTime.Today;
            var month = date.Month;
            // Lấy settings của quý và nửa năm
            var settings = new List<PeriodDto> { };
            if (settingsForQuater2.Contains(period))
            {
                settings = (await _repoPeriod.FindAll(x => x.PeriodType.Code == SystemPeriod.Quarterly && settingsForQuater2.Contains(x.Value)).ToListAsync())
               .Select(x => new PeriodDto
               {
                   Title = x.Title,
                   PeriodTypeId = x.PeriodTypeId,
                   Value = x.Value,
                   ReportTime = x.ReportTime,
                   Months = x.Months
               }).ToList();
            }
            if (settingsForQuater4.Contains(period))
            {
                settings = (await _repoPeriod.FindAll(x => x.PeriodType.Code == SystemPeriod.Quarterly && settingsForQuater4.Contains(x.Value)).ToListAsync())
               .Select(x => new PeriodDto
               {
                   Title = x.Title,
                   PeriodTypeId = x.PeriodTypeId,
                   Value = x.Value,
                   ReportTime = x.ReportTime,
                   Months = x.Months
               }).ToList();
            }
            var quarterlyModel = settings
              .Select(x => new
              {
                  x.Title,
                  x.PeriodTypeId,
                  x.Value,
                  x.ReportTime,
                  Months = x.Months.Split(",").Select(int.Parse).ToList()
              }).ToList();
            var quarterly = quarterlyModel.Where(x => x.Months.Contains(month)).FirstOrDefault();
            var monthsOfCurrentQuarter = quarterly.Months.OrderBy(x => x).ToList();

            var data = await _repoObjective.FindAll(x => x.PICs.Select(a => a.AccountId).Contains(accountId))
                .Where(x => x.ResultOfMonth.Any(a => monthsOfCurrentQuarter.Contains(a.Month)))
                .Select(x => new
                {
                    Objective = x.Topic,
                    L0TargetList = x.ToDoList.FirstOrDefault(x => x.Level == ToDoListLevel.Target).Action,
                    L0ActionList = x.ToDoList.Where(x => x.Level == ToDoListLevel.Action).Select(a => a.Action).ToList(),
                    ResultOfMonth = x.ResultOfMonth.Select(a => new { a.Month, a.Title, a.ObjectiveId, a.CreatedBy }),
                    Settings = monthsOfCurrentQuarter,
                    PeriodTypeId = quarterly.PeriodTypeId,
                    Period = quarterly.Value
                })
                .ToListAsync();


            return data;
        }

        public async Task<object> GetAllAttitudeScoreByFunctionalLeader()
        {
            var accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            int accountId = JWTExtensions.GetDecodeTokenById(accessToken);
            var date = DateTime.Today;
            var month = date.Month;
            int currentHalfYear = month <= 6 && month >= 1 ? 1 : 2;
            var accounts = await _repoAccount.FindAll(x => x.Leader == accountId).Select(x => x.Id).ToListAsync();
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

            var data = await _repoObjective.FindAll(x => x.PICs.Any(a => accounts.Contains(a.AccountId)))
                .Where(x => x.ResultOfMonth.Any(a => monthsOfCurrentHalfYear.Contains(a.Month)))
                .Select(x => new
                {
                    Objective = x.Topic,
                    L0TargetList = x.ToDoList.FirstOrDefault(x => x.Level == ToDoListLevel.Target).Action,
                    L0ActionList = x.ToDoList.Where(x => x.Level == ToDoListLevel.Action).Select(a => a.Action).ToList(),
                    ResultOfMonth = x.ResultOfMonth.Select(a => new { a.Month, a.Title, a.ObjectiveId, a.CreatedBy }),
                    Settings = monthsOfCurrentHalfYear,
                    PeriodTypeId = halfYear.PeriodTypeId,
                    Period = halfYear.Value
                })
                .ToListAsync();


            return data;
        }
        public async Task<OperationResult> Reject(List<int> ids)
        {
            var accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            int accountID = JWTExtensions.GetDecodeTokenById(accessToken);
            var items = await _repo.FindAll(x => ids.Contains(x.Id)).ToListAsync();
            foreach (var item in items)
            {
                item.IsReject = true;
                item.IsRelease = false;
            }

            try
            {
                _repo.UpdateRange(items);
                await _unitOfWork.SaveChangeAsync();

                operationResult = new OperationResult
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = MessageReponse.AddSuccess,
                    Success = true,
                    Data = items
                };
            }
            catch (Exception ex)
            {
                operationResult = ex.GetMessageError();
            }
            return operationResult;
        }

        public async Task<OperationResult> Release(List<int> ids)
        {
            var accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            int accountID = JWTExtensions.GetDecodeTokenById(accessToken);
            var items = await _repo.FindAll(x => ids.Contains(x.Id)).ToListAsync();
            foreach (var item in items)
            {
                item.IsReject = false;
                item.IsRelease = true;
            }

            try
            {
                _repo.UpdateRange(items);
                await _unitOfWork.SaveChangeAsync();

                operationResult = new OperationResult
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = MessageReponse.AddSuccess,
                    Success = true,
                    Data = items
                };
            }
            catch (Exception ex)
            {
                operationResult = ex.GetMessageError();
            }
            return operationResult;
        }

        public async Task<OperationResult> DisableReject(List<int> ids)
        {
            var accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            int accountID = JWTExtensions.GetDecodeTokenById(accessToken);
            var items = await _repo.FindAll(x => ids.Contains(x.Id)).ToListAsync();
            foreach (var item in items)
            {
                item.IsReject = false;
            }

            try
            {
                _repo.UpdateRange(items);
                await _unitOfWork.SaveChangeAsync();

                operationResult = new OperationResult
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = MessageReponse.AddSuccess,
                    Success = true,
                    Data = items
                };
            }
            catch (Exception ex)
            {
                operationResult = ex.GetMessageError();
            }
            return operationResult;
        }

        public async Task<object> GetAllAttitudeScoreL2ByAccountId(int accountId)
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

            var data = await _repoObjective.FindAll(x => x.ToDoList.Select(a => a.CreatedBy).Contains(accountId))
                .Select(x => new
                {
                    Objective = x.Topic,
                    L0TargetList = x.ToDoList.FirstOrDefault(a => a.Level == ToDoListLevel.Target && a.CreatedBy == accountId).Action,
                    L0ActionList = x.ToDoList.Where(a => a.Level == ToDoListLevel.Action && a.CreatedBy == accountId).Select(a => a.Action).ToList(),
                    ResultOfMonth = x.ResultOfMonth.Where(a => a.CreatedBy == accountId).Select(a => new { a.Month, a.Title, a.ObjectiveId, a.CreatedBy }),
                    Settings = monthsOfCurrentHalfYear,
                    PeriodTypeId = halfYear.PeriodTypeId,
                    Period = halfYear.Value
                })
                .ToListAsync();


            return data;
        }
        public async Task<object> GetAllAttitudeScoreGHRByAccountId(int accountId)
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

            var data = await _repoObjective.FindAll(x => x.ToDoList.Select(a => a.CreatedBy).Contains(accountId))
                .Select(x => new
                {
                    Objective = x.Topic,
                    L0TargetList = x.ToDoList.FirstOrDefault(a => a.Level == ToDoListLevel.Target && a.CreatedBy == accountId).Action,
                    L0ActionList = x.ToDoList.Where(a => a.Level == ToDoListLevel.Action && a.CreatedBy == accountId).Select(a => a.Action).ToList(),
                    ResultOfMonth = x.ResultOfMonth.Where(a => a.CreatedBy == accountId).Select(a => new { a.Month, a.Title, a.ObjectiveId, a.CreatedBy }),
                    Settings = monthsOfCurrentHalfYear,
                    PeriodTypeId = halfYear.PeriodTypeId,
                    Period = halfYear.Value
                })
                .ToListAsync();


            return data;
        }
        public async Task<object> GetAllAttitudeScoreGFLByAccountId(int accountId)
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

            var data = await _repoObjective.FindAll(x => x.ToDoList.Select(a => a.CreatedBy).Contains(accountId))
                .Select(x => new
                {
                    Objective = x.Topic,
                    L0TargetList = x.ToDoList.FirstOrDefault(a => a.Level == ToDoListLevel.Target && a.CreatedBy == accountId).Action,
                    L0ActionList = x.ToDoList.Where(a => a.Level == ToDoListLevel.Action && a.CreatedBy == accountId).Select(a => a.Action).ToList(),
                    ResultOfMonth = x.ResultOfMonth.Where(a => a.CreatedBy == accountId).Select(a => new { a.Month, a.Title, a.ObjectiveId, a.CreatedBy }),
                    Settings = monthsOfCurrentHalfYear,
                    PeriodTypeId = halfYear.PeriodTypeId,
                    Period = halfYear.Value
                })
                .ToListAsync();


            return data;
        }

        public async Task<OperationResult> ReportQ1orQ3(Q1OrQ3Request request)
        {
            var exportList = new List<Q1OrQ3Export>();
            var l1Score = await _repoKPIScore.FindAll(x => x.Period == request.Period && x.PeriodTypeId == request.PeriodTypeId).ToListAsync();


            var settings = await _repoPeriod.FindAll(x => x.Id == request.Period && x.PeriodTypeId == request.PeriodTypeId).FirstOrDefaultAsync();

            if (settings == null) return new OperationResult
            {
                StatusCode = HttpStatusCode.BadRequest,
                Message = "Could not found the period settings",
                Success = false
            };
            var months = settings.Months.Split(",").Select(int.Parse).ToList();
            var todolist = _repo.FindAll(x => months.Contains(x.CreatedTime.Month)).Select(x => x.CreatedBy).Distinct().ToListAsync();
            //foreach (var item in todolist)
            //{

            //}
            throw new NotImplementedException();
        }
    }
}
