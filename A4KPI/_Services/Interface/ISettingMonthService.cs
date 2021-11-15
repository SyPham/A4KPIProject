﻿using AutoMapper;
using A4KPI.Data;
using A4KPI.DTO;
using A4KPI.Models;
using A4KPI._Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using A4KPI._Repositories.Interface;

namespace A4KPI._Services.Services
{
    public interface ISettingMonthService : IServiceBase<SettingMonthly, SettingMonthlyDto>
    {
        Task<OperationResult> AddCustom(SettingMonthlyDto model);
    }
    
}
