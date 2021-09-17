using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A4KPI.DTO
{
    public class UploadFileDto
    {
    }
    public class PostRequest
    {
        public int KpiId { get; set; }
        public DateTime CurrentTime { get; set; }
    }
}
