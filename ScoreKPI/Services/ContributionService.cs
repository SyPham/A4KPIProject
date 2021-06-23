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
    public interface IContributionService: IServiceBase<Contribution, ContributionDto>
    {
        Task<List<ContributionDto>> GetAllByObjectiveId(int objectiveId);
        Task<ContributionDto> GetFisrtByObjectiveId(int objectiveId, int createdBy);
    }
    public class ContributionService : ServiceBase<Contribution, ContributionDto>, IContributionService
    {
        private OperationResult operationResult;

        private readonly IRepositoryBase<Contribution> _repo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        public ContributionService(
            IRepositoryBase<Contribution> repo, 
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
        public async Task<ContributionDto> GetFisrtByObjectiveId(int objectiveId, int createdBy)
        {
            var currrentQuarter = (DateTime.Now.Month + 2) / 3;
            return await _repo.FindAll(x => x.Period == currrentQuarter && x.CreatedBy == createdBy).ProjectTo<ContributionDto>(_configMapper).FirstOrDefaultAsync();
        }
        public async Task<List<ContributionDto>> GetAllByObjectiveId(int objectiveId)
        {
            var currrentQuarter = (DateTime.Now.Month + 2) / 3;

            return await _repo.FindAll(x => x.Period == currrentQuarter).ProjectTo<ContributionDto>(_configMapper).ToListAsync();
        }
        /// <summary>
        /// Chỉnh sửa thành vừa cập nhật vừa thêm mới
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public override async Task<OperationResult> AddAsync(ContributionDto model)
        {
            if (model.Id > 0)
            {
                var item = await _repo.FindAll(x => x.Id == model.Id && x.CreatedBy == model.CreatedBy).AsNoTracking().FirstOrDefaultAsync();
                item.Content = model.Content;
                _repo.Update(item);
            }
            else
            {
                var itemList = _mapper.Map<Contribution>(model);
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
