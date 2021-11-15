using AutoMapper;
using AutoMapper.QueryableExtensions;
using ESS_API.Helpers;
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
using A4KPI._Repositories.Interface;

namespace A4KPI._Services.Services
{
  
    public class OCNewService : IOCNewService
    {
        private OperationResult operationResult;

        private readonly IOCNewRepository _repo;
        private readonly IOCAccountRepository _repoOCAccount;
        private readonly IAccountRepository _repoAccount;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        public OCNewService(
            IOCNewRepository repo,
            IOCAccountRepository repoOCAccount,
            IAccountRepository repoAccount,
            IMapper mapper, 
            MapperConfiguration configMapper
            )
        {
            _repo = repo;
            _repoOCAccount = repoOCAccount;
            _repoAccount = repoAccount;
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
                   await _repoOCAccount.SaveAll();
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
                    await _repoOCAccount.SaveAll();
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
                        await _repoOCAccount.SaveAll();
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

        public Task<OperationResult> AddAsync(OCNewDto model)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult> AddRangeAsync(List<OCNewDto> model)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult> UpdateAsync(OCNewDto model)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult> UpdateRangeAsync(List<OCNewDto> model)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<OCNewDto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<PagedList<OCNewDto>> GetWithPaginationsAsync(PaginationParams param)
        {
            throw new NotImplementedException();
        }

        public Task<PagedList<OCNewDto>> SearchAsync(PaginationParams param, object text)
        {
            throw new NotImplementedException();
        }

        public OCNewDto GetById(object id)
        {
            throw new NotImplementedException();
        }

        public Task<OCNewDto> GetByIdAsync(object id)
        {
            throw new NotImplementedException();
        }
    }
}
