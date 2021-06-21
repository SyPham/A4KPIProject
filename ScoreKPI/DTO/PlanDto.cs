using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ScoreKPI.DTO
{

    public class PlanDto
    {

        public int Id { get; set; }
   
        public string Topic { get; set; }
        public bool Status { get; set; }
        public int AccountId { get; set; }
        public int CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
       
    }
}
