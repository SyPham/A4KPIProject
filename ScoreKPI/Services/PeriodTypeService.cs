using AutoMapper;
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
    public interface IPeriodTypeService: IServiceBase<PeriodType, PeriodTypeDto>
    {
    }
    public class PeriodTypeService : ServiceBase<PeriodType, PeriodTypeDto>, IPeriodTypeService
    {
        private readonly IRepositoryBase<PeriodType> _repo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        public PeriodTypeService(
            IRepositoryBase<PeriodType> repo, 
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
    }
}
