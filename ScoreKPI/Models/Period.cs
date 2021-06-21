using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ScoreKPI.Models
{
    [Table("Periods")]
    public class Period
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }
        public virtual ICollection<PeriodReportTime> PeriodReportTimes { get; set; }

    }
}
