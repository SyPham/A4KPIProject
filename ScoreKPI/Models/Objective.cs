using ScoreKPI.Models.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ScoreKPI.Models
{
  
    [Table("Objectives")]
    public class Objective : IDateTracking
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(1000)]
        public string Topic { get; set; }
        public bool Status { get; set; }
        public int CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime Date { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
        public virtual ICollection<ResultOfMonth> ResultOfMonth { get; set; }
        public virtual ICollection<ToDoList> ToDoList { get; set; }

        public virtual ICollection<PIC> PICs { get; set; }
        public virtual ICollection<KPIScore> KPIScores { get; set; }
        public virtual ICollection<AttitudeScore> AttitudeScores { get; set; }

    }
}
