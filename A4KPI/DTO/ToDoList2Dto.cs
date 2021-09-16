using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A4KPI.DTO
{
    public class ToDoList2Dto
    {

    }
    public class UpdatePDCADto
    {
        public int ActionId { get; set; }
        public int DoId { get; set; }
        public string Content { get; set; }
        public string Target { get; set; }
        public string Deadline { get; set; }
        public int? StatusId { get; set; }
        public string DoContent { get; set; }
        public string Achievement { get; set; }

    }
    
}
