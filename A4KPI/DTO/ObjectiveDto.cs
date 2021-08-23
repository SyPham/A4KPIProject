using A4KPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A4KPI.DTO
{
    public class ObjectiveDto
    {
        public int Id { get; set; }
        public string Topic { get; set; }
        public bool Status { get; set; }
       
        public int CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
        public List<int> AccountIdList { get; set; }
        public string Accounts { get; set; }

        public DateTime Date { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
      
        public  Account Creator { get; set; }
    }

    public class ObjectiveRequestDto
    {
        public int Id { get; set; }
        public string Topic { get; set; }
        public bool Status { get; set; }
        public DateTime Date { get; set; }
        public List<int> AccountIdList { get; set; }
    }
    public class ResultOfMonthRequestDto
    {
        public int Id { get; set; }
        public int ObjectiveId { get; set; }
        public int CreatedBy { get; set; }
        public int Period { get; set; }
        public string Title { get; set; }
    }
    public class RejectRequestDto
    {
        public List<int> Ids { get; set; }
    }
    public class ReleaseRequestDto
    {
        public List<int> Ids { get; set; }
    }
}
