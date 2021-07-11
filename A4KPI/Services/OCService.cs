using AutoMapper;
using AutoMapper.QueryableExtensions;
using ESS_API.Helpers;
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
    public interface IOCService: IServiceBase<OC, OCDto>
    {
        // Task<List<OCDto>> GetAllByObjectiveId(int objectiveId);
        // Task<OCDto> GetFisrtByObjectiveId(int objectiveId, int createdBy);
        Task<IEnumerable<HierarchyNode<OCDto>>> GetAllAsTreeView();
        Task<List<OcUserDto>> GetUserByOcID(int ocID);
        Task<object> MappingUserOC(OcUserDto OcUserDto);
        Task<object> MappingRangeUserOC(OcUserDto model);
        Task<object> RemoveUserOC(OcUserDto OcUserDto);
      
    }
    public class OCService : ServiceBase<OC, OCDto>, IOCService
    {
        private OperationResult operationResult;

        private readonly IRepositoryBase<OC> _repo;
        private readonly IRepositoryBase<OCUser> _repoOcUser;
        private readonly IRepositoryBase<Account> _repoAccount;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        public OCService(
            IRepositoryBase<OC> repo, 
            IRepositoryBase<OCUser> repoOcUser,
            IRepositoryBase<Account> repoAccount,
            IUnitOfWork unitOfWork,
            IMapper mapper, 
            MapperConfiguration configMapper
            )
            : base(repo, unitOfWork, mapper,  configMapper)
        {
            _repo = repo;
            _repoOcUser = repoOcUser;
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


        public async Task<object> MappingUserOC(OcUserDto OcUserDto)
        {
            var item = await _repoOcUser.FindAll().FirstOrDefaultAsync(x => x.UserID == OcUserDto.UserID && x.OCID == OcUserDto.OCID);
            if (item == null)
            {
                _repoOcUser.Add(new OCUser { 
                    UserID = OcUserDto.UserID,
                    OCID = OcUserDto.OCID
                });
                try
                {
                   await _unitOfWork.SaveChangeAsync();
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
            } else
            {

                return new
                {
                    status = false,
                    message = "The User belonged with other building!"
                };
            }
            // var test = _repoAccount.FindById(OcUserDto.UserID) ;
            // if(test == null)
            //    return new
            //     {
            //         status = false,
            //         message = "Failed on save!"
            //     };
            // List<string> list = test.RoleOC.Split(',').ToList();
            // if (!test.RoleOC.Equals(OcUserDto.OCname)) {
            //     list.Add(OcUserDto.OCname);
            // }
            // test.RoleOC = string.Join(",", list);
            // try
            // {
            //     await _repoAccount.SaveAll();
            //     return new
            //     {
            //         status = true,
            //         message = "Mapping Successfully!"
            //     };
            // }
            // catch (Exception)
            // {
            //     return new
            //     {
            //         status = false,
            //         message = "Failed on save!"
            //     };
            // }
        }

        public async Task<object> RemoveUserOC(OcUserDto OcUserDto)
        {
            var item = await _repoOcUser.FindAll().FirstOrDefaultAsync(x => x.UserID == OcUserDto.UserID && x.OCID == OcUserDto.OCID);
            if (item != null)
            {
                _repoOcUser.Remove(item);
                try
                {
                    await _unitOfWork.SaveChangeAsync();
                    return new
                    {
                        status = true,
                        message = "Delete Successfully!"
                    };
                }
                catch (Exception)
                {
                    return new
                    {
                        status = false,
                        message = "Failed on delete!"
                    };
                }
            }
            else
            {

                return new
                {
                    status = false,
                    message = ""
                };
            }
            // // string[] termsList;
            // var test = _repoAccount.FindById(OcUserDto.UserID) ;
            // // List<string> list = new List<string>(termsList);
            // List<string> list = test.RoleOC.Split(',').ToList();
            // // termsList.Append();
            // if (test.RoleOC.Equals(OcUserDto.OCname)) {
            //     list.Remove(OcUserDto.OCname);
            // }
            // test.RoleOC = string.Join(",", list);
            // try
            // {
            //     await _repoAccount.SaveAll();
            //     return new
            //     {
            //         status = true,
            //         message = "Mapping Successfully!"
            //     };
            // }
            // catch (Exception)
            // {
            //     return new
            //     {
            //         status = false,
            //         message = "Failed on save!"
            //     };
            // }
        }

        public async Task<object> MappingRangeUserOC(OcUserDto model)
        {
            try
            {
                foreach (var item in model.AccountIdList)
                {
                    var items = await _repoOcUser.FindAll().FirstOrDefaultAsync(x => x.UserID == item && x.OCID == model.OCID);
                    var item_username = _repoAccount.FindAll().FirstOrDefault(x => x.Id == item).FullName;
                    if (items == null)
                    {
                        _repoOcUser.Add(new OCUser { 
                            UserID = item,
                            OCID = model.OCID
                        });
                        await _unitOfWork.SaveChangeAsync();
                    } else
                    {
                        return new
                        {
                            status = false,
                            message = $"User {item_username} already exists in the {model.OCname}"
                        };
                    }
                    // var mapping = _repoAccount.FindById(item);
                    // List<string> list = new List<string>();
                    // if(mapping == null)
                    //     return new
                    //     {
                    //         status = false,
                    //         message = "Failed on save!"
                    //     };
                    // if (mapping.RoleOC == null || mapping.RoleOC == "")
                    // {
                    //     mapping.RoleOC = model.OCname;
                    // } else {
                    //     list = mapping.RoleOC.Split(',').ToList();
                    //     if (!mapping.RoleOC.Equals(model.OCname)) {
                    //         list.Add(model.OCname);
                    //     } else {
                    //         return new
                    //         {
                    //             status = false,
                    //             message = $"User {mapping.FullName} already exists in the {model.OCname}"
                    //         };
                    //     }
                    //     mapping.RoleOC = string.Join(",", list);
                    // }
                    // await _repoAccount.SaveAll();
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
       

        public async Task<List<OcUserDto>> GetUserByOcID(int ocID)
        {
            return await _repoOcUser.FindAll().Where(x=>x.OCID == ocID).ProjectTo<OcUserDto>(_configMapper).ToListAsync();
        }
    }
}
