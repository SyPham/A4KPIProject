using AutoMapper;
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
   
    public class TargetYTDService :  ITargetYTDService
    {
        private readonly ITargetYTDRepository _repo;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        public TargetYTDService(
            ITargetYTDRepository repo, 
            IMapper mapper, 
            MapperConfiguration configMapper
            )
        {
            _repo = repo;
            _mapper = mapper;
            _configMapper = configMapper;
        }

        public Task<OperationResult> AddAsync(TargetYTDDto model)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult> AddRangeAsync(List<TargetYTDDto> model)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<TargetYTDDto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public TargetYTDDto GetById(object id)
        {
            throw new NotImplementedException();
        }

        public Task<TargetYTDDto> GetByIdAsync(object id)
        {
            throw new NotImplementedException();
        }

        public Task<PagedList<TargetYTDDto>> GetWithPaginationsAsync(PaginationParams param)
        {
            throw new NotImplementedException();
        }

        public Task<PagedList<TargetYTDDto>> SearchAsync(PaginationParams param, object text)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult> UpdateAsync(TargetYTDDto model)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult> UpdateRangeAsync(List<TargetYTDDto> model)
        {
            throw new NotImplementedException();
        }
    }
}
