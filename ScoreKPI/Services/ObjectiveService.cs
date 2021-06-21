using AutoMapper;
using AutoMapper.QueryableExtensions;
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
    public interface IObjectiveService: IServiceBase<Objective, ObjectiveDto>
    {
        Task<OperationResult> PostAsync(ObjectiveRequestDto model);
        Task<OperationResult> PutAsync(ObjectiveRequestDto model);
    }
    public class ObjectiveService : ServiceBase<Objective, ObjectiveDto>, IObjectiveService
    {
        private OperationResult operationResult;

        private readonly IRepositoryBase<Objective> _repo;
        private readonly IRepositoryBase<PIC> _repoPIC;
        private readonly IRepositoryBase<ResultOfMonth> _repoResultOfMonth;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        public ObjectiveService(
            IRepositoryBase<Objective> repo, 
            IRepositoryBase<PIC> repoPIC,
            IRepositoryBase<ResultOfMonth> repoResultOfMonth,
            IUnitOfWork unitOfWork,
            IMapper mapper, 
            MapperConfiguration configMapper
            )
            : base(repo, unitOfWork, mapper,  configMapper)
        {
            _repo = repo;
            _repoPIC = repoPIC;
            _repoResultOfMonth = repoResultOfMonth;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configMapper = configMapper;
        }
      
        public async Task<OperationResult> PostAsync(ObjectiveRequestDto model)
        {
            var item = _mapper.Map<Objective>(model);
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
                //throw new Exception();
                var resultOfMonthList = new List<ResultOfMonth> { };
                await _unitOfWork.SaveChangeAsync();

                for (int i = 1; i <= 12; i++)
                {
                    resultOfMonthList.Add(new ResultOfMonth
                    {
                        Month = i,
                        ObjectiveId = item.Id
                    });
                }
                
                _repoResultOfMonth.AddRange(resultOfMonthList);
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
