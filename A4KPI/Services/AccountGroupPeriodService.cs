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
    public interface IAccountGroupPeriodService: IServiceBase<AccountGroupPeriod, AccountGroupPeriodDto>
    {
    }
    public class AccountGroupPeriodService : ServiceBase<AccountGroupPeriod, AccountGroupPeriodDto>, IAccountGroupPeriodService
    {
        private readonly IRepositoryBase<AccountGroupPeriod> _repo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        public AccountGroupPeriodService(
            IRepositoryBase<AccountGroupPeriod> repo, 
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
        public override async Task<List<AccountGroupPeriodDto>> GetAllAsync()
        {
            return await _repo.FindAll()
                .Include(x=> x.Period)
                .Include(x=> x.AccountGroup)
                .ProjectTo<AccountGroupPeriodDto>(_configMapper).ToListAsync();

        }
    }
}
