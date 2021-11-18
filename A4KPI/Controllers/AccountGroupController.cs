﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using A4KPI.DTO;
using A4KPI.Helpers;
using A4KPI._Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using A4KPI.Constants;

namespace A4KPI.Controllers
{
    public class AccountGroupController : ApiControllerBase
    {
        private readonly IAccountGroupService _service;

        public AccountGroupController(IAccountGroupService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<ActionResult> GetAccountGroupForTodolistByAccountId()
        {
            return Ok((await _service.GetAccountGroupForTodolistByAccountId()).OrderBy(x => x.Sequence));
        }
        [HttpGet]
        public async Task<ActionResult> GetAllAsync()
        {
            return Ok((await _service.GetAllAsync()).OrderBy(x=>x.Sequence));
        }

        

    }
}
