﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
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
        Task<KPIScoreDto> GetFisrtByAccountId(int accountId, int periodTypeId, int period, string scoreType);
        Task<KPIScoreDto> GetFisrtSelfScoreByAccountId(int accountId, int periodTypeId, int period, string scoreType);
    }
    public class KPIScoreService : ServiceBase<KPIScore, KPIScoreDto>, IKPIScoreService
    {
        private readonly IRepositoryBase<KPIScore> _repo;
        private readonly IRepositoryBase<PeriodType> _repoPeriodType;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly MapperConfiguration _configMapper;
        private OperationResult operationResult;

        public KPIScoreService(
            IRepositoryBase<KPIScore> repo,
            IRepositoryBase<PeriodType> repoPeriodType,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            MapperConfiguration configMapper
            )
            : base(repo, unitOfWork, mapper, configMapper)
        {
            _repo = repo;
            _repoPeriodType = repoPeriodType;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _configMapper = configMapper;
        }
        public async Task<KPIScoreDto> GetFisrtByAccountId(int accountId, int periodTypeId, int period, string scoreType)
        {
            var accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            int scoreBy = JWTExtensions.GetDecodeTokenById(accessToken);

            return await _repo.FindAll(x => 
                                        x.ScoreType == scoreType 
                                        && x.PeriodTypeId == periodTypeId 
                                        && x.CreatedTime.Year == DateTime.Today.Year 
                                        && x.Period == period
                                        && accountId == x.AccountId 
                                        && scoreBy == x.ScoreBy 
                                        && x.AccountId != scoreBy)
                                    .ProjectTo<KPIScoreDto>(_configMapper)
                                    .FirstOrDefaultAsync();
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

        public async Task<KPIScoreDto> GetFisrtSelfScoreByAccountId(int scoreBy, int periodTypeId, int period, string scoreType)
        {
            var accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            int accountID = JWTExtensions.GetDecodeTokenById(accessToken);
         
            // tu cham cho minh
            return await _repo.FindAll(x =>  
                                    x.PeriodTypeId == periodTypeId 
                                    && x.CreatedTime.Year == DateTime.Today.Year 
                                    && x.Period == period 
                                    && x.ScoreBy == scoreBy
                                    && x.ScoreType == scoreType
                                    && x.AccountId == scoreBy
                                    && x.AccountId == accountID
                                ).ProjectTo<KPIScoreDto>(_configMapper).FirstOrDefaultAsync();
        }
    }
}
