using System;

namespace A4KPI.DTO
{
    public class CommentDto
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int CreatedBy { get; set; }
        public int AccountId { get; set; }
        public string ScoreType { get; set; }

        public int PeriodTypeId { get; set; }
        public int Period { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }
    
}
