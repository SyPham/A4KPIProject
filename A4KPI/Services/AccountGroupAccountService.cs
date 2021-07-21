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
    public interface IAccountGroupAccountService: IServiceBase<AccountGroupAccount, AccountGroupAccountDto>
    {
    }
    public class AccountGroupAccountService : ServiceBase<AccountGroupAccount, AccountGroupAccountDto>, IAccountGroupAccountService
    {
        private readonly IRepositoryBase<AccountGroupAccount> _repo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        public AccountGroupAccountService(
            IRepositoryBase<AccountGroupAccount> repo, 
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
