using ScoreKPI.Models.Interface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ScoreKPI.Models
{
    [Table("Accounts")]
    public class Account: IDateTracking
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(255)]
        public string Username { get; set; }
        [MaxLength(255)]
        public string FullName { get; set; }
        [MaxLength(255)]
        public string Password { get; set; }
        [MaxLength(255)]
        public string Email { get; set; }
        public string RoleOC { get; set; }
        public bool StatusOC { get; set; }
        public bool IsLock { get; set; }
        public int? AccountTypeId { get; set; }
        public int? AccountGroupId { get; set; }
        public int CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime CreatedTime { get ; set ; }
        public DateTime? ModifiedTime { get ; set ; }
        [ForeignKey(nameof(AccountTypeId))]
        public virtual AccountType AccountType { get; set; }
        [ForeignKey(nameof(AccountGroupId))]
        public virtual AccountGroup AccountGroup { get; set; }
        public virtual ICollection<Objective> Objectives { get; set; }
    }
}
