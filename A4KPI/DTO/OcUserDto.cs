using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace A4KPI.DTO
{
    public class OcUserDto
    {
        public int UserID { get; set; }
        public int OCID { get; set; }
        public string OCname { get; set; }
        public List<int> AccountIdList { get; set; }
    }
}
