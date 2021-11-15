using AutoMapper;
using A4KPI.Data;
using A4KPI.DTO;
using A4KPI.Models;
using A4KPI._Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using A4KPI.Helpers;
using A4KPI.Constants;
using Microsoft.AspNetCore.Http;
using NetUtility;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using A4KPI._Repositories.Interface;

namespace A4KPI._Services.Services
{
    public interface IKPINewService: IServiceBase<KPINew, KPINewDto>
    {
        Task<object> GetKPIByOcID(int ocID);
        Task<object> GetListPic();
        Task<object> GetPolicyByOcID(int ocID);
        Task<object> GetAllType();
        Task<bool> Delete(int id);
        Task<IEnumerable<HierarchyNode<KPINewDto>>> GetAllAsTreeView();
        Task<IEnumerable<HierarchyNode<KPINewDto>>> GetAllAsTreeView2nd3rd();
    }
    
}
