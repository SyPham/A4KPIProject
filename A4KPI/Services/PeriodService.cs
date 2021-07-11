using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using A4KPI.Data;
using A4KPI.DTO;
using A4KPI.Models;
using A4KPI.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A4KPI.Services
{
    public interface IPeriodService: IServiceBase<Period, PeriodDto>
    {
        Task<List<PeriodDto>> GetAllByPeriodTypeIdAsync(int periodTypeId);
    }
    public class PeriodService : ServiceBase<Period, PeriodDto>, IPeriodService
    {
        private readonly IRepositoryBase<Period> _repo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        public PeriodService(
            IRepositoryBase<Period> repo, 
            IUnitOfWork unitOfWork,
            IMapper mapper, 
            MapperConfiguration configMapper
            )
            : base(repo, unitOfWork, mapper,  configMapper)
        {
            _repo = repo;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configMapper = configMapper;
        }
        public override async Task<List<PeriodDto>> GetAllAsync()
        {
            return await _repo.FindAll().ProjectTo<PeriodDto>(_configMapper).ToListAsync();

        }
        public  async Task<List<PeriodDto>> GetAllByPeriodTypeIdAsync(int periodTypeId)
        {
            return await _repo.FindAll(x=>x.PeriodTypeId == periodTypeId).OrderBy(x=>x.Value).ProjectTo<PeriodDto>(_configMapper).ToListAsync();

        }
    }
}
