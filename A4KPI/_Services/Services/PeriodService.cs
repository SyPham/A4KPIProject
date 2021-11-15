﻿using AutoMapper;
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
   
    public class PeriodService : IPeriodService
    {
        private readonly IPeriodRepository _repo;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        public PeriodService(
            IPeriodRepository repo, 
            IMapper mapper, 
            MapperConfiguration configMapper
            )
        {
            _repo = repo;
            _mapper = mapper;
            _configMapper = configMapper;
        }

        public Task<OperationResult> AddAsync(PeriodDto model)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult> AddRangeAsync(List<PeriodDto> model)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public  async Task<List<PeriodDto>> GetAllAsync()
        {
            return await _repo.FindAll().ProjectTo<PeriodDto>(_configMapper).ToListAsync();

        }
        public  async Task<List<PeriodDto>> GetAllByPeriodTypeIdAsync(int periodTypeId)
        {
            return await _repo.FindAll(x=>x.PeriodTypeId == periodTypeId).OrderBy(x=>x.Value).ProjectTo<PeriodDto>(_configMapper).ToListAsync();

        }

        public PeriodDto GetById(object id)
        {
            throw new NotImplementedException();
        }

        public Task<PeriodDto> GetByIdAsync(object id)
        {
            throw new NotImplementedException();
        }

        public Task<PagedList<PeriodDto>> GetWithPaginationsAsync(PaginationParams param)
        {
            throw new NotImplementedException();
        }

        public Task<PagedList<PeriodDto>> SearchAsync(PaginationParams param, object text)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult> UpdateAsync(PeriodDto model)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult> UpdateRangeAsync(List<PeriodDto> model)
        {
            throw new NotImplementedException();
        }
    }
}
