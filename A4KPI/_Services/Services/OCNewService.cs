using AutoMapper;
using AutoMapper.QueryableExtensions;

using Microsoft.EntityFrameworkCore;
using A4KPI.Constants;
using A4KPI.Data;
using A4KPI.DTO;
using A4KPI.Helpers;
using A4KPI.Models;
using A4KPI._Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using A4KPI._Repositories.Interface;

namespace A4KPI._Services.Services
{
  
    public class OCNewService : IOCNewService
    {
        private OperationResult operationResult;

        private readonly IOCNewRepository _repo;
        private readonly IOCAccountRepository _repoOCAccount;
        private readonly IAccountRepository _repoAccount;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        public OCNewService(
            IOCNewRepository repo,
            IOCAccountRepository repoOCAccount,
            IAccountRepository repoAccount,
            IMapper mapper, 
            MapperConfiguration configMapper
            )
        {
            _repo = repo;
            _repoOCAccount = repoOCAccount;
            _repoAccount = repoAccount;
            _mapper = mapper;
            _configMapper = configMapper;
        }

       

        
    }
}
