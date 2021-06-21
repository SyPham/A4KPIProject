using ScoreKPI.Models.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ScoreKPI.Models
{
    [Table("AccountGroupPeriods")]
    public class AccountGroupPeriod 
    {
        [Key]
        public int Id { get; set; }
        public int AccountGroupId { get; set; }
        public int PeriodId { get; set; }
        [ForeignKey(nameof(PeriodId))]
        public virtual Period Period { get; set; }
        [ForeignKey(nameof(AccountGroupId))]
        public virtual AccountGroup AccountGroup { get; set; }
    }
}
