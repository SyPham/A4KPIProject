using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
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
    public interface IContributionService: IServiceBase<Contribution, ContributionDto>
    {
        Task<List<ContributionDto>> GetAllByObjectiveId(int objectiveId);
        Task<ContributionDto> GetFisrtByAccountId(int accountId, int periodTypeId, int period, string scoreType);
        /// <summary>
        /// Lấy nội dung L1 comment cho L0
        /// </summary>
        /// <param name="accountId">L0</param>
        /// <param name="periodTypeId">Loại hàng tháng, quý, năm</param>
        /// <param name="period">Quý, tháng, năm Ex: 1,2,3...</param>
        /// <returns>Nội dung L1 comment cho L0</returns>
        Task<ContributionDto> GetL1CommentByAccountId(int accountId, int periodTypeId, int period);
    }
    public class ContributionService : ServiceBase<Contribution, ContributionDto>, IContributionService
    {
        private OperationResult operationResult;

        private readonly IRepositoryBase<Contribution> _repo;
        private readonly IRepositoryBase<Account> _repoAccount;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        public ContributionService(
            IRepositoryBase<Contribution> repo,
            IRepositoryBase<Account> repoAccount,
            IHttpContextAccessor httpContextAccessor,
            IUnitOfWork unitOfWork,
            IMapper mapper, 
            MapperConfiguration configMapper
            )
            : base(repo, unitOfWork, mapper,  configMapper)
        {
            _repo = repo;
            _repoAccount = repoAccount;
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configMapper = configMapper;
        }
        public async Task<ContributionDto> GetFisrtByAccountId(int accountId, int periodTypeId, int period, string scoreType)
        {
            var accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            int createdBy = JWTExtensions.GetDecodeTokenById(accessToken);
          
            return await _repo.FindAll(x => x.PeriodTypeId == periodTypeId 
            && x.CreatedTime.Year == DateTime.Today.Year 
            && x.Period == period 
            && accountId == x.AccountId 
            && scoreType == x.ScoreType
            && createdBy == x.CreatedBy 
            && x.AccountId != createdBy
            ).ProjectTo<ContributionDto>(_configMapper).FirstOrDefaultAsync();
        }
        public async Task<List<ContributionDto>> GetAllByObjectiveId(int objectiveId)
        {
            var currrentQuarter = (DateTime.Now.Month + 2) / 3;

            return await _repo.FindAll(x => x.Period == currrentQuarter).ProjectTo<ContributionDto>(_configMapper).ToListAsync();
        }
        /// <summary>
        /// Chỉnh sửa thành vừa cập nhật vừa thêm mới
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public override async Task<OperationResult> AddAsync(ContributionDto model)
        {
            if (model.Id > 0)
            {
                var item = await _repo.FindAll(x => x.Id == model.Id && x.CreatedBy == model.CreatedBy).AsNoTracking().FirstOrDefaultAsync();
                item.Content = model.Content;
                _repo.Update(item);
            }
            else
            {
                var itemList = _mapper.Map<Contribution>(model);
                _repo.Add(itemList);
            }
            try
            {
                await _unitOfWork.SaveChangeAsync();
                operationResult = new OperationResult
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = MessageReponse.AddSuccess,
                    Success = true,
                    Data = model
                };
            }
            catch (Exception ex)
            {
                operationResult = ex.GetMessageError();
            }
            return operationResult;
        }

        public async  Task<ContributionDto> GetL1CommentByAccountId(int accountId, int periodTypeId, int period)
        {
            var l0 = await _repoAccount.FindAll(x => x.Id == accountId).FirstOrDefaultAsync();
            if (l0 == null) return new ContributionDto();
            return await _repo.FindAll(x => x.ScoreType == ScoreType.L1 && x.PeriodTypeId == periodTypeId && x.CreatedTime.Year == DateTime.Today.Year && x.Period == period && accountId == x.AccountId && l0.Manager == x.CreatedBy && x.AccountId != l0.Manager).ProjectTo<ContributionDto>(_configMapper).FirstOrDefaultAsync();
        }
    }
}
