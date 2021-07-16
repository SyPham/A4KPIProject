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
    public interface ISpecialContributionScoreService : IServiceBase<SpecialContributionScore, SpecialContributionScoreDto>
    {
       
        Task<SpecialContributionScoreDto> GetFisrtByAccountId(int accountId, int periodTypeId, int period, string scoreType);
        /// <summary>
        /// Lấy điểm special score L1 chấm cho L0
        /// </summary>
        /// <param name="accountId">L0</param>
        /// <param name="periodTypeId">Loại hàng tháng, quý, năm</param>
        /// <param name="period">Quý, tháng, năm Ex: 1,2,3...</param>
        /// <returns>Điểm special score L1 chấm cho L0</returns>
        Task<SpecialContributionScoreDto> GetSpecialScoreByAccountId(int accountId, int periodTypeId, int period, string scoreType);

    }
    public class SpecialContributionScoreService : ServiceBase<SpecialContributionScore, SpecialContributionScoreDto>, ISpecialContributionScoreService
    {
        private readonly IRepositoryBase<SpecialContributionScore> _repo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IRepositoryBase<Account> _repoAccount;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly MapperConfiguration _configMapper;
        private OperationResult operationResult;

        public SpecialContributionScoreService(
            IRepositoryBase<SpecialContributionScore> repo,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IRepositoryBase<Account> repoAccount,
             IHttpContextAccessor httpContextAccessor,
            MapperConfiguration configMapper
            )
            : base(repo, unitOfWork, mapper, configMapper)
        {
            _repo = repo;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _repoAccount = repoAccount;
            _httpContextAccessor = httpContextAccessor;
            _configMapper = configMapper;
        }
        
        public async Task<SpecialContributionScoreDto> GetFisrtByAccountId(int accountId, int periodTypeId,int period, string scoreType)
        {
            var accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            int scoreBy = JWTExtensions.GetDecodeTokenById(accessToken);

            return await _repo.FindAll(x => x.ScoreType == scoreType 
                                        && x.PeriodTypeId == periodTypeId 
                                        && x.CreatedTime.Year == DateTime.Today.Year 
                                        && x.Period == period
                                        && accountId == x.AccountId 
                                        && scoreBy == x.ScoreBy 
                                        && x.AccountId != scoreBy)
                                .ProjectTo<SpecialContributionScoreDto>(_configMapper)
                                .FirstOrDefaultAsync();

        }
        /// <summary>
        /// Chỉnh sửa thành vừa cập nhật vừa thêm mới
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public override async Task<OperationResult> AddAsync(SpecialContributionScoreDto model)
        {
            if (model.Id > 0)
            {
                var item = await _repo.FindAll(x => x.Id == model.Id && x.ScoreBy == model.ScoreBy).AsNoTracking().FirstOrDefaultAsync();
                item.Point = model.Point;
                _repo.Update(item);
            }
            else
            {
                var itemList = _mapper.Map<SpecialContributionScore>(model);
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
        public async Task<SpecialContributionScoreDto> GetSpecialScoreByAccountId(int accountId, int periodTypeId, int period, string scoreType)
        {
            var l0 = await _repoAccount.FindAll(x => x.Id == accountId).FirstOrDefaultAsync();
            if (l0 == null) return new SpecialContributionScoreDto();
            return await _repo.FindAll(x => x.PeriodTypeId == periodTypeId && x.CreatedTime.Year == DateTime.Today.Year && x.Period == period && accountId == x.AccountId && l0.Manager == x.ScoreBy && x.AccountId != l0.Manager).ProjectTo<SpecialContributionScoreDto>(_configMapper).FirstOrDefaultAsync();
        }
    }
}
