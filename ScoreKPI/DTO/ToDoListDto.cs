using ScoreKPI.Models;
using ScoreKPI.Models.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ScoreKPI.DTO
{
   
    public class ToDoListDto 
    {

        public int Id { get; set; }
        public string yourObjective { get; set; }
        public string Action { get; set; }
        public string Remark { get; set; }
        public int? ProgressId { get; set; }
        public int ObjectiveId { get; set; }
        public string AccountGroupType{ get; set; }
        public int CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }

    }

    public class ToDoListByLevelL1L2Dto
    {

        public int Id { get; set; }
        public string Objective { get; set; }
        public List<string> L0TargetList { get; set; }
        public List<string> L0ActionList { get; set; }
        public string Result1OfMonth { get; set; }
        public string Result2OfMonth { get; set; }
        public string Result3OfMonth { get; set; }
        public object Months { get; set; }
    }
    public class SelfScoreDto
    {

        public List<string> ObjectiveList { get; set; }
        public string ResultOfMonth { get; set; }
        public int Month { get; set; }
    }
    public class ImportExcelFHO
    {

        public string KPIObjective { get; set; }
        public  string UserList { get; set; }
    }
}
