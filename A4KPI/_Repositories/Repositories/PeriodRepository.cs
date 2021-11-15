﻿
using AutoMapper;
using A4KPI.Data;
using A4KPI.DTO;
using A4KPI.Models;
using A4KPI._Repositories.Interface;
using A4KPI._Repositories.Repositories;

namespace A4KPI._Repositories.Interface
{

    public class PeriodRepository : RepositoryBase<Period>, IPeriodRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public PeriodRepository(DataContext context, IMapper mapper) : base(context)
        {
            _context = context;
            _mapper = mapper;
        }
    }

}
