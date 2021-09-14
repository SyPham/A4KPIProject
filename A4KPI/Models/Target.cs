using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace A4KPI.Models
{
    [Table("Targets")]
    public class Target
    {
        [Key]
        public int Id { get; set; }
        public double Value { get; set; } // this month target
        public double Performance { get; set; } // this month performance
        public int KPIId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime TargetTime { get; set; }

        public DateTime CreatedTime { get; set; }
        [ForeignKey(nameof(KPIId))]
        public virtual KPINew KPINew { get; set; }


    }
}
