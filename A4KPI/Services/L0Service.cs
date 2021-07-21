using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A4KPI.Services
{
    public interface IL0Service
    {
        /// <summary>
        /// Loaddata todolist
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="currentTime"></param>
        /// <returns></returns>
        Task<object> L0(int accountId, DateTime currentTime);

        /// <summary>
        /// Loaddata KPIScore Modal
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="currentTime"></param>
        /// <returns></returns>
        Task<object> GetAllKPIScoreL0ByPeriod(int period);
    }
    public class L0Service
    {
    }
}
