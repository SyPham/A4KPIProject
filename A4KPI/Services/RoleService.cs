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
    public interface IRoleService : IServiceBase<Role, RoleDto>
    {
        Task<OperationResult> LockAsync(int id);
        Task<AccountDto> GetByUsername(string username);
        Task<object> GetAccounts();
        Task<object> MapUserRole(int userID, int roleID);
        Task<object> GetRoleByUserID(int userid);
    }
    public class RoleService : ServiceBase<Role, RoleDto>, IRoleService
    {
        private readonly IRepositoryBase<Role> _repo;
        private readonly IRepositoryBase<UserRole> _repoUserRole;
        private readonly IRepositoryBase<OC> _repoOc;
        private readonly IRepositoryBase<AccountGroupAccount> _repoAccountGroupAccount;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        private OperationResult operationResult;

        public RoleService(
            IRepositoryBase<Role> repo,
            IRepositoryBase<OC> repoOC,
            IRepositoryBase<UserRole> repoUserRole,
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
        public async Task<object> GetRoleByUserID(int userid)
        {
            try
            {
                var model = await _repoUserRole.FindAll().Include(x => x.Role).FirstOrDefaultAsync(x => x.UserID == userid);

                return model.Role;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }
        public override async Task<OperationResult> AddAsync(RoleDto model)
        {
            try
            {
                var artRole = _mapper.Map<Role>(model);
                _repo.Add(artRole);
                await _unitOfWork.SaveChangeAsync();
                operationResult = new OperationResult
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = MessageReponse.AddSuccess,
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
        /// <summary>
        /// Add account sau do add AccountGroupAccount
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public override async Task<OperationResult> UpdateAsync(RoleDto model)
        {

            try
            {
                var artRole = _mapper.Map<Role>(model);
                _repo.Update(artRole);
                await _unitOfWork.SaveChangeAsync();
                operationResult = new OperationResult
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = MessageReponse.AddSuccess,
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

        public override async Task<List<RoleDto>> GetAllAsync()
        {
            return await _repo.FindAll(x => x.Code != "SUPPER_ADMIN").ProjectTo<RoleDto>(_configMapper).OrderBy(x => x.ID).ToListAsync();
            //throw new NotImplementedException();
        }

        public Task<OperationResult> LockAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<AccountDto> GetByUsername(string username)
        {
            throw new NotImplementedException();
        }

        public Task<object> GetAccounts()
        {
            throw new NotImplementedException();
        }

        public async Task<object> MapUserRole(int userID, int roleID)
        {
            var item = await _repoUserRole.FindAll().Where(x => x.UserID == userID).ToListAsync();
            if (item.Count == 0)
            {
                try
                {
                    _repoUserRole.Add(new UserRole
                    {
                        UserID = userID,
                        RoleID = roleID,
                    });

                    return new
                    {
                        status = await _unitOfWork.SaveChangeAsync(),
                        message = "Mapping Successfully!"
                    };
                }
                catch (Exception)
                {
                    return new
                    {
                        status = false,
                        message = "Failed on save!"
                    };
                }
            }
            else
            {

                try
                {
                    _repoUserRole.RemoveMultiple(item);
                    await _unitOfWork.SaveChangeAsync();

                    _repoUserRole.Add(new UserRole
                    {
                        UserID = userID,
                        RoleID = roleID,
                    });


                    return new
                    {
                        status = await _unitOfWork.SaveChangeAsync(),
                        message = "Mapping Successfully!"
                    };
                }
                catch (Exception)
                {
                    return new
                    {
                        status = false,
                        message = "Failed on save!"
                    };
                }
            }
            throw new NotImplementedException();
        }
    }
}
