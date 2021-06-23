using ScoreKPI.Models.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ScoreKPI.Models
{
    [Table("AttitudeScore")]
    public class AttitudeScore: IDateTracking
    {
        [Key]
        public int Id { get; set; }
        public int Period { get; set; }
        public double Point { get; set; }
        public int PeriodTypeId { get; set; }
        public int AccountId { get; set; }
        public int ScoreBy { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
        [ForeignKey(nameof(PeriodTypeId))]
        public virtual Period PeriodType { get; set; }
        [ForeignKey(nameof(AccountId))]
        public virtual Account Account { get; set; }

        [ForeignKey(nameof(ScoreBy))]
        public virtual Account AccountScored { get; set; }
    }
}
