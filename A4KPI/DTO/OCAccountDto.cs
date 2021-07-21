using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace A4KPI.DTO
{
    public class OCAccountDto
    {
        public int AccountId { get; set; }
        public int OCId { get; set; }
        public string OCName { get; set; }
        public string FullName { get; set; }
        public List<int> AccountIdList { get; set; }
    }
}
