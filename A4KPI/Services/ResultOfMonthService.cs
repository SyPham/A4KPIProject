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
    public interface IResultOfMonthService : IServiceBase<ResultOfMonth, ResultOfMonthDto>
    {
        Task<List<ResultOfMonthDto>> GetAllByMonth(int objectiveId, DateTime currentTime);
        Task<OperationResult> UpdateResultOfMonthAsync(ResultOfMonthRequestDto model);

    }
    public class ResultOfMonthService : ServiceBase<ResultOfMonth, ResultOfMonthDto>, IResultOfMonthService
    {
        private OperationResult operationResult;

        private readonly IRepositoryBase<ResultOfMonth> _repo;
        private readonly IRepositoryBase<ToDoList> _repoToDoList;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly MapperConfiguration _configMapper;
        public ResultOfMonthService(
            IRepositoryBase<ResultOfMonth> repo,
            IRepositoryBase<ToDoList> repoToDoList,
            IUnitOfWork unitOfWork,
            IMapper mapper,
             IHttpContextAccessor httpContextAccessor,
            MapperConfiguration configMapper
            )
            : base(repo, unitOfWork, mapper, configMapper)
        {
            _repo = repo;
            _repoToDoList = repoToDoList;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _configMapper = configMapper;
        }

        public async Task<List<ResultOfMonthDto>> GetAllByMonth(int objectiveId, DateTime currentTime)
        {
            var month = currentTime.Month;
            var accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            int accountId = JWTExtensions.GetDecodeTokenById(accessToken);
            return await _repo.FindAll(x => x.CreatedBy == accountId && objectiveId == x.ObjectiveId && month == x.Month).ProjectTo<ResultOfMonthDto>(_configMapper).ToListAsync();

        }
        public async Task<OperationResult> UpdateResultOfMonthAsync(ResultOfMonthRequestDto model)
        {
            try
            {
                var item = await _repo.FindByIdAsync(model.Id);
                if (item == null)
                {
                    _repo.Add(new ResultOfMonth
                    {
                        Month = model.Period,
                        Title = model.Title,
                        ObjectiveId = model.ObjectiveId,
                        CreatedBy = model.CreatedBy
                    });
                }
                else
                {
                    item.ObjectiveId = model.ObjectiveId;
                    item.Title = model.Title;
                    item.CreatedBy = model.CreatedBy;
                    _repo.Update(item);
                }

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
    }
}
