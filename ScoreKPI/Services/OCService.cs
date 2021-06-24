using AutoMapper;
using AutoMapper.QueryableExtensions;
using ESS_API.Helpers;
using Microsoft.EntityFrameworkCore;
using ScoreKPI.Constants;
using ScoreKPI.Data;
using ScoreKPI.DTO;
using ScoreKPI.Helpers;
using ScoreKPI.Models;
using ScoreKPI.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ScoreKPI.Services
{
    public interface IOCService: IServiceBase<OC, OCDto>
    {
        // Task<List<OCDto>> GetAllByObjectiveId(int objectiveId);
        // Task<OCDto> GetFisrtByObjectiveId(int objectiveId, int createdBy);
        Task<IEnumerable<HierarchyNode<OCDto>>> GetAllAsTreeView();
        Task<List<AccountDto>> GetUserByOCname(string name);
        Task<object> MappingUserOC(OcUserDto OcUserDto);
        Task<object> MappingRangeUserOC(OcUserDto model);
        Task<object> RemoveUserOC(OcUserDto OcUserDto);
      
    }
    public class OCService : ServiceBase<OC, OCDto>, IOCService
    {
        private OperationResult operationResult;

        private readonly IRepositoryBase<OC> _repo;
        private readonly IRepositoryBase<Account> _repoAccount;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        public OCService(
            IRepositoryBase<OC> repo, 
            IRepositoryBase<Account> repoAccount,
            IUnitOfWork unitOfWork,
            IMapper mapper, 
            MapperConfiguration configMapper
            )
            : base(repo, unitOfWork, mapper,  configMapper)
        {
            _repo = repo;
             _repoAccount = repoAccount;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configMapper = configMapper;
        }

        public async Task<IEnumerable<HierarchyNode<OCDto>>> GetAllAsTreeView()
        {
            var lists = (await _repo.FindAll().ProjectTo<OCDto>(_configMapper).OrderBy(x => x.Name).ToListAsync()).AsHierarchy(x => x.Id, y => y.ParentId);
            return lists;
        }

        public async Task<List<AccountDto>> GetUserByOCname(string name)
        {
            return await _repoAccount.FindAll().Where(x=> x.RoleOC.Contains(name.ToUpper())).ProjectTo<AccountDto>(_configMapper).ToListAsync();
        }

        public async Task<object> MappingUserOC(OcUserDto OcUserDto)
        {
            var test = _repoAccount.FindById(OcUserDto.UserID) ;
            if(test == null)
               return new
                {
                    status = false,
                    message = "Failed on save!"
                };
            List<string> list = test.RoleOC.Split(',').ToList();
            if (!test.RoleOC.Contains(OcUserDto.OCname)) {
                list.Add(OcUserDto.OCname);
            }
            test.RoleOC = string.Join(",", list);
            try
            {
                await _repoAccount.SaveAll();
                return new
                {
                    status = true,
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

        public async Task<object> RemoveUserOC(OcUserDto OcUserDto)
        {
            // string[] termsList;
            var test = _repoAccount.FindById(OcUserDto.UserID) ;
            // List<string> list = new List<string>(termsList);
            List<string> list = test.RoleOC.Split(',').ToList();
            // termsList.Append();
            if (test.RoleOC.Contains(OcUserDto.OCname)) {
                list.Remove(OcUserDto.OCname);
            }
            test.RoleOC = string.Join(",", list);
            try
            {
                await _repoAccount.SaveAll();
                return new
                {
                    status = true,
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

        public async Task<object> MappingRangeUserOC(OcUserDto model)
        {
            try
            {
                foreach (var item in model.AccountIdList)
                {
                    var mapping = _repoAccount.FindById(item);
                    List<string> list = new List<string>();
                    if(mapping == null)
                        return new
                        {
                            status = false,
                            message = "Failed on save!"
                        };
                    if (mapping.RoleOC == null || mapping.RoleOC == "")
                    {
                        mapping.RoleOC = model.OCname;
                    } else {
                        list = mapping.RoleOC.Split(',').ToList();
                        if (!mapping.RoleOC.Contains(model.OCname)) {
                            list.Add(model.OCname);
                        } else {
                            return new
                            {
                                status = false,
                                message = $"User {mapping.FullName} already exists in the {model.OCname}"
                            };
                        }
                        mapping.RoleOC = string.Join(",", list);
                    }
                    await _repoAccount.SaveAll();
                }
                return new
                {
                    status = true,
                    message = "Mapping Successfully!"
                };
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }
       
    }
}
