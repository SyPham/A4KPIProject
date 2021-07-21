using System;

namespace A4KPI.DTO
{
    public class PerformanceDto
    {
        public int Id { get; set; }
        public int ObjectiveId { get; set; }
        public string ObjectiveName { get; set; }
        public int Month { get; set; }
        public double Percentage { get; set; }
        public int UploadBy { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }
}
