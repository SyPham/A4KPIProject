using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ScoreKPI.DTO
{
    public class AttitudeDto
    {
        public int Id { get; set; }
        public double Point { get; set; }
   
    }
}
