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
            if (!(_context.Progresses.Any()))
            {
                _context.Progresses.AddRange(new List<Progress> {
                    new Progress {Name = "In Progress"},
                    new Progress{ Name = "Done" },
                    new Progress{ Name = "Pending" },
                    new Progress{ Name = "Undone"}
                });
                _context.SaveChanges();
            }

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
