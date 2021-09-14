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
    public interface IToDoList2Service
    {
        Task<OperationResult> SubmitAction(ActionRequestDto model);
        Task<object> L0(DateTime currentTime);
        Task<object> GetActionsForL0(int kpiNewId);



    }
    public class ToDoList2Service : IToDoList2Service
    {
        private readonly IRepositoryBase<Models.Action> _repoAction;
        private readonly IRepositoryBase<Do> _repoDo;
        private readonly IRepositoryBase<Policy> _repoPolicy;
        private readonly IRepositoryBase<KPINew> _repoKPINew;
        private readonly IRepositoryBase<TargetYTD> _repoTargetYTD;
        private readonly IRepositoryBase<Result> _repoResult;
        private readonly IRepositoryBase<Target> _repoTarget;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepositoryBase<Account> _repoAccount;
        private readonly IRepositoryBase<AccountGroupAccount> _repoAccountGroupAccount;
        private readonly IRepositoryBase<SettingMonthly> _repoSettingMonthly;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        private OperationResult operationResult;

        public ToDoList2Service(
             IRepositoryBase<Models.Action> repoAction,
             IRepositoryBase<Do> repoDo,
             IRepositoryBase<Policy> repoPolicy,
             IRepositoryBase<KPINew> repoKPINew,
             IRepositoryBase<TargetYTD> repoTargetYTD,
             IRepositoryBase<Result> repoResult,
             IRepositoryBase<Target> repoTarget,
             IHttpContextAccessor httpContextAccessor,
            IUnitOfWork unitOfWork,
                 IRepositoryBase<Account> repoAccount,
            IRepositoryBase<AccountGroupAccount> repoAccountGroupAccount,
             IRepositoryBase<SettingMonthly> repoSettingMonthly, IMapper mapper,
            MapperConfiguration configMapper
            )
        {
            _repoAction = repoAction;
            _repoDo = repoDo;
            _repoPolicy = repoPolicy;
            _repoKPINew = repoKPINew;
            _repoTargetYTD = repoTargetYTD;
            _repoResult = repoResult;
            _repoTarget = repoTarget;
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
            _repoAccount = repoAccount;
            _repoAccountGroupAccount = repoAccountGroupAccount;
            _repoSettingMonthly = repoSettingMonthly;
            _mapper = mapper;
            _configMapper = configMapper;
        }

        public async Task<object> GetActionsForL0(int kpiNewId)
        {
           
                var actions = await _repoAction.FindAll(x => x.KPIId == kpiNewId).ProjectTo<ActionDto>(_configMapper).ToListAsync();
                var kpiModel = await _repoKPINew.FindAll(x => x.Id == kpiNewId).FirstOrDefaultAsync();
                var policyModel = await _repoPolicy.FindAll(x => x.Id == kpiModel.PolicyId).FirstOrDefaultAsync();
             
                var pic = await _repoAccount.FindAll(x => x.Id == kpiModel.Pic).ProjectTo<AccountDto>(_configMapper).FirstOrDefaultAsync();
                var target = await _repoTarget.FindAll(x => x.KPIId == kpiNewId).ProjectTo<TargetDto>(_configMapper).FirstOrDefaultAsync();
                var targetYTD = await _repoTargetYTD.FindAll(x => x.KPIId == kpiNewId && x.CreatedTime.Year == DateTime.Now.Year).ProjectTo<TargetYTDDto>(_configMapper).FirstOrDefaultAsync();
                var kpi = kpiModel.Name;
                var policy = policyModel.Name;
                return new
                {
                    Actions = actions,
                    Kpi = kpi,
                    Policy = policy,
                    Pic = pic.FullName,
                    Target = target,
                    TargetYTD = targetYTD,
                };
            
         
        }

        public async Task<object> L0(DateTime currentTime)
        {
            var ct = DateTime.Now;
            var accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            int accountId = JWTExtensions.GetDecodeTokenById(accessToken);
            var account = await _repoAccount.FindAll(x => x.Id == accountId).FirstOrDefaultAsync();
            if (account == null) return null;


            var checkRole = await _repoAccountGroupAccount.FindAll(x => x.AccountId == accountId).Select(x => x.AccountGroup.Position).AnyAsync(x => SystemRole.L0 == x);
            if (checkRole == false) return null;
            var date = currentTime;
            var month = date.Month;
            //  && x.Actions.Any() == false
            var actions = await _repoKPINew.FindAll(x => x.Pic == accountId && x.Actions.Any() == false).Select(x => new
            {
                Id = x.Id,
                Topic = x.Name,
                Type = "Action"
            }).ToListAsync();

            var updatePDCA = await _repoKPINew.FindAll(x => x.Pic == accountId && x.Actions.Any()).Select(x => new
            {
                Id = x.Id,
                Topic = x.Name,
                Type = "UpdatePDCA"
            }).ToListAsync();
            var setting = await _repoSettingMonthly.FindAll(x => x.DisplayTime.Year == ct.Year && x.DisplayTime.Month == ct.Month).FirstOrDefaultAsync();
            if (setting.DisplayTime <= ct)
                return actions.Concat(updatePDCA);
            return actions;
        }


        public async Task<OperationResult> SubmitAction(ActionRequestDto model)
        {
            var updateActionList = model.Actions.Where(x => x.Id > 0).ToList();
            var addActionList = model.Actions.Where(x => x.Id == 0).ToList();

            try
            {
                
                var targetYTD = _mapper.Map<TargetYTD>(model.TargetYTD);
                var target = _mapper.Map<Target>(model.Target);
                var currentTime = DateTime.Now;
                if (currentTime.Month == 12)
                {
                    var targetTime = new DateTime(currentTime.Year + 1, 1, currentTime.Day);
                    target.TargetTime = targetTime;
                } else
                {
                    var targetTime = new DateTime(currentTime.Year, currentTime.Month+1, currentTime.Day);
                    target.TargetTime = targetTime;
                }
                var updateActions = _mapper.Map<List<Models.Action>>(updateActionList);
                var addActions = _mapper.Map<List<Models.Action>>(addActionList);
                if (target.Id > 0)
                    _repoTarget.Update(target);
                else
                    _repoTarget.Add(target);

                if (targetYTD.Id > 0)
                    _repoTargetYTD.Update(targetYTD);

                else
                    _repoTargetYTD.Add(targetYTD);
                _repoAction.AddRange(addActions);
                _repoAction.UpdateRange(updateActions);

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
