using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using A4KPI.Constants;
using A4KPI.Data;
using A4KPI.DTO;
using A4KPI.Helpers;
using A4KPI.Models;
using A4KPI.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace A4KPI.Services
{
    public interface IAccountService : IServiceBase<Account, AccountDto>
    {
        Task<OperationResult> LockAsync(int id);
        Task<AccountDto> GetByUsername(string username);
        Task<OperationResult> ChangePasswordAsync(ChangePasswordRequest request);
        Task<object> GetAccounts();
    }
    public class AccountService : ServiceBase<Account, AccountDto>, IAccountService
    {
        private readonly IRepositoryBase<Account> _repo;
        private readonly IRepositoryBase<OC> _repoOc;
        private readonly IRepositoryBase<UserRole> _repoUserRole;
        private readonly IRepositoryBase<Role> _repoRole;
        private readonly IRepositoryBase<AccountGroupAccount> _repoAccountGroupAccount;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        private OperationResult operationResult;

        public AccountService(
            IRepositoryBase<Account> repo,
            IRepositoryBase<OC> repoOC,
            IRepositoryBase<UserRole> repoUserRole,
            IRepositoryBase<Role> repoRole,
            IRepositoryBase<AccountGroupAccount> repoAccountGroupAccount,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            MapperConfiguration configMapper
            )
            : base(repo, unitOfWork, mapper, configMapper)
        {
            _repo = repo;
            _repoOc = repoOC;
            _repoUserRole = repoUserRole;
            _repoRole = repoRole;
            _repoAccountGroupAccount = repoAccountGroupAccount;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configMapper = configMapper;
        }
        /// <summary>
        /// Add account sau do add AccountGroupAccount
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// 
        public async Task<OperationResult> ChangePasswordAsync(ChangePasswordRequest request)
        {
            var item = await _repo.FindByIdAsync(request.Id);
            if (item == null)
            {
                return new OperationResult { StatusCode = HttpStatusCode.NotFound, Message = "Không tìm thấy tài khoản này! Not found the account", Success = false };
            }
            item.Password = request.NewPassword.ToEncrypt();
            try
            {
                _repo.Update(item);
                await _unitOfWork.SaveChangeAsync();
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
        public override async Task<OperationResult> AddAsync(AccountDto model)
        {
            try
            {
                var item = _mapper.Map<Account>(model);
                item.Password = item.Password.ToEncrypt();
                _repo.Add(item);

                await _unitOfWork.SaveChangeAsync();
                var list = new List<AccountGroupAccount>();
                foreach (var accountGroupId in model.AccountGroupIds)
                {
                    list.Add(new AccountGroupAccount(accountGroupId, item.Id));
                }
                _repoAccountGroupAccount.AddRange(list);
                await _unitOfWork.SaveChangeAsync();

                operationResult = new OperationResult
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = MessageReponse.AddSuccess,
                    Success = true,
                    Data = item.Id
                };
            }
            catch (Exception ex)
            {
                operationResult = ex.GetMessageError();
            }
            return operationResult;
        }
        /// <summary>
        /// Add account sau do add AccountGroupAccount
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public override async Task<OperationResult> UpdateAsync(AccountDto model)
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
                await _unitOfWork.SaveChangeAsync();
                var removingList = await _repoAccountGroupAccount.FindAll(x => x.AccountId == item.Id).ToListAsync();
                _repoAccountGroupAccount.RemoveMultiple(removingList);
                await _unitOfWork.SaveChangeAsync();

                var list = new List<AccountGroupAccount>();
                foreach (var accountGroupId in model.AccountGroupIds)
                {
                    list.Add(new AccountGroupAccount(accountGroupId, item.Id));
                }
                _repoAccountGroupAccount.AddRange(list);
                await _unitOfWork.SaveChangeAsync();

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
        public override async Task<List<AccountDto>> GetAllAsync()
        {
            var query = _repo.FindAll(x => x.AccountType.Code != "SYSTEM");
            var model = from a in query
                        join b in query on a.Leader equals b.Id into ab
                        from ab1 in ab.DefaultIfEmpty()
                        join c in query on a.Manager equals c.Id into ac
                        from ac1 in ac.DefaultIfEmpty()
                        select new AccountDto
                        {
                            Id = a.Id,
                            Username = a.Username,
                            Password = a.Password,
                            FactId = a.FactId != null  ? a.FactId : 0,
                            CenterId = a.CenterId != null  ? a.CenterId : 0,
                            DeptId = a.DeptId != null  ? a.DeptId : 0,
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
                            FactName = a.FactId != null ? _repoOc.FindById(a.FactId).Name : "N/A",
                            CenterName = a.CenterId != null ? _repoOc.FindById(a.CenterId).Name : "N/A",
                            DeptName = a.DeptId != null ? _repoOc.FindById(a.DeptId).Name : "N/A",
                            Role = _repoRole.FindAll().FirstOrDefault(x => x.ID == _repoUserRole.FindAll().FirstOrDefault(y => y.UserID == a.Id).RoleID).Name ?? "N/A",
                            RoleCode = _repoRole.FindAll().FirstOrDefault(x => x.ID == _repoUserRole.FindAll().FirstOrDefault(y => y.UserID == a.Id).RoleID).Code ?? "N/A"

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
                await _unitOfWork.SaveChangeAsync();
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
            var query = await _repo.FindAll(x => x.AccountType.Code != "SYSTEM").Select(x=> new { 
            x.Username,
            x.Id,
            x.FullName,
            IsLeader = x.AccountGroupAccount.Any(a => a.AccountGroup.Position  == SystemRole.FunctionalLeader)
            }).ToListAsync();
            return query;
        }
    }
}
