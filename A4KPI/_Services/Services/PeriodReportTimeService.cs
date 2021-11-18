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

        
    }
}
