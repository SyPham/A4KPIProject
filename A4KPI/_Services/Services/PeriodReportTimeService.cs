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
    
    public class PeriodReportTimeService :  IPeriodReportTimeService
    {
        private readonly IPolicyRepository _repo;
        public PeriodReportTimeService(
            IPolicyRepository repo, 
            IMapper mapper, 
            MapperConfiguration configMapper
            )
        {
            _repo = repo;
        }

        public Task<OperationResult> AddAsync(PeriodReportTimeDto model)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult> AddRangeAsync(List<PeriodReportTimeDto> model)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<PeriodReportTimeDto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public PeriodReportTimeDto GetById(object id)
        {
            throw new NotImplementedException();
        }

        public Task<PeriodReportTimeDto> GetByIdAsync(object id)
        {
            throw new NotImplementedException();
        }

        public Task<PagedList<PeriodReportTimeDto>> GetWithPaginationsAsync(PaginationParams param)
        {
            throw new NotImplementedException();
        }

        public Task<PagedList<PeriodReportTimeDto>> SearchAsync(PaginationParams param, object text)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult> UpdateAsync(PeriodReportTimeDto model)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult> UpdateRangeAsync(List<PeriodReportTimeDto> model)
        {
            throw new NotImplementedException();
        }
    }
}
