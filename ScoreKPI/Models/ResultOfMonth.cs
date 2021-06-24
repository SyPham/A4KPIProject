using ScoreKPI.Models.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ScoreKPI.Models
{
    [Table("ResultOfMonth")]
    public class ResultOfMonth: IDateTracking
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public int Month { get; set; }
        public int ObjectiveId { get; set; }
        public int CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
        [ForeignKey(nameof(CreatedBy))]
        public virtual Account Account { get; set; }
        [ForeignKey(nameof(ObjectiveId))]
        public virtual Objective Objective { get; set; }
    }
}
