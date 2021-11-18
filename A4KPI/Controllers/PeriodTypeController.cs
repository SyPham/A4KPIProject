using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using A4KPI.DTO;
using A4KPI.Helpers;
using A4KPI._Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A4KPI.Controllers
{
    public class PeriodTypeController : ApiControllerBase
    {
        private readonly IPeriodTypeService _service;

        public PeriodTypeController(IPeriodTypeService service)
        {
            _service = service;
        }

       

    }
}
