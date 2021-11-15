using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NetUtility;
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
    public interface IToDoListService 
    {
        Task<object> GetAllInCurrentQuarterByObjectiveIdAsync(int objectiveId);
        /// <summary>
        /// Lấy objective list PICS
        /// Nếu quyền là L1, L2, FHO, GHR, GM thì sẽ để trống
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        Task<object> L0(int accountId, DateTime currentTime);
        /// <summary>
        /// Chấm điểm KPI và điểm thái độ của những người mình đã giao nhiệm vụ
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        Task<object> L1(int accountId, DateTime currentTime);
        Task<object> FunctionalLeader(int accountId, DateTime currentTime);

        /// <summary>
        /// Lấy tất cả cấp dưới của mình để chấm điểm
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        Task<object> L2(int accountId, DateTime currentTime);
        /// <summary>
        /// Chấm điểm tất cả
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        Task<object> GHR(int accountId, DateTime currentTime);
        /// <summary>
        /// Lấy tất cả user để chấm điểm( gán ở bảng oc user)
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        Task<object> FHO(int accountId);

        Task<object> Updater(int accountId, DateTime currentTime);

        /// <summary>
        /// Lấy tất cả user để chấm điểm( gán ở bảng oc user)
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        Task<object> GM(int accountId, DateTime currentTime);

        Task<object> GetAllByObjectiveIdAsync(int objectiveId);
        Task<List<ToDoListByLevelL1L2Dto>> GetAllKPIScoreByAccountId(int accountId);
        Task<object> GetAllKPIScoreL1ByAccountId(int accountId, DateTime currentTime);
        Task<object> GetAllKPIScoreL2ByAccountId(int accountId, DateTime currentTime);
        Task<object> GetAllKPIScoreGHRByAccountId(int accountId, DateTime currentTime);
        Task<object> GetAllAttitudeScoreL1ByAccountId(int accountId);
        Task<object> GetAllAttitudeScoreL2ByAccountId(int accountId);
        Task<object> GetAllAttitudeScoreGHRByAccountId(int accountId);
        Task<object> GetAllAttitudeScoreGFLByAccountId(int accountId);
        Task<object> GetAllKPISelfScoreByObjectiveId(int objectiveId, int accountId);
        Task<object> GetAllObjectiveByL1L2();
        Task<object> GetQuarterlySetting();
        Task<object> GetAllByObjectiveIdAsTreeAsync(int objectiveId);

        Task<object> GetAllKPIScoreL0ByPeriod(int period);
        Task<object> GetAllAttitudeScoreByFunctionalLeader();
        Task<OperationResult> Reject(List<int> ids);
        Task<OperationResult> DisableReject(List<int> ids);
        Task<OperationResult> Release(List<int> ids);

        Task<OperationResult> ReportQ1orQ3(Q1OrQ3Request request);

    }
    
}
