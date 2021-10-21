using AutoMapper;
using A4KPI.Data;
using A4KPI.DTO;
using A4KPI.Models;
using A4KPI.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using A4KPI.Helpers;
using A4KPI.Constants;
using Microsoft.AspNetCore.Http;
using NetUtility;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace A4KPI.Services
{
    public interface IKPINewService: IServiceBase<KPINew, KPINewDto>
    {
        Task<object> GetKPIByOcID(int ocID);
        Task<object> GetListPic();
        Task<object> GetPolicyByOcID(int ocID);
        Task<object> GetAllType();
        Task<bool> Delete(int id);
        Task<IEnumerable<HierarchyNode<KPINewDto>>> GetAllAsTreeView();
        Task<IEnumerable<HierarchyNode<KPINewDto>>> GetAllAsTreeView2nd3rd();
    }
    public class KPINewService : ServiceBase<KPINew, KPINewDto>, IKPINewService
    {
        private readonly IRepositoryBase<KPINew> _repo;
        private readonly IRepositoryBase<Policy> _repoPolicy;
        private readonly IRepositoryBase<OCPolicy> _repoOcPolicy;
        private readonly IRepositoryBase<OC> _repoOc;
        private readonly IRepositoryBase<Types> _repoType;
        private readonly IRepositoryBase<Account> _repoAc;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly MapperConfiguration _configMapper;
        private OperationResult operationResult;
        public KPINewService(
            IRepositoryBase<KPINew> repo, 
            IRepositoryBase<Policy> repoPolicy, 
            IRepositoryBase<Types> repoType, 
            IRepositoryBase<Account> repoAc,
            IRepositoryBase<OCPolicy> repoOcPolicy,
            IRepositoryBase<OC> repoOc,
            IUnitOfWork unitOfWork,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper, 
            MapperConfiguration configMapper
            )
            : base(repo, unitOfWork, mapper,  configMapper)
        {
            _repo = repo;
            _repoPolicy = repoPolicy;
            _repoOcPolicy = repoOcPolicy;
            _repoOc = repoOc;
            _repoType = repoType;
            _httpContextAccessor = httpContextAccessor;
            _repoAc = repoAc;
            _unitOfWork = unitOfWork;
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
            var lists = (await _repo.FindAll().ProjectTo<KPINewDto>(_configMapper).OrderBy(x => x.Name).ToListAsync()).Select(x => new KPINewDto
            {
                Id = x.Id,
                ParentId = x.ParentId,
                Name = x.Name,
                PolicyId = x.PolicyId,
                UpdateBy = x.UpdateBy,
                Pic = x.Pic,
                TypeId = x.TypeId,
                Level = x.Level,
                PolicyName = _repoPolicy.FindAll().FirstOrDefault(y => y.Id == x.PolicyId).Name ?? "",
                TypeName = _repoType.FindAll().FirstOrDefault(y => y.Id == x.TypeId).Name ?? "",
                PICName = _repoAc.FindAll().FirstOrDefault(y => y.Id == x.Pic).FullName ?? "",
                UpdateName = _repoAc.FindAll().FirstOrDefault(y => y.Id == x.UpdateBy).FullName ?? "",
                FactId = _repoAc.FindAll().FirstOrDefault(y => y.Id == x.Pic).FactId ?? 0,
                CenterId = _repoAc.FindAll().FirstOrDefault(y => y.Id == x.Pic).CenterId ?? 0,
                DeptId = _repoAc.FindAll().FirstOrDefault(y => y.Id == x.Pic).DeptId ?? 0,
                UpdateDate = x.UpdateDate,
                //FactName =  _repoAc.FindAll().FirstOrDefault(y => y.Id == x.Pic).FactId == null || _repoAc.FindAll().FirstOrDefault(y => y.Id == x.Pic).FactId == 0 ? "N/A" : _repoOc.FindAll().FirstOrDefault(y => y.Id == _repoAc.FindAll().FirstOrDefault(y => y.Id == x.Pic).FactId).Name,
                //CenterName = _repoAc.FindAll().FirstOrDefault(y => y.Id == x.Pic).CenterId == null || _repoAc.FindAll().FirstOrDefault(y => y.Id == x.Pic).CenterId == 0 ? "N/A" : _repoOc.FindAll().FirstOrDefault(y => y.Id == _repoAc.FindAll().FirstOrDefault(y => y.Id == x.Pic).CenterId).Name,
                //DeptName = _repoAc.FindAll().FirstOrDefault(y => y.Id == x.Pic).DeptId == null || _repoAc.FindAll().FirstOrDefault(y => y.Id == x.Pic).DeptId == 0 ? "N/A" : _repoOc.FindAll().FirstOrDefault(y => y.Id == _repoAc.FindAll().FirstOrDefault(y => y.Id == x.Pic).DeptId).Name,

            }).ToList().OrderBy(x => x.PolicyId).ToList();
            var data = lists.Select(x => new KPINewDto
            {
                Id = x.Id,
                ParentId = x.ParentId,
                Name = x.Name,
                PolicyId = x.PolicyId,
                UpdateBy = x.UpdateBy,
                Pic = x.Pic,
                TypeId = x.TypeId,
                Level = x.Level,
                PolicyName = x.PolicyName,
                TypeName = x.TypeName,
                PICName = x.PICName,
                UpdateName = x.UpdateName,
                FactId = x.FactId,
                CenterId = x.CenterId,
                DeptId = x.DeptId,
                UpdateDate = x.UpdateDate,
                FactName = x.FactId == 0 ? "N/A" : _repoOc.FindById(x.FactId).Name,
                CenterName = x.CenterId == 0 ? "N/A" : _repoOc.FindById(x.CenterId).Name,
                DeptName = x.DeptId == 0 ? "N/A" : _repoOc.FindById(x.DeptId).Name
                //FactName =  _repoAc.FindAll().FirstOrDefault(y => y.Id == x.Pic).FactId == null || _repoAc.FindAll().FirstOrDefault(y => y.Id == x.Pic).FactId == 0 ? "N/A" : _repoOc.FindAll().FirstOrDefault(y => y.Id == _repoAc.FindAll().FirstOrDefault(y => y.Id == x.Pic).FactId).Name,
                //CenterName = _repoAc.FindAll().FirstOrDefault(y => y.Id == x.Pic).CenterId == null || _repoAc.FindAll().FirstOrDefault(y => y.Id == x.Pic).CenterId == 0 ? "N/A" : _repoOc.FindAll().FirstOrDefault(y => y.Id == _repoAc.FindAll().FirstOrDefault(y => y.Id == x.Pic).CenterId).Name,
                //DeptName = _repoAc.FindAll().FirstOrDefault(y => y.Id == x.Pic).DeptId == null || _repoAc.FindAll().FirstOrDefault(y => y.Id == x.Pic).DeptId == 0 ? "N/A" : _repoOc.FindAll().FirstOrDefault(y => y.Id == _repoAc.FindAll().FirstOrDefault(y => y.Id == x.Pic).DeptId).Name,

            }).ToList().AsHierarchy(x => x.Id, y => y.ParentId);
            return data;
        }

        public async Task<IEnumerable<HierarchyNode<KPINewDto>>> GetAllAsTreeView2nd3rd()
        {

            string token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            var accountId = JWTExtensions.GetDecodeTokenById(token).ToInt();

            var dataAc = _repoAc.FindById(accountId);
            var list = new List<KPINewDto>();
            var lists = (await _repo.FindAll().ProjectTo<KPINewDto>(_configMapper).OrderBy(x => x.Name).ToListAsync()).Select(x => new KPINewDto
            {
                Id = x.Id,
                ParentId = x.ParentId,
                Name = x.Name,
                PolicyId = x.PolicyId,
                UpdateBy = x.UpdateBy,
                Pic = x.Pic,
                CreateBy= x.CreateBy,
                LevelOcPolicy = x.LevelOcPolicy,
                LevelOcCreateBy = x.LevelOcCreateBy,
                OcIdCreateBy = x.OcIdCreateBy,
                TypeId = x.TypeId,
                Level = x.Level,
                PolicyName = _repoPolicy.FindAll().FirstOrDefault(y => y.Id == x.PolicyId).Name ?? "",
                TypeName = _repoType.FindAll().FirstOrDefault(y => y.Id == x.TypeId).Name ?? "",
                PICName = _repoAc.FindAll().FirstOrDefault(y => y.Id == x.Pic).FullName ?? "",
                UpdateName = _repoAc.FindAll().FirstOrDefault(y => y.Id == x.UpdateBy).FullName ?? "",
                FactId = _repoAc.FindAll().FirstOrDefault(y => y.Id == x.Pic).FactId ?? 0,
                CenterId = _repoAc.FindAll().FirstOrDefault(y => y.Id == x.Pic).CenterId ?? 0,
                DeptId = _repoAc.FindAll().FirstOrDefault(y => y.Id == x.Pic).DeptId ?? 0,
                UpdateDate = x.UpdateDate,
                

            }).ToList().OrderBy(x => x.PolicyId).ToList();
            if (dataAc.FactId > 0 && dataAc.CenterId == 0 && dataAc.DeptId == 0)
            {
                list = new List<KPINewDto>();
            }
            if (dataAc.FactId > 0 && dataAc.CenterId > 0 && dataAc.DeptId == 0)
            {
                List<int> OcIdUnder = _repoOc.FindAll(x => x.ParentId == dataAc.CenterId).Select(x => x.Id).ToList();
                var OcIdOver = _repoOc.FindAll().FirstOrDefault(x => x.Id == dataAc.CenterId).ParentId;
                var picOver = _repoAc.FindAll().FirstOrDefault(x => x.FactId == OcIdOver && x.CenterId == 0 && x.DeptId == 0).Id;
                var th1 = lists.Where(x => x.LevelOcCreateBy == 1 || x.LevelOcCreateBy == 2 || x.LevelOcCreateBy == 3).ToList();
                list = th1.Where(x => x.CreateBy == accountId || x.Pic == accountId || x.Pic == picOver || OcIdUnder.Contains(x.OcIdCreateBy.ToInt())).ToList();
            }
            if (dataAc.FactId > 0 && dataAc.CenterId > 0 && dataAc.DeptId > 0)
            {
                List<int> picOver = _repoAc.FindAll(x => x.FactId == dataAc.FactId && x.CenterId == dataAc.CenterId && x.DeptId == 0).Select(x => x.Id).ToList();
                list = lists.Where(x => x.CreateBy == accountId || x.Pic == accountId || picOver.Contains(x.Pic)).ToList();

                foreach (var item in list)
                {
                    if (item.Level == 2)
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
                PolicyId = x.PolicyId,
                UpdateBy = x.UpdateBy,
                Pic = x.Pic,
                TypeId = x.TypeId,
                CreateBy = x.CreateBy,
                Level = x.Level,
                PolicyName = x.PolicyName,
                TypeName = x.TypeName,
                PICName = x.PICName,
                UpdateName = x.UpdateName,
                FactId = x.FactId,
                CenterId = x.CenterId,
                DeptId = x.DeptId,
                UpdateDate = x.UpdateDate,
                FactName = x.FactId == 0 ? "N/A" : _repoOc.FindById(x.FactId).Name,
                CenterName = x.CenterId == 0 ? "N/A" : _repoOc.FindById(x.CenterId).Name,
                DeptName = x.DeptId == 0 ? "N/A" : _repoOc.FindById(x.DeptId).Name
                

            }).ToList().AsHierarchy(x => x.Id, y => y.ParentId);

            
            return data;
        }

        public async Task<object> GetAllType()
        {
            var data = _repoType.FindAll();
            return data;
        }

        public async Task<object> GetKPIByOcID(int ocID)
        {
            var data = _repo.FindAll(x => x.OcId == ocID).Select(x => new { 
                x.Id,
                x.Name,
                x.Pic,
                x.PolicyId,
                x.UpdateBy,
                x.TypeId,

                PolicyName = _repoPolicy.FindAll().FirstOrDefault(y => y.Id == x.PolicyId).Name ?? "",
                TypeName = _repoType.FindAll().FirstOrDefault(y => y.Id == x.TypeId).Name ?? "",
                PICName = _repoAc.FindAll().FirstOrDefault(y => y.Id == x.Pic).FullName ?? "",
                UpdateName = _repoAc.FindAll().FirstOrDefault(y => y.Id == x.UpdateBy).FullName ?? "",
                UpdateDate = x.UpdateDate.ToString("MM-dd-yyyy"),

                FactId = _repoAc.FindAll().FirstOrDefault(y => y.Id == x.Pic).FactId ?? 0,
                CenterId = _repoAc.FindAll().FirstOrDefault(y => y.Id == x.Pic).CenterId ?? 0,
                DeptId = _repoAc.FindAll().FirstOrDefault(y => y.Id == x.Pic).DeptId ?? 0,

                FactName =  _repoOc.FindAll().FirstOrDefault(y => y.Id == _repoAc.FindAll().FirstOrDefault(y => y.Id == x.Pic).FactId).Name ?? "N/A",
                CenterName = _repoOc.FindAll().FirstOrDefault(y => y.Id == _repoAc.FindAll().FirstOrDefault(y => y.Id == x.Pic).CenterId).Name ?? "N/A",
                DeptName = _repoOc.FindAll().FirstOrDefault(y => y.Id == _repoAc.FindAll().FirstOrDefault(y => y.Id == x.Pic).DeptId).Name ?? "N/A",
                //Mgmt = _re
            }).ToList();
            return data;
        }
        public override async Task<OperationResult> AddAsync(KPINewDto model)
        {
            try
            {
                string token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
                var accountId = JWTExtensions.GetDecodeTokenById(token).ToInt();
                var dataAc = _repoAc.FindById(accountId);
                var dataAcPo = _repoAc.FindById(model.Pic);
                var levelCreateBy = 0;
                if (dataAc.FactId > 0 && dataAc.CenterId == 0 && dataAc.DeptId == 0)
                {
                    levelCreateBy = 1;
                }
                if (dataAc.FactId > 0 && dataAc.CenterId > 0 && dataAc.DeptId == 0)
                {
                    levelCreateBy = 2;

                }
                if (dataAc.FactId > 0 && dataAc.CenterId > 0 && dataAc.DeptId > 0)
                {
                    levelCreateBy = 3;

                }
                var levelPic = 0;

                if (dataAcPo.FactId > 0 && dataAcPo.CenterId == 0 && dataAcPo.DeptId == 0)
                {
                    levelPic = 1;
                }
                if (dataAcPo.FactId > 0 && dataAcPo.CenterId > 0 && dataAcPo.DeptId == 0)
                {
                    levelPic = 2;
                }
                if (dataAcPo.FactId > 0 && dataAcPo.CenterId > 0 && dataAcPo.DeptId > 0)
                {
                    levelPic = 3;
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

                var ocIdPic = 0;

                if (dataAcPo.FactId > 0 && dataAcPo.CenterId == 0 && dataAcPo.DeptId == 0)
                {
                    ocIdPic = dataAcPo.FactId.ToInt();
                }
                if (dataAcPo.FactId > 0 && dataAcPo.CenterId > 0 && dataAcPo.DeptId == 0)
                {
                    ocIdPic = dataAcPo.CenterId.ToInt();
                }
                if (dataAcPo.FactId > 0 && dataAcPo.CenterId > 0 && dataAcPo.DeptId > 0)
                {
                    ocIdPic = dataAcPo.DeptId.ToInt();
                }
                model.OcIdCreateBy = ocIdCreate;
                model.OcIdPolicy = ocIdPic;
                model.LevelOcCreateBy = levelCreateBy;
                model.LevelOcPolicy = levelPic;
                model.CreateBy = accountId;
                model.UpdateBy = accountId;
                var item = _mapper.Map<KPINew>(model);
                item.UpdateDate = DateTime.Now;
                _repo.Add(item);
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

        public override async Task<OperationResult> UpdateAsync(KPINewDto model)
        {
            string token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            var accountId = JWTExtensions.GetDecodeTokenById(token).ToInt();
            try
            {
                var item = await _repo.FindByIdAsync(model.Id);
                item.Name = model.Name;
                item.PolicyId = model.PolicyId;
                item.TypeId = model.TypeId;
                item.Pic = model.Pic;
                item.UpdateBy = accountId;
                item.UpdateDate = DateTime.Now;
                _repo.Update(item);
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
        public async Task<object> GetPolicyByOcID(int ocID)
        {
            var levelOc = _repoOc.FindAll().FirstOrDefault(x => x.Id == ocID).Level;
            var parentofLevelOc = _repoOc.FindAll().FirstOrDefault(x => x.Id == ocID).ParentId;
            if (levelOc == 3)
            {
                return _repoOcPolicy.FindAll(x => x.OcId == ocID).Select(x => new {
                    x.Id,
                    x.PolicyId,
                    Name = _repoPolicy.FindAll().FirstOrDefault(y => y.Id == x.PolicyId).Name ?? ""
                }).ToList();
            } else
            {
                return _repoOcPolicy.FindAll(x => x.OcId == parentofLevelOc).Select(x => new { 
                    x.Id,
                    x.PolicyId,
                    Name = _repoPolicy.FindAll().FirstOrDefault(y => y.Id == x.PolicyId).Name ?? ""
                }).ToList();
            }
            //return data;
        }

        public async Task<bool> Delete(int id)
        {
            var item = _repo.FindById(id);
            var itemChild = _repo.FindAll().Where(x => x.ParentId == id).ToList();
            if (itemChild != null)
            {
                _repo.RemoveMultiple(itemChild);
               await _unitOfWork.SaveChangeAsync();
            }
            try
            {
                _repo.Remove(item);
                await _unitOfWork.SaveChangeAsync();
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
           
        }

        
    }
}
