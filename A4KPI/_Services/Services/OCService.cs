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
using A4KPI._Repositories.Interface;

namespace A4KPI._Services.Services
{
  
    public class OCService : IOCService
    {
        private OperationResult operationResult;

        private readonly IOCRepository _repo;
        private readonly IOCAccountRepository _repoOCAccount;
        private readonly IAccountRepository _repoAccount;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        public OCService(
            IOCRepository repo,
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


        public async Task<List<OCDto>> GetAllAsync()
        {
            return await _repo.FindAll().ProjectTo<OCDto>(_configMapper)
                .OrderByDescending(x => x.Id).ToListAsync();
        }

        public async Task<object> GetAllLevel3()
        {
            var lists = (await _repo.FindAll(x => x.Level == Level.Level_3).ToListAsync());
            return lists;
        }

        public async Task<IEnumerable<HierarchyNode<OCDto>>> GetAllAsTreeView()
        {
            var lists = (await _repo.FindAll().ProjectTo<OCDto>(_configMapper).OrderBy(x => x.Name).ToListAsync()).AsHierarchy(x => x.Id, y => y.ParentId);
            return lists;
        }


        public async Task<object> MappingUserOC(OCAccountDto OCAccountDto)
        {
            var item = await _repoOCAccount.FindAll(x => x.AccountId == OCAccountDto.AccountId && x.OCId == OCAccountDto.OCId).FirstOrDefaultAsync();
            if (item == null)
            {
                _repoOCAccount.Add(new OCAccount {
                    AccountId = OCAccountDto.AccountId,
                    OCId = OCAccountDto.OCId
                });
                try
                {
                   await _repo.SaveAll();
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
            var item = await _repoOCAccount.FindAll(x => x.AccountId == OCAccountDto.AccountId && x.OCId == OCAccountDto.OCId).FirstOrDefaultAsync();
            if (item != null)
            {
                _repoOCAccount.Remove(item);
                try
                {
                    await _repo.SaveAll();
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
                    var items = await _repoOCAccount.FindAll(x => x.AccountId == item && x.OCId == model.OCId).FirstOrDefaultAsync();
                    var item_username = _repoAccount.FindAll(x => x.Id == item).FirstOrDefault().FullName;
                    if (items == null)
                    {
                        _repoOCAccount.Add(new OCAccount { 
                            AccountId = item,
                            OCId = model.OCId
                        });
                        await _repo.SaveAll();
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
            return await _repoOCAccount.FindAll(x => x.OCId == ocID).ProjectTo<OCAccountDto>(_configMapper).ToListAsync();
        }

        public async Task<OperationResult> AddAsync(OCDto model)
        {
            var add = _mapper.Map<OC>(model);
            _repo.Add(add);

            try
            {
                await _repo.SaveAll();
                operationResult = new OperationResult
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = MessageReponse.UpdateSuccess,
                    Success = true,
                    Data = add
                };
            }
            catch (Exception ex)
            {
                operationResult = ex.GetMessageError();
            }
            return operationResult;
        }

      

        public async Task<OperationResult> UpdateAsync(OCDto model)
        {

            var update = _mapper.Map<OC>(model);
            _repo.Update(update);

            try
            {
                await _repo.SaveAll();
                operationResult = new OperationResult
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = MessageReponse.UpdateSuccess,
                    Success = true,
                    Data = update
                };
            }
            catch (Exception ex)
            {
                operationResult = ex.GetMessageError();
            }
            return operationResult;
            
        }

     

        public async Task<OperationResult> DeleteAsync(int id)
        {
            var delete = _repo.FindById(id);
            _repo.Remove(delete);

            var deleteChild = _repo.FindAll(x => x.ParentId == id).ToList();
            _repo.RemoveMultiple(deleteChild);
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
