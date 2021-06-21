
using ScoreKPI.Models.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ScoreKPI.DTO
{
    public class MailingDto
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string Frequency { get; set; }
        public string Report { get; set; }
        public int AccountId { get; set; }
        public DateTime TimeSend { get; set; }
        public int CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime CreatedTime { get ; set ; }
        public DateTime? ModifiedTime { get ; set ; }
    }
}
