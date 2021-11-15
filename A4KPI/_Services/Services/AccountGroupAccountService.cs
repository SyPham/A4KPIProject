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

namespace A4KPI._Services.Services
{
    public class AccountGroupAccountService : IAccountGroupAccountService
    {
        private readonly IAccountGroupRepository _repo;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        public AccountGroupAccountService(
            IAccountGroupRepository repo, 
            IMapper mapper, 
            MapperConfiguration configMapper
            )
        {
            _repo = repo;
            _mapper = mapper;
            _configMapper = configMapper;
        }

        public Task<OperationResult> AddAsync(AccountGroupAccountDto model)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult> AddRangeAsync(List<AccountGroupAccountDto> model)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<AccountGroupAccountDto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public AccountGroupAccountDto GetById(object id)
        {
            throw new NotImplementedException();
        }

        public Task<AccountGroupAccountDto> GetByIdAsync(object id)
        {
            throw new NotImplementedException();
        }

        public Task<PagedList<AccountGroupAccountDto>> GetWithPaginationsAsync(PaginationParams param)
        {
            throw new NotImplementedException();
        }

        public Task<PagedList<AccountGroupAccountDto>> SearchAsync(PaginationParams param, object text)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult> UpdateAsync(AccountGroupAccountDto model)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult> UpdateRangeAsync(List<AccountGroupAccountDto> model)
        {
            throw new NotImplementedException();
        }
    }
}
