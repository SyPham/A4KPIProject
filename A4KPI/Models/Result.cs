using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace A4KPI.Models
{
    [Table("Results")]
    public class Result
    {
        [Key]
        public int Id { get; set; }
        public string Content { get; set; }

        public int KPIId { get; set; }
        [ForeignKey(nameof(KPIId))]
        public virtual KPINew KPINew { get; set; }
    }
}
