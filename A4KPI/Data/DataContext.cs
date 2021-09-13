using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using A4KPI.Models;
using A4KPI.Models.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace A4KPI.Data
{
    public class DataContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Policy> Policies { get; set; }
        public DbSet<OCPolicy> OCPolicies { get; set; }
        public DbSet<KPINew> KPINews { get; set; }
        public DbSet<Types> Types { get; set; }
        public DbSet<OCNew> OCNews { get; set; }
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
        public DbSet<AccountGroupAccount> AccountGroupAccount { get; set; }
        public DbSet<KPIScore> KPIScore { get; set; }
        public DbSet<AttitudeScore> AttitudeScore { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Contribution> Contributions { get; set; }
        public DbSet<OC> OCs { get; set; }
        public DbSet<OCAccount> OCAccounts { get; set; }
        public DbSet<SpecialScore> SpecialScore { get; set; }
        public DbSet<SmartScore> SmartScore { get; set; }
        public DbSet<Performance> Performances { get; set; }
        public DbSet<SpecialContributionScore> SpecialContributionScore { get; set; }
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder) {

            // Xóa user1 trong bảng account thì những accountId của user1 trong bảng Performance không bị xóa theo
            modelBuilder.Entity<Performance>()
                .HasOne(s => s.Account)
                .WithMany(ta => ta.Performances)
                .HasForeignKey(u => u.UploadBy)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Performance>()
               .HasOne(s => s.Objective)
               .WithMany(ta => ta.Performances)
               .HasForeignKey(u => u.ObjectiveId)
               .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<OCAccount>()
            .HasOne(s => s.Account)
            .WithMany(ta => ta.OCAccounts)
            .HasForeignKey(u => u.AccountId)
            .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<OCAccount>()
               .HasOne(s => s.OC)
               .WithMany(ta => ta.OCAccounts)
               .HasForeignKey(u => u.OCId)
               .OnDelete(DeleteBehavior.NoAction);
        }
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
