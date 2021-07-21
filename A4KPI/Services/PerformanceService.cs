using AutoMapper;
using A4KPI.Data;
using A4KPI.DTO;
using A4KPI.Models;
using A4KPI.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using A4KPI.Constants;
using A4KPI.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace A4KPI.Services
{
    public interface IPerformanceService : IServiceBase<Performance, PerformanceDto>
    {
        Task<OperationResult> Submit(List<PerformanceDto> performances);
        Task<object> GetKPIObjectivesByUpdater();
    }
    public class PerformanceService : ServiceBase<Performance, PerformanceDto>, IPerformanceService
    {
        private readonly IRepositoryBase<Performance> _repo;
        private readonly IRepositoryBase<OC> _repoOC;
        private readonly IRepositoryBase<OCAccount> _repoOCAccount;
        private readonly IRepositoryBase<AccountGroupAccount> _repoAccountGroupAccount;
        private readonly IRepositoryBase<Objective> _repoObjective;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly MapperConfiguration _configMapper;
        private OperationResult operationResult;

        public PerformanceService(
            IRepositoryBase<Performance> repo,
            IRepositoryBase<OC> repoOC,
            IRepositoryBase<OCAccount> repoOCAccount,
            IRepositoryBase<AccountGroupAccount> repoAccountGroupAccount,
            IRepositoryBase<Objective> repoObjective,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            MapperConfiguration configMapper
            )
            : base(repo, unitOfWork, mapper, configMapper)
        {
            _repo = repo;
            _repoOC = repoOC;
            _repoOCAccount = repoOCAccount;
            _repoAccountGroupAccount = repoAccountGroupAccount;
            _repoObjective = repoObjective;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _configMapper = configMapper;
        }

        public async Task<object> GetKPIObjectivesByUpdater()
        {
            var currentMonth = DateTime.Today.Month;
            var accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            int accountId = JWTExtensions.GetDecodeTokenById(accessToken);
            var ocId = await _repoOCAccount.FindAll(x => x.AccountId == accountId).Select(x=>x.OCId).FirstOrDefaultAsync();
            var checkRole = await _repoAccountGroupAccount.FindAll(x => x.AccountId == accountId).Select(x => x.AccountGroup.Position).AnyAsync(x => SystemRole.Updater == x);
            if (checkRole == false) return new List<PerformanceDto>();

            var fho = await _repoOC.FindAll(x => x.Id == ocId).Select(x => x.ParentId).FirstOrDefaultAsync();
            var factoryHead = await _repoOCAccount.FindAll(x => x.OCId == fho).Select(x=>x.AccountId).ToListAsync();

            var model = await (from a in _repoObjective.FindAll(x => factoryHead.Contains(x.CreatedBy))
                               join b in _repo.FindAll(x => x.Month == currentMonth && x.UploadBy == accountId) on a.Id equals b.ObjectiveId into ab
                               from c in ab.DefaultIfEmpty()
                               select new PerformanceDto
                               {
                                   Id = c != null ? c.Id : 0,
                                   UploadBy = accountId,
                                   ObjectiveId = a.Id,
                                   ObjectiveName = a.Topic,
                                   Month = currentMonth,
                                   CreatedTime = c != null ? c.CreatedTime : DateTime.MinValue,
                                   Percentage = c != null ? c.Percentage : 0
                               }).ToListAsync();
            return model;
        }

        public async Task<OperationResult> Submit(List<PerformanceDto> performances)
        {
            try
            {
                var items = _mapper.Map<List<Performance>>(performances);
                if (items.All(x => x.Id > 0))
                    _repo.UpdateRange(items);
                else
                    _repo.AddRange(items);
                await _unitOfWork.SaveChangeAsync();

                operationResult = new OperationResult
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = MessageReponse.AddSuccess,
                    Success = true,
                    Data = performances
                };
            }
            catch (Exception ex)
            {
                operationResult = ex.GetMessageError();
            }
            return operationResult;
        }
    }
}
