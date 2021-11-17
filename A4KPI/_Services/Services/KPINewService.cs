using AutoMapper;
using A4KPI.Data;
using A4KPI.DTO;
using A4KPI.Models;
using A4KPI._Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using A4KPI.Helpers;
using A4KPI.Constants;
using Microsoft.AspNetCore.Http;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using A4KPI._Repositories.Interface;

namespace A4KPI._Services.Services
{
 
    public class KPINewService : IKPINewService
    {
        private readonly IKPINewRepository _repo;
        private readonly IPolicyRepository _repoPolicy;
        private readonly IOCPolicyRepository _repoOcPolicy;
        private readonly IOCRepository _repoOc;
        private readonly ITypeRepository _repoType;
        private readonly IKPIAccountRepository _repoKPIAc;
        private readonly IAccountRepository _repoAc;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly MapperConfiguration _configMapper;
        private OperationResult operationResult;
        public KPINewService(
            IKPINewRepository repo,
            IPolicyRepository repoPolicy,
            ITypeRepository repoType,
            IAccountRepository repoAc,
            IKPIAccountRepository repoKPIAc,
            IOCPolicyRepository repoOcPolicy,
            IOCRepository repoOc,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper, 
            MapperConfiguration configMapper
            )
        {
            _repo = repo;
            _repoPolicy = repoPolicy;
            _repoOcPolicy = repoOcPolicy;
            _repoOc = repoOc;
            _repoKPIAc = repoKPIAc;
            _repoType = repoType;
            _httpContextAccessor = httpContextAccessor;
            _repoAc = repoAc;
            _mapper = mapper;
            _configMapper = configMapper;
        }

        public async Task<object> GetListPic()
        {
            string token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            var accountId = JWTExtensions.GetDecodeTokenById(token).ToInt();

            var dataAc = _repoAc.FindById(accountId);
            var list = new List<AccountDto>();
            var lists = (await _repoAc.FindAll().ProjectTo<AccountDto>(_configMapper).OrderBy(x => x.FullName).ToListAsync()).Select(x => new AccountDto
            {
                Id = x.Id,
                FullName = x.FullName,
                FactId = x.FactId,
                DeptId = x.DeptId,
                CenterId = x.CenterId

            }).ToList();
            if (dataAc.FactId > 0 && dataAc.CenterId == 0 && dataAc.DeptId == 0)
            {
                List<int> OcIdUnder = _repoOc.FindAll(x => x.ParentId == dataAc.FactId).Select(x => x.Id).ToList();
                list = lists.Where(x => OcIdUnder.Contains(x.CenterId.ToInt())).ToList();
            }
            if (dataAc.FactId > 0 && dataAc.CenterId > 0 && dataAc.DeptId == 0)
            {
                List<int> OcIdUnder = _repoOc.FindAll(x => x.ParentId == dataAc.CenterId).Select(x => x.Id).ToList();
                list = lists.Where(x => OcIdUnder.Contains(x.DeptId.ToInt()) || x.Id == accountId).ToList();
            }
            if (dataAc.FactId > 0 && dataAc.CenterId > 0 && dataAc.DeptId > 0)
            {
                List<int> OcIdUnder = _repoOc.FindAll(x => x.ParentId == dataAc.CenterId).Select(x => x.Id).ToList();
                list = lists.Where(x => x.DeptId == dataAc.DeptId).ToList();
            }
            var data = list.Select(x => new AccountDto { 
                Id = x.Id,
                FullName = x.FullName
            }).ToList();
            return data;
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<HierarchyNode<KPINewDto>>> GetAllAsTreeView()
        {
            var lists = (await _repo.FindAll().OrderBy(x => x.Name).ToListAsync()).Select(x => new KPINewDto
            {
                Id = x.Id,
                ParentId = x.ParentId,
                Name = x.Name,
                UpdateBy = x.UpdateBy,
                Pics = x.KPIAccounts.Count > 0 ? x.KPIAccounts.Where(y => y.KpiId == x.Id).Select(x => x.AccountId).ToList() : new List<int> { },
                TypeId = x.TypeId,
                Level = x.Level,
                TypeName = x.TypeId == 0 ? "" : _repoType.FindAll().FirstOrDefault(y => y.Id == x.TypeId).Name,
                PICName = x.KPIAccounts.Count > 0 ? String.Join(" , ", x.KPIAccounts.Select(x => _repoAc.FindById(x.AccountId).FullName)) : null,
                UpdateName = _repoAc.FindAll().FirstOrDefault(y => y.Id == x.UpdateBy).FullName ?? "",
                UpdateDate = x.UpdateDate.ToString(),
                FactName = String.Join(" , ", x.KPIAccounts.Select(x => _repoOc.FindAll(y => y.Id == x.FactId).ToList().Count > 0 ? _repoOc.FindById(x.FactId).Name : null).Where(x => !String.IsNullOrEmpty(x))),
                CenterName = String.Join(" , ", x.KPIAccounts.Select(x => _repoOc.FindAll(y => y.Id == x.CenterId).ToList().Count > 0 ? _repoOc.FindById(x.CenterId).Name : null).Where(x => !String.IsNullOrEmpty(x))),
                DeptName = String.Join(" , ", x.KPIAccounts.Select(x => _repoOc.FindAll(y => y.Id == x.DeptId).ToList().Count > 0 ? _repoOc.FindById(x.DeptId).Name : null).Where(x => !String.IsNullOrEmpty(x)))

            }).ToList().OrderBy(x => x.Level).ToList();
            var data = lists.Select(x => new KPINewDto
            {
                Id = x.Id,
                ParentId = x.ParentId,
                Name = x.Name,
                UpdateBy = x.UpdateBy,
                Pics = x.Pics,
                TypeId = x.TypeId,
                Level = x.Level,
                TypeName = x.TypeName,
                PICName = x.PICName,
                UpdateName = x.UpdateName,
                UpdateDate = x.UpdateDate,
                FactName = x.FactName,
                CenterName = x.CenterName,
                DeptName = x.DeptName

            }).ToList().AsHierarchy(x => x.Id, y => y.ParentId);
            return data;
        }

        public async Task<IEnumerable<HierarchyNode<KPINewDto>>> GetAllAsTreeView2nd3rd()
        {

            string token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            var accountId = JWTExtensions.GetDecodeTokenById(token).ToInt();

            var dataAc = _repoAc.FindById(accountId);
            var list = new List<KPINewDto>();
            var lists = (await _repo.FindAll().OrderBy(x => x.Name).ToListAsync()).Select(x => new KPINewDto
            {
                Id = x.Id,
                ParentId = x.ParentId,
                Name = x.Name,
                UpdateBy = x.UpdateBy,
                Pics = x.KPIAccounts.Count > 0 ? x.KPIAccounts.Where(y => y.KpiId == x.Id).Select(x => x.AccountId).ToList() : new List<int> { },
                CreateBy = x.CreateBy,
                LevelOcCreateBy = x.LevelOcCreateBy,
                OcIdCreateBy = x.OcIdCreateBy,
                TypeId = x.TypeId,
                Level = x.Level,
                TypeName = x.TypeId == 0 ? "" : _repoType.FindAll().FirstOrDefault(y => y.Id == x.TypeId).Name,
                PICName = x.KPIAccounts.Count > 0 ? String.Join(" , ", x.KPIAccounts.Select(x => _repoAc.FindById(x.AccountId).FullName)) : null,
                UpdateDate = x.UpdateDate.ToString(),
                UpdateName = _repoAc.FindAll().FirstOrDefault(y => y.Id == x.UpdateBy).FullName ?? "",
                FactName = String.Join(" , ", x.KPIAccounts.Select(x => _repoOc.FindAll(y => y.Id == x.FactId).ToList().Count > 0 ? _repoOc.FindById(x.FactId).Name : null).Where(x => !String.IsNullOrEmpty(x))),
                CenterName = String.Join(" , ", x.KPIAccounts.Select(x => _repoOc.FindAll(y => y.Id == x.CenterId).ToList().Count > 0 ? _repoOc.FindById(x.CenterId).Name : null).Where(x => !String.IsNullOrEmpty(x))),
                DeptName = String.Join(" , ", x.KPIAccounts.Select(x => _repoOc.FindAll(y => y.Id == x.DeptId).ToList().Count > 0 ? _repoOc.FindById(x.DeptId).Name : null).Where(x => !String.IsNullOrEmpty(x))),


            }).ToList().OrderBy(x => x.Level).ToList();
            if (dataAc.FactId > 0 && dataAc.CenterId == 0 && dataAc.DeptId == 0)
            {
                list = new List<KPINewDto>();
            }
            if (dataAc.FactId > 0 && dataAc.CenterId > 0 && dataAc.DeptId == 0)
            {
                List<int> OcIdUnder = _repoOc.FindAll(x => x.ParentId == dataAc.CenterId).Select(x => x.Id).ToList();
                var OcIdOver = _repoOc.FindAll().FirstOrDefault(x => x.Id == dataAc.CenterId).ParentId;
                var picOver = _repoAc.FindAll().FirstOrDefault(x => x.FactId == OcIdOver && x.CenterId == 0 && x.DeptId == 0).Id;
                List<int> kpiPicOver = _repoKPIAc.FindAll(x => x.AccountId == picOver).Select(x => x.KpiId).ToList();
                List<int> kpiMyPic = _repoKPIAc.FindAll(x => x.AccountId == accountId).Select(x => x.KpiId).ToList();
                var th1 = lists.Where(x => x.LevelOcCreateBy == Level.Level_1 || x.LevelOcCreateBy == Level.Level_2 || x.LevelOcCreateBy == Level.Level_3).ToList();
                list = th1.Where(x => x.CreateBy == accountId || kpiPicOver.Contains(x.Id) || kpiMyPic.Contains(x.Id) || OcIdUnder.Contains(x.OcIdCreateBy.ToInt())).ToList();
            }
            if (dataAc.FactId > 0 && dataAc.CenterId > 0 && dataAc.DeptId > 0)
            {
                List<int> picOver = _repoAc.FindAll(x => x.FactId == dataAc.FactId && x.CenterId == dataAc.CenterId && x.DeptId == 0).Select(x => x.Id).ToList();
                var kpiPicOver = new List<int>();
                foreach (var item in picOver)
                {
                    var datas = _repoKPIAc.FindAll(x => x.AccountId == item).Select(x => x.KpiId).ToList();
                    kpiPicOver.AddRange(datas);
                }
                List<int> kpiMyPic = _repoKPIAc.FindAll(x => x.AccountId == accountId).Select(x => x.KpiId).ToList();
                list = lists.Where(x => x.CreateBy == accountId || kpiMyPic.Contains(x.Id) || kpiPicOver.Contains(x.Id)).ToList();

                foreach (var item in list)
                {
                    if (item.Level == Level.Level_2)
                    {
                        item.ParentId = null;
                    }
                }

            }
            var data = list.Select(x => new KPINewDto
            {
                Id = x.Id,
                ParentId = x.ParentId,
                Name = x.Name,
                UpdateBy = x.UpdateBy,
                Pics = x.Pics,
                TypeId = x.TypeId,
                CreateBy = x.CreateBy,
                Level = x.Level,
                TypeName = x.TypeName,
                PICName = x.PICName,
                UpdateName = x.UpdateName,
                FactId = x.FactId,
                CenterId = x.CenterId,
                DeptId = x.DeptId,
                UpdateDate = x.UpdateDate,
                FactName = x.FactName,
                CenterName = x.CenterName,
                DeptName = x.DeptName
                

            }).ToList().AsHierarchy(x => x.Id, y => y.ParentId);

            
            return data;
        }


        public async Task<object> GetAllType(string lang)
        {
            var data = new List<TypeDto>();
            if (lang == "en")
            {
                data = await _repoType.FindAll().Select(x => new TypeDto { 
                    Id= x.Id,
                    Name = x.NameEn,
                    Description = x.Description
                }).ToListAsync();
            } else
            {
                data = await _repoType.FindAll().Select(x => new TypeDto
                {
                    Id = x.Id,
                    Name = x.NameZh,
                    Description = x.Description
                }).ToListAsync();
            }
            //var data = await _repoType.FindAll().ToListAsync();
            return data;
        }

        public  async Task<OperationResult> AddAsync(KPINewDto model)
        {
            try
            {
                string token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
                var accountId = JWTExtensions.GetDecodeTokenById(token).ToInt();
                var dataAc = _repoAc.FindById(accountId);
                //var dataAcPo = _repoAc.FindById(model.Pic);

                var levelCreateBy = 0;
                if (dataAc.FactId > 0 && dataAc.CenterId == 0 && dataAc.DeptId == 0)
                {
                    levelCreateBy = Level.Level_1;
                }
                if (dataAc.FactId > 0 && dataAc.CenterId > 0 && dataAc.DeptId == 0)
                {
                    levelCreateBy = Level.Level_2;

                }
                if (dataAc.FactId > 0 && dataAc.CenterId > 0 && dataAc.DeptId > 0)
                {
                    levelCreateBy = Level.Level_3;

                }

                var ocIdCreate = 0;
                if (dataAc.FactId > 0 && dataAc.CenterId == 0 && dataAc.DeptId == 0)
                {
                    ocIdCreate = dataAc.FactId.ToInt();
                }
                if (dataAc.FactId > 0 && dataAc.CenterId > 0 && dataAc.DeptId == 0)
                {
                    ocIdCreate = dataAc.CenterId.ToInt();

                }
                if (dataAc.FactId > 0 && dataAc.CenterId > 0 && dataAc.DeptId > 0)
                {
                    ocIdCreate = dataAc.DeptId.ToInt();
                }

                
                model.OcIdCreateBy = ocIdCreate;
                model.LevelOcCreateBy = levelCreateBy;
                model.CreateBy = accountId;
                model.UpdateBy = accountId;
                var item = _mapper.Map<KPINew>(model);
                item.UpdateDate = DateTime.Now;
                _repo.Add(item);
                await _repo.SaveAll();

                var list = new List<KPIAccount>();
                foreach (var acId in model.KpiIds)
                {
                    var dataAdd = new KPIAccount
                    {
                        AccountId = acId,
                        KpiId = item.Id,
                        DeptId = _repoAc.FindAll().FirstOrDefault(x => x.Id == acId).DeptId ?? 0,
                        CenterId = _repoAc.FindAll().FirstOrDefault(x => x.Id == acId).CenterId ?? 0,
                        FactId = _repoAc.FindAll().FirstOrDefault(x => x.Id == acId).FactId ?? 0

                    };
                    list.Add(dataAdd);
                }
                _repoKPIAc.AddRange(list);
                await _repoKPIAc.SaveAll();

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

        public  async Task<OperationResult> UpdateAsync(KPINewDto model)
        {
            string token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            var accountId = JWTExtensions.GetDecodeTokenById(token).ToInt();
            try
            {
                var item = await _repo.FindByIdAsync(model.Id);
                item.Name = model.Name;
                item.PolicyId = model.PolicyId;
                item.TypeId = model.TypeId;
                item.UpdateBy = accountId;
                item.UpdateDate = DateTime.Now;
                _repo.Update(item);
                await _repo.SaveAll();

                var removingList = await _repoKPIAc.FindAll(x => x.KpiId == model.Id).ToListAsync();
                _repoKPIAc.RemoveMultiple(removingList);
                await _repoKPIAc.SaveAll();

                var list = new List<KPIAccount>();
                foreach (var acId in model.KpiIds)
                {
                    var dataAdd = new KPIAccount
                    {
                        AccountId = acId,
                        KpiId = item.Id,
                        DeptId = _repoAc.FindAll().FirstOrDefault(x => x.Id == acId).DeptId ?? 0,
                        CenterId = _repoAc.FindAll().FirstOrDefault(x => x.Id == acId).CenterId ?? 0,
                        FactId = _repoAc.FindAll().FirstOrDefault(x => x.Id == acId).FactId ?? 0

                    };
                    list.Add(dataAdd);
                }
                _repoKPIAc.AddRange(list);
                await _repoKPIAc.SaveAll();
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

        public async Task<bool> Delete(int id)
        {
            var item = _repo.FindById(id);
            var itemChild = _repo.FindAll().Where(x => x.ParentId == id).ToList();
            if (itemChild != null)
            {
                _repo.RemoveMultiple(itemChild);
               await _repo.SaveAll();
            }
            try
            {
                _repo.Remove(item);
                await _repo.SaveAll();
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
           
        }

        public Task<OperationResult> AddRangeAsync(List<KPINewDto> model)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult> UpdateRangeAsync(List<KPINewDto> model)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<KPINewDto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<PagedList<KPINewDto>> GetWithPaginationsAsync(PaginationParams param)
        {
            throw new NotImplementedException();
        }

        public Task<PagedList<KPINewDto>> SearchAsync(PaginationParams param, object text)
        {
            throw new NotImplementedException();
        }

        public KPINewDto GetById(object id)
        {
            throw new NotImplementedException();
        }

        public Task<KPINewDto> GetByIdAsync(object id)
        {
            throw new NotImplementedException();
        }

        public Task<object> GetKPIByOcID(int ocID)
        {
            throw new NotImplementedException();
        }

        public Task<object> GetPolicyByOcID(int ocID)
        {
            throw new NotImplementedException();
        }
    }
}
