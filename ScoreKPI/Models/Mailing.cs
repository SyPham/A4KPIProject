using ScoreKPI.Models.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ScoreKPI.Models
{
    [Table("Mailings")]
    public class Mailing: IDateTracking
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(1000)]
        public string Url { get; set; }
        [MaxLength(255)]
        public string Frequency { get; set; }
        [MaxLength(255)]
        public string Report { get; set; }
        public int AccountId { get; set; }
        public DateTime TimeSend { get; set; }
        public int CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime CreatedTime { get ; set ; }
        public DateTime? ModifiedTime { get ; set ; }
        [ForeignKey(nameof(AccountId))]
        public virtual Account Account { get; set; }
    }
}
