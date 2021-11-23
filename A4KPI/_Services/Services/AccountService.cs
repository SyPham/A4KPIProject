using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using A4KPI.Constants;
using A4KPI.Data;
using A4KPI.DTO;
using A4KPI.Helpers;
using A4KPI.Models;
using A4KPI._Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Extensions.Configuration;
using A4KPI._Repositories.Interface;

namespace A4KPI._Services.Services
{

    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _repo;
        private readonly IOCRepository _repoOc;
        private readonly IUserRoleRepository _repoUserRole;
        private readonly IRoleRepository _repoRole;
        private readonly IAccountGroupAccountRepository _repoAccountGroupAccount;
        private readonly IMapper _mapper;
        private readonly IMailExtension _mailHelper;
        private readonly MapperConfiguration _configMapper;
        private readonly IConfiguration _configuration;
        private OperationResult operationResult;

        public AccountService(
            IAccountRepository repo,
            IOCRepository repoOC,
            IUserRoleRepository repoUserRole,
            IRoleRepository repoRole,
            IAccountGroupAccountRepository repoAccountGroupAccount,
            IMapper mapper,
            IMailExtension mailExtension,
            IConfiguration configuration,
            MapperConfiguration configMapper
            )
        {
            _repo = repo;
            _repoOc = repoOC;
            _repoUserRole = repoUserRole;
            _repoRole = repoRole;
            _repoAccountGroupAccount = repoAccountGroupAccount;
            _mapper = mapper;
            _mailHelper = mailExtension;
            _configuration = configuration;
            _configMapper = configMapper;
        }
        /// <summary>
        /// Add account sau do add AccountGroupAccount
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// 

        public async Task<object> ChangePasswordAsync2(ChangePasswordRequest request)
        {
            var listEmail = new List<string>();
            var item = await _repo.FindByIdAsync(request.Id);
            if (item == null)
            {
                return new OperationResult { StatusCode = HttpStatusCode.NotFound, Message = "Không tìm thấy tài khoản này! Not found the account", Success = false };
            }
            item.Password = request.NewPassword.ToEncrypt();
            listEmail.Add(item.Email);
            try
            {
                _repo.Update(item);

                await _repo.SaveAll();
                return true;
            }
            catch (Exception ex)
            {
                return false;
                //operationResult = ex.GetMessageError();
            }
            //return operationResult;
        }

        public async Task<OperationResult> ChangePasswordAsync(ChangePasswordRequest request)
        {
            var listEmail = new List<string>();
            var item = await _repo.FindByIdAsync(request.Id);
            if (item == null)
            {
                return new OperationResult { StatusCode = HttpStatusCode.NotFound, Message = "Không tìm thấy tài khoản này! Not found the account", Success = false };
            }
            item.Password = request.NewPassword.ToEncrypt();
            try
            {
                _repo.Update(item);

                await _repo.SaveAll();
                operationResult = new OperationResult
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = "Successfully 成功地!",
                    Success = true,
                    Data = item
                };
            }
            catch (Exception ex)
            {
                operationResult = ex.GetMessageError();
            }
            return operationResult;
        }

        public async Task<OperationResult> AddAsync(AccountDto model)
        {
            try
            {
                var item = _mapper.Map<Account>(model);
                item.Password = item.Password.ToEncrypt();
                _repo.Add(item);
                int id = await AddAccount(item);
                var list = new List<AccountGroupAccount>();
                foreach (var accountGroupId in model.AccountGroupIds)
                {
                    list.Add(new AccountGroupAccount(accountGroupId, id));
                }

                _repoAccountGroupAccount.AddRange(list);
                await _repo.SaveAll();

                operationResult = new OperationResult
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = MessageReponse.AddSuccess,
                    Success = true,
                    Data = id
                };
            }
            catch (Exception ex)
            {
                operationResult = ex.GetMessageError();
            }
            return operationResult;
        }

        public async Task<int> AddAccount(Account item)
        {
            _repo.Add(item);
            await _repo.SaveAll();
            return item.Id;
        }
        /// <summary>
        /// Add account sau do add AccountGroupAccount
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<OperationResult> UpdateAsync(AccountDto model)
        {
            try
            {
                var item = await _repo.FindByIdAsync(model.Id);
                if (model.Password.IsBase64() == false)
                    item.Password = model.Password.ToEncrypt();
                item.Username = model.Username;
                item.FullName = model.FullName;
                item.Email = model.Email;
                item.Leader = model.Leader;
                item.FactId = model.FactId;
                item.CenterId = model.CenterId;
                item.DeptId = model.DeptId;
                item.Manager = model.Manager;
                _repo.Update(item);

                var removingList = await _repoAccountGroupAccount.FindAll(x => x.AccountId == item.Id).ToListAsync();
                _repoAccountGroupAccount.RemoveMultiple(removingList);

                var list = new List<AccountGroupAccount>();
                foreach (var accountGroupId in model.AccountGroupIds)
                {
                    list.Add(new AccountGroupAccount(accountGroupId, item.Id));
                }
                _repoAccountGroupAccount.AddRange(list);

                await _repoAccountGroupAccount.SaveAll();

                operationResult = new OperationResult
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = MessageReponse.UpdateSuccess,
                    Success = true,
                    Data = model
                };
            }
            catch (Exception ex)
            {
                operationResult = ex.GetMessageError();
            }
            return operationResult;
        }

        public async Task<List<AccountDto>> GetAllAsync()
        {
            var query = _repo.FindAll(x => x.AccountType.Code != Systems.Administrator);
            var model = from a in query
                        join b in query on a.Leader equals b.Id into ab
                        from ab1 in ab.DefaultIfEmpty()
                        join c in query on a.Manager equals c.Id into ac
                        from ac1 in ac.DefaultIfEmpty()
                        join d in _repoUserRole.FindAll() on a.Id equals d.UserID
                        select new AccountDto
                        {
                            Id = a.Id,
                            Username = a.Username,
                            Password = a.Password,
                            FactId = a.FactId != null ? a.FactId : 0,
                            CenterId = a.CenterId != null ? a.CenterId : 0,
                            DeptId = a.DeptId != null ? a.DeptId : 0,
                            CreatedBy = a.CreatedBy,
                            CreatedTime = a.CreatedTime,
                            ModifiedBy = a.ModifiedBy,
                            ModifiedTime = a.ModifiedTime,
                            IsLock = a.IsLock,
                            AccountTypeId = a.AccountTypeId,
                            AccountGroupIds = a.AccountGroupAccount.Count > 0 ? a.AccountGroupAccount.Select(x => x.AccountGroup.Id).ToList() : new List<int> { },
                            AccountGroupText = a.AccountGroupAccount.Count > 0 ? String.Join(",", a.AccountGroupAccount.Select(x => x.AccountGroup.Name)) : "",
                            FullName = a.FullName,
                            Email = a.Email,
                            LeaderName = ab1 != null ? ab1.FullName : "N/A",
                            Manager = a.Manager != null ? a.Manager : 0,
                            Leader = a.Leader != null ? a.Leader : 0,
                            ManagerName = ac1 != null ? ac1.FullName : "N/A",
                            FactName = a.FactId != null || a.FactId != 0 ? _repoOc.FindById(a.FactId).Name : "N/A",
                            CenterName = a.CenterId != null || a.CenterId != 0 ? _repoOc.FindById(a.CenterId).Name : "N/A",
                            DeptName = a.DeptId != null || a.DeptId != 0 ? _repoOc.FindById(a.DeptId).Name : "N/A",
                            Role = _repoRole.FindById(d.RoleID) != null ? _repoRole.FindById(d.RoleID).Name : "N/A",
                            RoleCode = _repoRole.FindById(d.RoleID) != null ? _repoRole.FindById(d.RoleID).Code : "N/A",

                        };
            var data = await model.ToListAsync();
            return data;

        }


        public async Task<OperationResult> LockAsync(int id)
        {
            var item = await _repo.FindByIdAsync(id);
            if (item == null)
            {
                return new OperationResult { StatusCode = HttpStatusCode.NotFound, Message = "Không tìm thấy tài khoản này!", Success = false };
            }
            item.IsLock = !item.IsLock;
            try
            {
                _repo.Update(item);
                await _repoOc.SaveAll();
                operationResult = new OperationResult
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = item.IsLock ? MessageReponse.LockSuccess : MessageReponse.UnlockSuccess,
                    Success = true,
                    Data = item
                };
            }
            catch (Exception ex)
            {
                operationResult = ex.GetMessageError();
            }
            return operationResult;
        }

        public async Task<AccountDto> GetByUsername(string username)
        {
            var result = await _repo.FindAll(x => x.Username.ToLower() == username.ToLower()).ProjectTo<AccountDto>(_configMapper).FirstOrDefaultAsync();
            return result;
        }

        public async Task<object> GetAccounts()
        {
            var query = await _repo.FindAll(x => x.AccountType.Code != "SYSTEM").Select(x => new
            {
                x.Username,
                x.Id,
                x.FullName,
                IsLeader = x.AccountGroupAccount.Any(a => a.AccountGroup.Position == SystemRole.FunctionalLeader)
            }).ToListAsync();
            return query;
        }

        public async Task<OperationResult> DeleteAsync(int id)
        {
            var delete = _repo.FindById(id);
            _repo.Remove(delete);

            try
            {
                await _repo.SaveAll();
                operationResult = new OperationResult
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = MessageReponse.UpdateSuccess,
                    Success = true,
                    Data = delete
                };
            }
            catch (Exception ex)
            {
                operationResult = ex.GetMessageError();
            }
            return operationResult;
        }


    }
}
