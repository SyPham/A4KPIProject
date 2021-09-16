using A4KPI.Models.Interface;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace A4KPI.Models
{
    [Table("Results")]
    public class Result: IDateTracking
    {
        [Key]
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime UpdateTime { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }

        public int KPIId { get; set; }
        [ForeignKey(nameof(KPIId))]
        public virtual KPINew KPINew { get; set; }
    }
}
