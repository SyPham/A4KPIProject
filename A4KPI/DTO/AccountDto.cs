using A4KPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A4KPI.DTO
{
    public class AccountDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public bool IsLock { get; set; }
        public int? AccountTypeId { get; set; }
        public int? Leader { get; set; }
        public string LeaderName { get; set; }
        public string FactName { get; set; }
        public string CenterName { get; set; }
        public string DeptName { get; set; }
        public int? Manager { get; set; }
        public int? FactId { get; set; }
        public int? CenterId { get; set; }
        public int? DeptId { get; set; }
        public string ManagerName { get; set; }
        public int CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
        public List<int> AccountGroupIds { get; set; }
        public string AccountGroupText { get; set; }
    }
}
