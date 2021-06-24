using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ScoreKPI.Models
{
    [Table("OCUsers")]
    public class OCUser
    {
        [Key]
        public int ID { get; set; }
        public int UserID { get; set; }
        public int OCID { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
