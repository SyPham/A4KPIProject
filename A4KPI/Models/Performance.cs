using A4KPI.Models.Interface;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace A4KPI.Models
{
    [Table("Performances")]
    public class Performance: IDateTracking
    {
        [Key]
        public int Id { get; set; }
        public int ObjectiveId { get; set; }
        public int Month { get; set; }
        [StringLength(200)]
        public string Percentage { get; set; }
        public int UploadBy { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }

        [ForeignKey(nameof(UploadBy))]
        public virtual Account Account { get; set; }

        [ForeignKey(nameof(ObjectiveId))]
        public virtual Objective Objective { get; set; }
    }
}
