﻿using AutoMapper;
using A4KPI.Data;
using A4KPI.DTO;
using A4KPI.Models;
using A4KPI._Services.Services;
using A4KPI._Repositories.Interface;

namespace A4KPI._Services.Services
{
    public interface IAccountTypeService: IServiceBase<AccountType, AccountTypeDto>
    {
    }
    
}
