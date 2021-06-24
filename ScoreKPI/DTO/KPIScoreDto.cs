using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScoreKPI.DTO
{
    public class KPIScoreDto
    {
        public int Id { get; set; }
        public int Period { get; set; }
        public double Point { get; set; }
        public string PeriodTypeCode { get; set; }
        public int PeriodTypeId { get; set; }
        public int AccountId { get; set; }
        public int ScoreBy { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }
}
