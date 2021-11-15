using AutoMapper;
using A4KPI.Data;
using A4KPI.DTO;
using A4KPI.Models;
using A4KPI._Services.Services;
using System.Threading.Tasks;
using System.Collections.Generic;
using A4KPI.Helpers;
using A4KPI._Repositories.Interface;

namespace A4KPI._Services.Services
{
    
    public class AccountTypeService : IAccountTypeService
    {
        private readonly IAccountTypeRepository _repo;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;

        public AccountTypeService(
            IAccountTypeRepository repo,
            IMapper mapper,
            MapperConfiguration configMapper
            )
        {
            _repo = repo;
            _mapper = mapper;
            _configMapper = configMapper;
        }

        public Task<OperationResult> AddAsync(AccountTypeDto model)
        {
            throw new System.NotImplementedException();
        }

        public Task<OperationResult> AddRangeAsync(List<AccountTypeDto> model)
        {
            throw new System.NotImplementedException();
        }

        public Task<OperationResult> DeleteAsync(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<AccountTypeDto>> GetAllAsync()
        {
            throw new System.NotImplementedException();
        }

        public AccountTypeDto GetById(object id)
        {
            throw new System.NotImplementedException();
        }

        public Task<AccountTypeDto> GetByIdAsync(object id)
        {
            throw new System.NotImplementedException();
        }

        public Task<PagedList<AccountTypeDto>> GetWithPaginationsAsync(PaginationParams param)
        {
            throw new System.NotImplementedException();
        }

        public Task<PagedList<AccountTypeDto>> SearchAsync(PaginationParams param, object text)
        {
            throw new System.NotImplementedException();
        }

        public Task<OperationResult> UpdateAsync(AccountTypeDto model)
        {
            throw new System.NotImplementedException();
        }

        public Task<OperationResult> UpdateRangeAsync(List<AccountTypeDto> model)
        {
            throw new System.NotImplementedException();
        }
    }
}
