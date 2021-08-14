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
using ESS_API.Helpers;
using AutoMapper.QueryableExtensions;

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
        private IEnumerable<OCDto> FindAllParents(List<OCDto> all_data, OCDto child)
        {
            var parent = all_data.FirstOrDefault(x => x.Id == child.ParentId);

            if (parent == null)
                return Enumerable.Empty<OCDto>();

            return new[] { parent }.Concat(FindAllParents(all_data, parent));
        }
       
        public async Task<object> GetKPIObjectivesByUpdater()
        {
            var currentMonth = DateTime.Today.Month;
            var accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            int accountId = JWTExtensions.GetDecodeTokenById(accessToken);
            var checkRole = await _repoAccountGroupAccount.FindAll(x => x.AccountId == accountId).Select(x => x.AccountGroup.Position).AnyAsync(x => SystemRole.Updater == x);
            if (checkRole == false) return new
            {
                Data = new List<PerformanceDto>(),
                IsAuthorize = false
            };
            // tim oc cua usser login
            var ocuser = await _repoOCAccount.FindAll(x => x.AccountId == accountId).Include(x => x.OC).FirstOrDefaultAsync();
            if (ocuser == null) return new
            {
                Data = new List<PerformanceDto>(),
                IsAuthorize = false
            };
            // Lay tat ca con cua oc
            if (ocuser.OC.ParentId == null) return new
            {
                Data = new List<PerformanceDto>(),
                IsAuthorize = false
            };

            var oc = _repoOC.FindAll().ProjectTo<OCDto>(_configMapper).ToList();
            var child = oc.FirstOrDefault(x => x.Id == ocuser.OCId);
            // Tìm tất cả cha của oc hiện tại
            var ocParentItemList = FindAllParents(oc, child);
            var ocItem = oc.AsHierarchy(x => x.Id, y => y.ParentId, ocuser.OCId).ToList();
            // Tìm tất cả con của oc hiện tại
            var ocChildItemList = ocItem.Flatten(x => x.ChildNodes).Select(x => x.Entity.Id).ToList();
            var ocs = ocParentItemList.Select(x => x.Id).Union(ocChildItemList).Distinct().ToList();
            // vao ocUser tim theo ocId list 
            var accountIds = await _repoOCAccount.FindAll(x => ocs.Contains(x.OCId))
                                                .Select(x => x.AccountId)
                                                .Distinct()
                                                .ToListAsync();
            if (accountIds.Any(x => x == accountId) == false) return new
            {
                Data = new List<PerformanceDto>(),
                IsAuthorize = false
            };

            // Neu ai co quyen factoryHeadr thi lay ra
            var factoryHead = await _repoAccountGroupAccount.FindAll(x => accountIds.Contains(x.AccountId) && x.AccountGroup.Position == SystemRole.FactoryHead).Select(x => x.AccountId).ToListAsync();

            var model = await (from a in _repoObjective.FindAll(x => factoryHead.Contains(x.CreatedBy))
                               join b in _repo.FindAll(x => x.Month == currentMonth) on a.Id equals b.ObjectiveId into ab
                               from c in ab.DefaultIfEmpty()
                               select new PerformanceDto
                               {
                                   Id = c != null ? c.Id : 0,
                                   ObjectiveId = a.Id,
                                   ObjectiveName = a.Topic,
                                   Month = currentMonth,
                                   CreatedTime = c != null ? c.CreatedTime : DateTime.MinValue,
                                   Percentage = c != null ? c.Percentage : 0
                               }).ToListAsync();
            return new
            {
                Data = model,
                IsAuthorize = true
            };
        }

        public async Task<OperationResult> Submit(List<PerformanceDto> performances)
        {
            try
            {
                var accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
                int accountId = JWTExtensions.GetDecodeTokenById(accessToken);
                var items = _mapper.Map<List<Performance>>(performances);
                items.ForEach(x =>
                {
                    x.UploadBy = accountId;
                });
                    _repo.UpdateRange(items.Where(x => x.Id > 0).ToList());
                    _repo.AddRange(items.Where(x=>x.Id == 0).ToList());
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
