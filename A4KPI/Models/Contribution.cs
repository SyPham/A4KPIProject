using A4KPI.Models.Interface;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace A4KPI.Models
{
    [Table("Contributions")]
    public class Contribution : IDateTracking
    {
        [Key]
        public int Id { get; set; }
        public string Content { get; set; }
        public int CreatedBy { get; set; }
        public int AccountId { get; set; }
        public int PeriodTypeId { get; set; }
        [MaxLength(50)]
        public string ScoreType { get; set; }

        public int Period { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
        [ForeignKey(nameof(AccountId))]
        public virtual Account Account { get; set; }
        [ForeignKey(nameof(PeriodTypeId))]
        public virtual PeriodType PeriodType { get; set; }
    }
}
