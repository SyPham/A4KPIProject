using A4KPI.Models;
using A4KPI.Models.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace A4KPI.DTO
{
    public class AccountGroupPeriodDto
    {
        public int Id { get; set; }
        public int AccountGroupId { get; set; }
  
        public int PeriodId { get; set; }
        public Period Period { get; set; }
        public AccountGroup AccountGroup { get; set; }


    }
}
