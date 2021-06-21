using ScoreKPI.Models.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ScoreKPI.Models
{
    [Table("KPIScore")]
    public class KPIScore: IDateTracking
    {
        [Key]

        public int Id { get; set; }
        [MaxLength(50)]
        public string PeriodType { get; set; }
        public int Period { get; set; }
        public double Point { get; set; }
        public int ObjectiveId { get; set; }
        public int ScoreBy { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
        [ForeignKey(nameof(ObjectiveId))]
        public virtual Objective Objective { get; set; }


    }
}
