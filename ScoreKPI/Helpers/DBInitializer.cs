using Microsoft.EntityFrameworkCore;
using ScoreKPI.Data;
using ScoreKPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScoreKPI.Helpers
{
    public static class DBInitializer
    {
        //private readonly DataContext _context;
        //public DBInitializer(DataContext context)
        //{
        //    _context = context;
        //}
        public static void Seed(DataContext _context)
        {
            #region Period Type
            if (!(_context.PeriodType.Any()))
            {
                _context.PeriodType.AddRange(new List<PeriodType> {
                    new PeriodType("Monthly", "Monthly", 1),
                     new PeriodType("Quarterly", "Quarterly", 2),
                      new PeriodType("HalfYear", "HalfYear", 3),
                });
                _context.SaveChanges();
            }
            #endregion

            #region Period 
            if (!(_context.Periods.Any()))
            {
                var monthly = _context.PeriodType.FirstOrDefault(x => x.Position == 1);
                var quarterly = _context.PeriodType.FirstOrDefault(x => x.Position == 2);
                var yearly = _context.PeriodType.FirstOrDefault(x => x.Position == 3);
                var dt = DateTime.Now;
                _context.Periods.AddRange(new List<Period> {
                    new Period(monthly.Id, 1, "Jan.", new DateTime(dt.Year,1, 28)),
                    new Period(monthly.Id, 2, "Feb.",new DateTime(dt.Year,2, 28)),
                    new Period(monthly.Id, 3, "Mar.",new DateTime(dt.Year,3, 28)),
                    new Period(monthly.Id, 4, "Apr.",new DateTime(dt.Year,4, 28)),
                    new Period(monthly.Id, 5, "May.",new DateTime(dt.Year,5, 28)),
                    new Period(monthly.Id, 6, "Jun.",new DateTime(dt.Year,6, 28)),
                    new Period(monthly.Id, 7, "Jul.",new DateTime(dt.Year,7, 28)),
                    new Period(monthly.Id, 8, "Aug.",new DateTime(dt.Year,8, 28)),
                    new Period(monthly.Id, 9, "Sep.",new DateTime(dt.Year,9, 28)),
                    new Period(monthly.Id, 10, "Oct.", new DateTime(dt.Year,10, 28)),
                    new Period(monthly.Id, 11, "Nov.", new DateTime(dt.Year,11, 28)),
                    new Period(monthly.Id, 12, "Dec.", new DateTime(dt.Year,12, 28)),
                    new Period(quarterly.Id, 1, "Q1", "2,3,4",  new DateTime(dt.Year ,5, 1)),
                    new Period(quarterly.Id, 2, "Q2", "5,6,7", new DateTime(dt.Year,8, 1)),
                    new Period(quarterly.Id, 3, "Q3", "8,9,10",  new DateTime(dt.Year,11, 11)),
                    new Period(quarterly.Id, 4, "Q4", "11,12,1",  new DateTime(dt.Year + 1,1, 1)),
                    new Period(yearly.Id, 1, "H1", "1,2,3,4,5,6", new DateTime(dt.Year,7, 1)),
                       new Period(yearly.Id, 2, "H2", "7,8,9,10,11,12",  new DateTime(dt.Year + 1,1, 1)),
                });
                _context.SaveChanges();
            }
            #endregion

            #region KPI Score
            if (!(_context.KPIs.Any()))
            {
                _context.KPIs.AddRange(new List<KPI> {
                    new KPI {Point = 0},
                    new KPI {Point = 0.5},
                      new KPI {Point = 1},
                    new KPI {Point = 1.5},
                      new KPI {Point = 2},
                    new KPI {Point = 2.5},
                      new KPI {Point = 3},
                    new KPI {Point = 3.5},
                     new KPI {Point = 4},
                    new KPI {Point = 4.5},
                     new KPI {Point = 5}
                });
                _context.SaveChanges();
            }

            #endregion

            #region Attitudes Score
            if (!(_context.Attitudes.Any()))
            {
                _context.Attitudes.AddRange(new List<Attitude> {
                     new Attitude {Point = 1},
                    new Attitude {Point = 2},
                      new Attitude {Point = 3},
                    new Attitude {Point = 4},
                      new Attitude {Point = 5},
                    new Attitude {Point = 6},
                      new Attitude {Point = 7},
                    new Attitude {Point = 8},
                     new Attitude {Point = 9},
                    new Attitude {Point = 10},
                });
                _context.SaveChanges();
            }

            #endregion

            #region Tiến độ
            //if (!(_context.Progresses.Any()))
            //{
            //    _context.Progresses.AddRange(new List<Progress> {
            //        new Progress {Name = "In Progress"},
            //        new Progress{ Name = "Done" },
            //        new Progress{ Name = "Pending" },
            //        new Progress{ Name = "Undone"}
            //    });
            //    _context.SaveChanges();
            //}

            #endregion

            #region Loại Tài Khoản
            if (!(_context.AccountTypes.Any()))
            {
                _context.AccountTypes.AddRange(new List<AccountType> {
                    new AccountType( "System Management", "SYSTEM"),
                    new AccountType( "Members", "MEMBER"),
                });
                _context.SaveChanges();
            }

            #endregion

            #region Tài Khoản
            if (!(_context.Accounts.Any()))
            {
                var supper = _context.AccountTypes.FirstOrDefault(x => x.Code.Equals("SYSTEM"));
                var user = _context.AccountTypes.FirstOrDefault(x => x.Code.Equals("MEMBER"));
                var account1 = new Account { Username = "admin", Password = "1", AccountTypeId = supper.Id };
                var account2 = new Account { Username = "user", Password = "1", AccountTypeId = user.Id };
                _context.Accounts.AddRange(new List<Account> {account1,
                   account2
                });
                _context.SaveChanges();
            }

            #endregion

            #region Nhóm Tài Khoản
            if (!(_context.AccountGroups.Any()))
            {

                _context.AccountGroups.AddRange(new List<AccountGroup> {
                    new AccountGroup { Name = "GM" },
                    new AccountGroup { Name = "GHM" },
                    new AccountGroup { Name = "L2" },
                    new AccountGroup { Name = "L1" },
                    new AccountGroup { Name = "L0" }
            });
                _context.SaveChanges();
            }

            #endregion


        }
    }
}
