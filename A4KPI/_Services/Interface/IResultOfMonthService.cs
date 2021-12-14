﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
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

namespace A4KPI._Services.Interface
{
    public interface IResultOfMonthService
    {
        Task<List<ResultOfMonthDto>> GetAllByMonth(int objectiveId, DateTime currentTime);
        Task<OperationResult> UpdateResultOfMonthAsync(ResultOfMonthRequestDto model);

    }
    
}
