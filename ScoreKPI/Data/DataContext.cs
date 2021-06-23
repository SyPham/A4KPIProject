using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ScoreKPI.Models;
using ScoreKPI.Models.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ScoreKPI.Data
{
    public class DataContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountType> AccountTypes { get; set; }
        public DbSet<Mailing> Mailings { get; set; }
        public DbSet<Progress> Progresses { get; set; }
        public DbSet<AccountGroup> AccountGroups { get; set; }
        public DbSet<PeriodReportTime> PeriodReportTimes { get; set; }
        public DbSet<Period> Periods { get; set; }
        public DbSet<PeriodType> PeriodType { get; set; }
        public DbSet<ToDoList> ToDoList { get; set; }
        public DbSet<Plan> Plans { get; set; }
        public DbSet<Objective> Objectives { get; set; }
        public DbSet<PIC> PIC { get; set; }
        public DbSet<Attitude> Attitudes { get; set; }
        public DbSet<KPI> KPIs { get; set; }
        public DbSet<ResultOfMonth> ResultOfMonth { get; set; }
        public DbSet<AccountGroupPeriod> AccountGroupPeriods { get; set; }
        public DbSet<KPIScore> KPIScore { get; set; }
        public DbSet<AttitudeScore> AttitudeScore { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Contribution> Contributions { get; set; }
        public DbSet<OC> OCs { get; set; }
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder) {  }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            IEnumerable<EntityEntry> modified = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Modified || e.State == EntityState.Added);
            foreach (EntityEntry item in modified)
            {
                if (item.Entity is IDateTracking changedOrAddedItem)
                {
                    if (item.State == EntityState.Added)
                    {
                        changedOrAddedItem.CreatedTime = DateTime.Now;
                    }
                    else
                    {
                        changedOrAddedItem.ModifiedTime = DateTime.Now;
                    }
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    => optionsBuilder.LogTo(Console.WriteLine);
    }
}
