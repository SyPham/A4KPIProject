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
  
    public class OCPolicyService : IOCPolicyService
    {
        private OperationResult operationResult;

        private readonly IOCPolicyRepository _repo;
        private readonly IOCRepository _repoOc;
        private readonly IPolicyRepository _repoPolicy;
        private readonly IOCAccountRepository _repoOCAccount;
        private readonly IAccountRepository _repoAccount;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        public OCPolicyService(
            IOCPolicyRepository repo,
            IOCRepository repoOc,
            IOCAccountRepository repoOCAccount,
            IPolicyRepository repoPolicy,
            IAccountRepository repoAccount,
            IMapper mapper, 
            MapperConfiguration configMapper
            )
        {
            _repo = repo;
            _repoOc = repoOc;
            _repoOCAccount = repoOCAccount;
            _repoAccount = repoAccount;
            _repoPolicy = repoPolicy;
            _mapper = mapper;
            _configMapper = configMapper;
        }

        public async Task<object> RemovePolicyOC(OCPolicyDto Dto)
        {
            var item_policy = _repoPolicy.FindById(Dto.ID);
            item_policy.Name = Dto.Name;
            await _repoPolicy.SaveAll();

            var item = _repo.FindAll(x => x.PolicyId == Dto.ID).ToList();
            if (item != null)
            {
                _repo.RemoveMultiple(item);
                await _repo.SaveAll();
                foreach (var items in Dto.OcIdList)
                {
                    _repo.Add(new OCPolicy
                    {
                        OcId = items,
                        PolicyId = Dto.ID,
                        OcName = _repoOc.FindAll().FirstOrDefault(x => x.Id == items).Name
                    });
                }
                try
                {
                    await _repo.SaveAll();
                    return new
                    {
                        status = true,
                        message = "Successfully!"
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
        public async Task<object> GetAllPolicy()
        {
            var data = await _repoPolicy.FindAll().Select(x => new OCPolicyDto
            {
                ID = x.Id,
                Name = x.Name,
                Factory = _repo.FindAll().Where(y => y.PolicyId == x.Id).Select(y => y.OcId).ToList(),
                FactoryName = String.Join(",", _repo.FindAll().Where(y => y.PolicyId == x.Id).Select(y => y.OcName))
            }).ToListAsync();
            return data;
        }
        public async Task<object> GetAllLevel3()
        {
            var lists = (await _repo.FindAll(x => x.OcId == 3).ToListAsync());
            return lists;
        }
        public async Task<IEnumerable<HierarchyNode<OCDto>>> GetAllAsTreeView()
        {
            var lists = (await _repo.FindAll().ProjectTo<OCDto>(_configMapper).OrderBy(x => x.Name).ToListAsync()).AsHierarchy(x => x.Id, y => y.ParentId);
            return lists;
        }


        public async Task<object> MappingPolicyOC(OCPolicyDto Dto)
        {
            var item = await _repoPolicy.FindAll().FirstOrDefaultAsync(x => x.Name.ToUpper().Equals(Dto.Name.ToUpper()));
            if (item == null)
            {
                var dataAdd = new Policy
                {
                    Name = Dto.Name
                };
                _repoPolicy.Add(dataAdd);
                await _repoPolicy.SaveAll();
                foreach (var items in Dto.OcIdList)
                {
                    var itemOc = await _repo.FindAll().FirstOrDefaultAsync(x => x.OcId == items && x.PolicyId == dataAdd.Id);
                    if (itemOc == null)
                    {
                        _repo.Add(new OCPolicy
                        {
                            OcId = items,
                            PolicyId = dataAdd.Id,
                            OcName = _repoOc.FindAll().FirstOrDefault(x => x.Id == items).Name
                        });
                    }
                }
                try
                {
                   await _repo.SaveAll();
                    return new
                    {
                        status = true,
                        message = "Successfully!"
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
                    message = "The Policy Already Exist!"
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

        public async Task<bool> DeletePolicy(int id)
        {
            var data_policy = _repoPolicy.FindById(id);
            if (data_policy != null)
            {
                _repoPolicy.Remove(data_policy);
            }
            var data_oc_policy = _repo.FindAll(x => x.PolicyId == id).ToList();
            if (data_oc_policy != null)
            {
                _repo.RemoveMultiple(data_oc_policy);
            }
            try
            {
                await _repo.SaveAll();
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
            throw new NotImplementedException();
        }
    }
}
