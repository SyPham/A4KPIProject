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
    public interface IResultOfMonthService: IServiceBase<ResultOfMonth, ResultOfMonthDto>
    {
        Task<List<ResultOfMonthDto>> GetAllByObjectiveId(int objectiveId);
        Task<OperationResult> UpdateResultOfMonthAsync(ResultOfMonthRequestDto model);

    }
    public class ResultOfMonthService : ServiceBase<ResultOfMonth, ResultOfMonthDto>, IResultOfMonthService
    {
        private OperationResult operationResult;

        private readonly IRepositoryBase<ResultOfMonth> _repo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        public ResultOfMonthService(
            IRepositoryBase<ResultOfMonth> repo, 
            IUnitOfWork unitOfWork,
            IMapper mapper, 
            MapperConfiguration configMapper
            )
            : base(repo, unitOfWork, mapper,  configMapper)
        {
            _repo = repo;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configMapper = configMapper;
        }
      
        public async Task<List<ResultOfMonthDto>> GetAllByObjectiveId(int objectiveId)
        {
            var month = DateTime.Now.Month;
            return await _repo.FindAll(x => x.ObjectiveId == objectiveId && month == x.Month).ProjectTo<ResultOfMonthDto>(_configMapper).ToListAsync();

        }
        public async Task<OperationResult> UpdateResultOfMonthAsync(ResultOfMonthRequestDto model)
        {
            var item = await _repo.FindByIdAsync(model.Id);
            item.Title = model.Title;
            _repo.Update(item);
            try
            {
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
