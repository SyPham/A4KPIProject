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
    public class PeriodReportTimeController : ApiControllerBase
    {
        private readonly IPeriodReportTimeService _service;

        public PeriodReportTimeController(IPeriodReportTimeService service)
        {
            _service = service;
        }

        

    }
}
