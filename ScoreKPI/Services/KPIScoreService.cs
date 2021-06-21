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
    public interface IKPIScoreService : IServiceBase<KPIScore, KPIScoreDto>
    {
        Task<KPIScoreDto> GetFisrtByObjectiveId(int objectiveId, int scoreBy);
    }
    public class KPIScoreService : ServiceBase<KPIScore, KPIScoreDto>, IKPIScoreService
    {
        private readonly IRepositoryBase<KPIScore> _repo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        private OperationResult operationResult;

        public KPIScoreService(
            IRepositoryBase<KPIScore> repo,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            MapperConfiguration configMapper
            )
            : base(repo, unitOfWork, mapper, configMapper)
        {
            _repo = repo;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configMapper = configMapper;
        }
        public async Task<KPIScoreDto> GetFisrtByObjectiveId(int objectiveId, int scoreBy)
        {
            return await _repo.FindAll(x => x.ObjectiveId == objectiveId && scoreBy == x.ScoreBy).ProjectTo<KPIScoreDto>(_configMapper).FirstOrDefaultAsync();
        }
        /// <summary>
        /// Chỉnh sửa thành vừa cập nhật vừa thêm mới
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public override async Task<OperationResult> AddAsync(KPIScoreDto model)
        {
            if (model.Id > 0)
            {
                var item = await _repo.FindAll(x => x.Id == model.Id && x.ScoreBy == model.ScoreBy).AsNoTracking().FirstOrDefaultAsync();
                item.Point = model.Point;
                _repo.Update(item);
            }
            else
            {
                var itemList = _mapper.Map<KPIScore>(model);
                _repo.Add(itemList);
            }
            try
            {
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
