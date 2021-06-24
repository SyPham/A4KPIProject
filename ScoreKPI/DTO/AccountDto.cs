using ScoreKPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScoreKPI.DTO
{
    public class AccountDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string RoleOC { get; set; }
        public bool StatusOC { get; set; }
        public bool IsLock { get; set; }
        public int AccountTypeId { get; set; }
        public int CreatedBy { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
        public int? AccountGroupId { get; set; }
        public AccountGroup AccountGroup { get; set; }
    }
}
