using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace A4KPI.Models
{
    [Table("OCPolicy")]
    public class OCPolicy
    {
        [Key]
        public int Id { get; set; }
        public int OcId { get; set; }
        public int PolicyId { get; set; }
        public string OcName { get; set; }

    }
}
