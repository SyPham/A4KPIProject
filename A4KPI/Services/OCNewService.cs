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
    public interface IOCNewService: IServiceBase<OCNew, OCNewDto>
    {
        // Task<List<OCDto>> GetAllByObjectiveId(int objectiveId);
        // Task<OCDto> GetFisrtByObjectiveId(int objectiveId, int createdBy);
        Task<IEnumerable<HierarchyNode<OCNewDto>>> GetAllAsTreeView();
        Task<List<OCAccountDto>> GetUserByOcID(int ocID);
        Task<object> MappingUserOC(OCAccountDto OCAccountDto);
        Task<object> MappingRangeUserOC(OCAccountDto model);
        Task<object> GetAllLevel3();
        Task<object> RemoveUserOC(OCAccountDto OCAccountDto);
      
    }
    public class OCNewService : ServiceBase<OCNew, OCNewDto>, IOCNewService
    {
        private OperationResult operationResult;

        private readonly IRepositoryBase<OCNew> _repo;
        private readonly IRepositoryBase<OCAccount> _repoOCAccount;
        private readonly IRepositoryBase<Account> _repoAccount;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        public OCNewService(
            IRepositoryBase<OCNew> repo, 
            IRepositoryBase<OCAccount> repoOCAccount,
            IRepositoryBase<Account> repoAccount,
            IUnitOfWork unitOfWork,
            IMapper mapper, 
            MapperConfiguration configMapper
            )
            : base(repo, unitOfWork, mapper,  configMapper)
        {
            _repo = repo;
            _repoOCAccount = repoOCAccount;
            _repoAccount = repoAccount;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configMapper = configMapper;
        }

        public async Task<object> GetAllLevel3()
        {
            var lists = (await _repo.FindAll(x => x.Level == 3).ToListAsync());
            return lists;
        }
        public async Task<IEnumerable<HierarchyNode<OCNewDto>>> GetAllAsTreeView()
        {
            var lists = (await _repo.FindAll().ProjectTo<OCNewDto>(_configMapper).OrderBy(x => x.Name).ToListAsync()).AsHierarchy(x => x.Id, y => y.ParentId);
            return lists;
        }


        public async Task<object> MappingUserOC(OCAccountDto OCAccountDto)
        {
            var item = await _repoOCAccount.FindAll().FirstOrDefaultAsync(x => x.AccountId == OCAccountDto.AccountId && x.OCId == OCAccountDto.OCId);
            if (item == null)
            {
                _repoOCAccount.Add(new OCAccount {
                    AccountId = OCAccountDto.AccountId,
                    OCId = OCAccountDto.OCId
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
        }

        public async Task<object> RemoveUserOC(OCAccountDto OCAccountDto)
        {
            var item = await _repoOCAccount.FindAll().FirstOrDefaultAsync(x => x.AccountId == OCAccountDto.AccountId && x.OCId == OCAccountDto.OCId);
            if (item != null)
            {
                _repoOCAccount.Remove(item);
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
           
        }

        public async Task<object> MappingRangeUserOC(OCAccountDto model)
        {
            try
            {
                foreach (var item in model.AccountIdList)
                {
                    var items = await _repoOCAccount.FindAll().FirstOrDefaultAsync(x => x.AccountId == item && x.OCId == model.OCId);
                    var item_username = _repoAccount.FindAll().FirstOrDefault(x => x.Id == item).FullName;
                    if (items == null)
                    {
                        _repoOCAccount.Add(new OCAccount { 
                            AccountId = item,
                            OCId = model.OCId
                        });
                        await _unitOfWork.SaveChangeAsync();
                    } else
                    {
                        return new
                        {
                            status = false,
                            message = $"User {item_username} already exists in the {model.OCName}"
                        };
                    }
                }
                return new
                {
                    status = true,
                    message = "Mapping Successfully!"
                };
            }
            catch (Exception ex)
            {
                return false;
                throw;
            }
        }
       

        public async Task<List<OCAccountDto>> GetUserByOcID(int ocID)
        {
            return await _repoOCAccount.FindAll().Where(x=>x.OCId == ocID).ProjectTo<OCAccountDto>(_configMapper).ToListAsync();
        }
    }
}
