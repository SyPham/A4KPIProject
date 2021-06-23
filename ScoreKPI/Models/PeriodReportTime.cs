using ScoreKPI.Models.Interface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ScoreKPI.Models
{
    [Table("PeriodReportTimes")]
    public class PeriodReportTime: IDateTracking
    {
       [Key]
        public int Id { get; set; }
        public int PeriodId { get; set; }
        public string Months { get; set; }
        public int PeriodOfYear { get; set; }
        public DateTime ReportTime { get; set; }
        public int CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime CreatedTime { get ; set ; }
        public DateTime? ModifiedTime { get ; set ; }

        [ForeignKey(nameof(PeriodId))]
        public virtual Period Period { get; set; }
    }
}
