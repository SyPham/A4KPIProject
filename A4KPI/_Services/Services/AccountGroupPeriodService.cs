using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using A4KPI.Data;
using A4KPI.DTO;
using A4KPI.Models;
using A4KPI._Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using A4KPI._Repositories.Interface;
using A4KPI.Helpers;
using A4KPI._Services.Interface;

namespace A4KPI._Services.Services
{

    public class AccountGroupPeriodService : IAccountGroupPeriodService
    {
        private readonly IAccountGroupPeriodRepository _repo;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        public AccountGroupPeriodService(
            IAccountGroupPeriodRepository repo, 
            IMapper mapper, 
            MapperConfiguration configMapper
            )
        {
            _repo = repo;
            _mapper = mapper;
            _configMapper = configMapper;
        }

        

        public async Task<List<AccountGroupPeriodDto>> GetAllAsync()
        {
            return await _repo.FindAll().Include(x => x.Period).Include(x => x.AccountGroup).ProjectTo<AccountGroupPeriodDto>(_configMapper).ToListAsync();

        }

        
    }
}
