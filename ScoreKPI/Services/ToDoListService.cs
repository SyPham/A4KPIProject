using AutoMapper;
using AutoMapper.QueryableExtensions;
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
        /// Lấy tất cả KPI Objective của PIC
        /// Nếu quyền là L1, L2, FHO, GHR, GM thì sẽ để trống
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        Task<object> L0(int accountId);
        /// <summary>
        /// Lấy tất cả KPI Objective của PIC
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        Task<object> L1(int accountId);

        /// <summary>
        /// Lấy tất cả KPI Objective của PIC
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        Task<object> L2(int accountId);

        Task<List<ToDoListDto>> GetAllByObjectiveIdAsync(int objectiveId);
        Task<List<ToDoListByLevelL1L2Dto>> GetAllInCurrentQuarterByAccountGroup(int accountId);
        Task<List<ToDoListByLevelL1L2Dto>> GetAllKPIScoreByAccountId(int accountId);
        Task<object> GetAllKPISelfScoreByObjectiveId(int objectiveId, int accountId);
        Task<object> GetAllObjectiveByL1L2();

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
        private readonly IRepositoryBase<Period> _repoPeriod;
        private readonly IRepositoryBase<AttitudeScore> _repoAttitudeScore;
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
            IRepositoryBase<Period> repoPeriod,
            IRepositoryBase<AttitudeScore> repoAttitudeScore,
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
            _repoPeriod = repoPeriod;
            _repoAttitudeScore = repoAttitudeScore;
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
            var monthlist = quarterly.Months.Split(',').Select(int.Parse).OrderBy(x=>x).ToList();

            var data = await _repoObjective.FindAll().Select(x => new ToDoListByLevelL1L2Dto
            {
                Id = x.Id,
                Objective = x.Topic,
                L0TargetList = x.ToDoList.Select(x => x.YourObjective).ToList(),
                L0ActionList = x.ToDoList.Select(x => x.Action).ToList(),
            }).ToListAsync();
            return data;

        }

        public async Task<object> GetAllKPISelfScoreByObjectiveId(int objectiveId,int accountId)
        {
            int currentQuarter = (DateTime.Now.Month + 2) / 3;
            var quarterly = await _repoPeriod.FindAll(x => x.PeriodType.Code == SystemPeriod.Quarterly && x.Value == currentQuarter).FirstOrDefaultAsync();
            if (!quarterly.Months.Contains(','))
            {
                return new List<SelfScoreDto> { };
            }
            var monthlist = quarterly.Months.Split(',').Select(int.Parse).OrderBy(x => x).ToList();
            
            var data = await _repo.FindAll(x=>x.CreatedBy == accountId).SelectMany(x=> 
              x.Objective.ResultOfMonth.Where(x=>x.ObjectiveId == objectiveId)
            ).ToListAsync();
            var model = (from a in monthlist
                        join b in data.DistinctBy(x=>new { x.Month, x.ObjectiveId }).ToList() on a equals b.Month into ab
                        from c in ab.DefaultIfEmpty()
                        select new SelfScoreDto
                        {
                            Month = a,
                            ObjectiveList = c != null ? c.Objective.ToDoList.Select(x => x.YourObjective).ToList() : new List<string>(),
                            ResultOfMonth = c != null ? c.Title : "N/A",
                        }).ToList();
        
            return  model;

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

        public async Task<object> L0(int accountId)
        {
            return await _repoObjective.FindAll().Where(x => x.PICs.Any(x => x.AccountId == accountId)).ProjectTo<ObjectiveDto>(_configMapper).ToListAsync();
        }

        public async Task<object> L1(int accountId)
        {
            int currentQuarter = (DateTime.Now.Month + 2) / 3;
            var date = DateTime.Today.AddMonths(-6);
            var month = date.Month;
            int currentHalfYear = month <= 6 ? 1 : 2;

            // Lấy settings của quý và nửa năm
            var quarterly = await _repoPeriod.FindAll(x => x.PeriodType.Code == SystemPeriod.Quarterly && x.Value == currentQuarter).FirstOrDefaultAsync();
            var halfYear = await _repoPeriod.FindAll(x => x.PeriodType.Code == SystemPeriod.HalfYear && x.Value == currentHalfYear).FirstOrDefaultAsync();

            // Lấy tất cả các user đã giao nhiệm vụ cho họ
            var data = (await _repoObjective.FindAll(x => x.CreatedBy == accountId)
                .SelectMany(x => x.PICs)
                .ToListAsync())
                .DistinctBy(x => x.AccountId)
                .ToList();
            var kpi = data.Select(x => new
            {
                Id = x.Id,
                Objective = $"{quarterly.Title} - {x.Account.FullName}",
                Type = "KPI"
            }).ToList();
            var attitude = data.Select(x => new
            {
                Id = x.Id,
                Objective = $"{quarterly.Title} - {x.Account.FullName}",
                Type = "Attitude"
            }).ToList();
            return kpi.Concat(attitude).ToList();
        }

        public Task<object> L2(int accountId)
        {
            throw new NotImplementedException();
        }

        public Task<List<ToDoListByLevelL1L2Dto>> GetAllInCurrentQuarterByAccountGroup(int accountId)
        {
            throw new NotImplementedException();
        }
    }
}
