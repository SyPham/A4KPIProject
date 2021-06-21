using Microsoft.EntityFrameworkCore;
using ScoreKPI.Data;
using ScoreKPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScoreKPI.Services
{
    public interface IAuthService
    {
        Task<Account> Login(string username, string password);
    }
    public class AuthService : IAuthService
    {
        private readonly IRepositoryBase<Account> _repo;

        public AuthService(
            IRepositoryBase<Account> repo
            )
        {
            _repo = repo;
        }
        public async Task<Account> Login(string username, string password)
        {
            var account = await _repo.FindAll().FirstOrDefaultAsync(x => x.Username == username && x.Password == password);

            if (account == null)
                return null;

            return account;
        }
      
    }
}
