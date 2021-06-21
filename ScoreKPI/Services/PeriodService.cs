using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using ScoreKPI.Data;
using ScoreKPI.DTO;
using ScoreKPI.Models;
using ScoreKPI.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScoreKPI.Services
{
    public interface IPeriodService: IServiceBase<Period, PeriodDto>
    {
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
    }
}
