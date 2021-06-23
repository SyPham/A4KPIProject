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
    [Table("Periods")]
    public class Period : IDateTracking
    {
        public Period(int periodTypeId, int value, string title, string months, DateTime reportTime)
        {
            PeriodTypeId = periodTypeId;
            Months = months;
            Value = value;
            ReportTime = reportTime;
            Title = title;
        }
        public Period(int periodTypeId, int month, string title, DateTime reportTime)
        {
            PeriodTypeId = periodTypeId;
            Value = month;
            ReportTime = reportTime;
            Title = title;

        }

        [Key]
        public int Id { get; set; }
        public int PeriodTypeId { get; set; }
        public string Months { get; set; }
        public int Value { get; set; }
        public string Title { get; set; }
        public DateTime ReportTime { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }

        [ForeignKey(nameof(PeriodTypeId))]
        public virtual PeriodType PeriodType { get; set; }
    }
}
