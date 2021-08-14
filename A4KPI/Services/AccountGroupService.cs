using AutoMapper;
using A4KPI.Data;
using A4KPI.DTO;
using A4KPI.Models;
using A4KPI.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using A4KPI.Helpers;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace A4KPI.Services
{
    public interface IAccountGroupService: IServiceBase<AccountGroup, AccountGroupDto>
    {
        Task<List<AccountGroupDto>> GetAccountGroupForTodolistByAccountId();

    }
    public class AccountGroupService : ServiceBase<AccountGroup, AccountGroupDto>, IAccountGroupService
    {
        private readonly IRepositoryBase<AccountGroup> _repo;
        private readonly IRepositoryBase<AccountGroupAccount> _repoAccount;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        public AccountGroupService(
            IRepositoryBase<AccountGroup> repo, 
            IRepositoryBase<AccountGroupAccount> repoAccount,
            IUnitOfWork unitOfWork,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper, 
            MapperConfiguration configMapper
            )
            : base(repo, unitOfWork, mapper,  configMapper)
        {
            _repo = repo;
            _repoAccount = repoAccount;
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _configMapper = configMapper;
        }

        public async Task<List<AccountGroupDto>> GetAccountGroupForTodolistByAccountId()
        {
            var currentMonth = DateTime.Today.Month;
            var accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            int accountId = JWTExtensions.GetDecodeTokenById(accessToken);
           
            // tim oc cua usser login
            return await _repoAccount.FindAll(x => x.AccountId == accountId).Select(x=>x.AccountGroup).ProjectTo<AccountGroupDto>(_configMapper).ToListAsync();
        }
    }
}
