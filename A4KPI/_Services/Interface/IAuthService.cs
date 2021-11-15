using Microsoft.EntityFrameworkCore;
using A4KPI.Data;
using A4KPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using A4KPI.Helpers;

namespace A4KPI._Services.Services
{
    public interface IAuthService
    {
        Task<Account> Login(string username, string password);
        Task<bool> CheckLock(string username);
    }
    
}
