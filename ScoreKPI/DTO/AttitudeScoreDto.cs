using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScoreKPI.DTO
{
    public class AttitudeScoreDto
    {
        public int Id { get; set; }
        public string PeriodType { get; set; }
        public int Period { get; set; }
        public double Point { get; set; }
        public int ObjectiveId { get; set; }
        public int ScoreBy { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }

    }
}
