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
    public interface IOCPolicyService 
    {
        Task<IEnumerable<HierarchyNode<OCDto>>> GetAllAsTreeView();
        Task<List<OCAccountDto>> GetUserByOcID(int ocID);
        Task<object> MappingPolicyOC(OCPolicyDto Dto);
        Task<object> RemovePolicyOC(OCPolicyDto Dto);
        Task<object> MappingRangeUserOC(OCAccountDto model);
        Task<object> GetAllLevel3();
        Task<object> GetAllPolicy();
        Task<bool> DeletePolicy(int id);
        Task<object> RemoveUserOC(OCAccountDto OCAccountDto);
      
    }
    
}
