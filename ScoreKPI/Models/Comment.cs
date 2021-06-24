using ScoreKPI.Models.Interface;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ScoreKPI.Models
{
    [Table("Comments")]
    public class Comment : IDateTracking
    {
        [Key]
        public int Id { get; set; }
        public string Content { get; set; }
        public int CreatedBy { get; set; }
        public int AccountId { get; set; }
        public int PeriodTypeId { get; set; }
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
