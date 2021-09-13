using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace A4KPI.DTO
{

    public class OCPolicyDto
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public List<int> OcIdList { get; set; }
        public List<int> Factory { get; set; }

        public string FactoryName { get; set; }
    }
}
