using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace A4KPI.Models
{
   

    [Table("KPINew")]
    public class KPINew
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int OcId { get; set; }
        public int PolicyId { get; set; }
        public int TypeId { get; set; }
        public int Pic { get; set; }
        public int UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }

    }
}
