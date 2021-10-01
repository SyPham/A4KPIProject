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
        Task<object> GetPolicyByOcID(int ocID);
        Task<object> GetAllType();
        Task<bool> Delete(int id);
        Task<IEnumerable<HierarchyNode<KPINewDto>>> GetAllAsTreeView();
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
        public async Task<IEnumerable<HierarchyNode<KPINewDto>>> GetAllAsTreeView()
        {
            var lists = (await _repo.FindAll().ProjectTo<KPINewDto>(_configMapper).OrderBy(x => x.Name).ToListAsync()).Select(x => new KPINewDto {
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
                FactName = _repoAc.FindAll().FirstOrDefault(y => y.Id == x.Pic).FactId == null ? "N/A" : _repoOc.FindAll().FirstOrDefault(y => y.Id == _repoAc.FindAll().FirstOrDefault(y => y.Id == x.Pic).FactId).Name,
                CenterName = _repoAc.FindAll().FirstOrDefault(y => y.Id == x.Pic).CenterId == null ? "N/A" : _repoOc.FindAll().FirstOrDefault(y => y.Id == _repoAc.FindAll().FirstOrDefault(y => y.Id == x.Pic).CenterId).Name,
                DeptName = _repoAc.FindAll().FirstOrDefault(y => y.Id == x.Pic).DeptId == null ? "N/A" : _repoOc.FindAll().FirstOrDefault(y => y.Id == _repoAc.FindAll().FirstOrDefault(y => y.Id == x.Pic).DeptId).Name,

            }).ToList().AsHierarchy(x => x.Id, y => y.ParentId);
            return lists;
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
            try
            {
                var item = await _repo.FindByIdAsync(model.Id);
                item.Name = model.Name;
                item.PolicyId = model.PolicyId;
                item.TypeId = model.TypeId;
                item.Pic = model.Pic;
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
