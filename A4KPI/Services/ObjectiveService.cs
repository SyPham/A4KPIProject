using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
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
    public interface IObjectiveService : IServiceBase<Objective, ObjectiveDto>
    {
        Task<OperationResult> PostAsync(ObjectiveRequestDto model);
        Task<OperationResult> PutAsync(ObjectiveRequestDto model);
       

        Task<List<ObjectiveDto>> GetAllKPIObjectiveByAccountId();
    }
    public class ObjectiveService : ServiceBase<Objective, ObjectiveDto>, IObjectiveService
    {
        private OperationResult operationResult;

        private readonly IRepositoryBase<Objective> _repo;
        private readonly IRepositoryBase<PIC> _repoPIC;
        private readonly IRepositoryBase<ResultOfMonth> _repoResultOfMonth;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly MapperConfiguration _configMapper;
        public ObjectiveService(
            IRepositoryBase<Objective> repo,
            IRepositoryBase<PIC> repoPIC,
            IRepositoryBase<ResultOfMonth> repoResultOfMonth,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            MapperConfiguration configMapper
            )
            : base(repo, unitOfWork, mapper, configMapper)
        {
            _repo = repo;
            _repoPIC = repoPIC;
            _repoResultOfMonth = repoResultOfMonth;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _configMapper = configMapper;
        }
        public override async Task<List<ObjectiveDto>> GetAllAsync()
        {

            return await _repo.FindAll().Include(x => x.Creator).ProjectTo<ObjectiveDto>(_configMapper).ToListAsync();
        }
        public async Task<List<ObjectiveDto>> GetAllKPIObjectiveByAccountId()
        {

            var accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            int accountID = JWTExtensions.GetDecodeTokenById(accessToken);
            return await _repo.FindAll().Where(x => x.CreatedBy == accountID).Include(x => x.Creator).ProjectTo<ObjectiveDto>(_configMapper).ToListAsync();
        }
        public async Task<OperationResult> PostAsync(ObjectiveRequestDto model)
        {
            var item = _mapper.Map<Objective>(model);
            var accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            int accountID = JWTExtensions.GetDecodeTokenById(accessToken);
            item.CreatedBy = accountID;
            _repo.Add(item);
            try
            {
                await _unitOfWork.SaveChangeAsync();
                var picList = new List<PIC>();
                foreach (var accountId in model.AccountIdList)
                {
                    var picItem = new PIC { AccountId = accountId, ObjectiveId = item.Id };
                    picList.Add(picItem);
                }
                _repoPIC.AddRange(picList);
                await _unitOfWork.SaveChangeAsync();

                operationResult = new OperationResult
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = MessageReponse.AddSuccess,
                    Success = true,
                    Data = item
                };
            }
            catch (Exception ex)
            {
                operationResult = ex.GetMessageError();
            }
            return operationResult;
        }

        public async Task<OperationResult> PutAsync(ObjectiveRequestDto model)
        {
            var item = await _repo.FindByIdAsync(model.Id);
            var accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            int accountID = JWTExtensions.GetDecodeTokenById(accessToken);
            item.CreatedBy = accountID;
            item.Date = model.Date;
            item.Topic = model.Topic;
            item.Status = model.Status;
            _repo.Update(item);
            try
            {
                await _unitOfWork.SaveChangeAsync();
                var deleteList = await _repoPIC.FindAll(x => x.ObjectiveId == item.Id).ToListAsync();
                _repoPIC.RemoveMultiple(deleteList);
                await _unitOfWork.SaveChangeAsync();
                var picList = new List<PIC>();
                foreach (var accountId in model.AccountIdList)
                {
                    var picItem = new PIC { AccountId = accountId, ObjectiveId = item.Id };
                    picList.Add(picItem);
                }
                _repoPIC.AddRange(picList);
                await _unitOfWork.SaveChangeAsync();

                operationResult = new OperationResult
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = MessageReponse.AddSuccess,
                    Success = true,
                    Data = item
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
