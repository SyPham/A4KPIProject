using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
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
        Task<List<ToDoListDto>> GetAllByObjectiveIdAsync(int objectiveId);
        Task<List<ToDoListByLevelL1L2Dto>> GetAllInCurrentQuarterByAccountGroup(int accountId);
        Task<object> GetAllObjectiveByL1L2();

    }
    public class ToDoListService : ServiceBase<ToDoList, ToDoListDto>, IToDoListService
    {
        private OperationResult operationResult;
        private readonly IRepositoryBase<ToDoList> _repo;
        private readonly IRepositoryBase<Account> _repoAccount;
        private readonly IRepositoryBase<Objective> _repoObjective;
        private readonly IRepositoryBase<ResultOfMonth> _repoResultOfMonth;
        private readonly IRepositoryBase<KPIScore> _repoKPIScore;
        private readonly IRepositoryBase<AttitudeScore> _repoAttitudeScore;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        public ToDoListService(
            IRepositoryBase<ToDoList> repo,
            IRepositoryBase<Account> repoAccount,
            IRepositoryBase<Objective> repoObjective,
            IRepositoryBase<ResultOfMonth> repoResultOfMonth,
            IRepositoryBase<KPIScore> repoKPIScore,
            IRepositoryBase<AttitudeScore> repoAttitudeScore,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            MapperConfiguration configMapper
            )
            : base(repo, unitOfWork, mapper, configMapper)
        {
            _repo = repo;
            _repoAccount = repoAccount;
            _repoObjective = repoObjective;
            _repoResultOfMonth = repoResultOfMonth;
            _repoKPIScore = repoKPIScore;
            _repoAttitudeScore = repoAttitudeScore;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configMapper = configMapper;
        }
        public async Task<List<ToDoListDto>> GetAllByObjectiveIdAsync(int objectiveId)
        {
            return await _repo.FindAll(x => x.ObjectiveId == objectiveId).ProjectTo<ToDoListDto>(_configMapper).ToListAsync();

        }

        public async Task<List<ToDoListByLevelL1L2Dto>> GetAllInCurrentQuarterByAccountGroup(int accountId)
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

            var role = await _repoAccount.FindAll(x => x.Id == accountId).FirstOrDefaultAsync();
            var positions = new List<int> { SystemRole.L1, SystemRole.L2 };
            if (positions.Contains(role.AccountGroup.Position))
            {
                var data = await _repoObjective.FindAll().Select(x => new ToDoListByLevelL1L2Dto
                {
                    Id = x.Id,
                    Objective = x.Topic,
                    L0TargetList = x.ToDoList.Select(x => x.YourObjective).ToList(),
                    L0ActionList =x.ToDoList.Select(x => x.Action).ToList(),
                    Result1OfMonth = x.ResultOfMonth.Where(a => monthlist[0].Equals(a.Month)).FirstOrDefault().Title,
                    Result2OfMonth = x.ResultOfMonth.Where(a => monthlist[1].Equals(a.Month)).FirstOrDefault().Title,
                    Result3OfMonth = x.ResultOfMonth.Where(a => monthlist[2].Equals(a.Month)).FirstOrDefault().Title,
                }).ToListAsync();
                return data;
            }

            return new List<ToDoListByLevelL1L2Dto> { };
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
                         join b in _repoResultOfMonth.FindAll(x => monthlist.Contains(x.Month)) on a.ObjectiveId equals b.ObjectiveId
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
    }
}
