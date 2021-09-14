using A4KPI.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A4KPI.Models
{
    public class ActionRequestDto
    {
        public List<ActionDto> Actions { get; set; }
        public TargetDto Target { get; set; }
        public TargetYTDDto TargetYTD { get; set; }
    }
    public class PDCARequestDto
    {
        public List<ActionDto> Actions { get; set; }
        public List<TargetDto> Targets { get; set; }
    }
}
