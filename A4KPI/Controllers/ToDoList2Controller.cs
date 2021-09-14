using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetUtility;
using OfficeOpenXml;
using A4KPI.DTO;
using A4KPI.Helpers;
using A4KPI.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using A4KPI.Models;

namespace A4KPI.Controllers
{
    public class ToDoList2Controller : ApiControllerBase
    {
        private readonly IToDoList2Service _service;

        public ToDoList2Controller(IToDoList2Service service)
        {
            _service = service;
          
        }

        [HttpPost]
        public async Task<ActionResult> SubmitAction(ActionRequestDto action)
        {

            return Ok(await _service.SubmitAction(action));
        }
        [HttpGet]
        public async Task<ActionResult> L0(DateTime currentTime)
        {
         
            return Ok(await _service.L0(currentTime));
        }
        [HttpGet]
        public async Task<ActionResult> GetActionsForL0(int kpiNewId)
        {

            return Ok(await _service.GetActionsForL0(kpiNewId));
        }
    }
}
