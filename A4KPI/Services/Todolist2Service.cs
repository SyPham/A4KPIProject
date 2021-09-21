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
        Task<OperationResult> SubmitUpdatePDCA(PDCARequestDto model);
        Task<object> GetStatus();
        Task<object> L0(DateTime currentTime);
        Task<object> GetActionsForL0(int kpiNewId);


        Task<object> GetPDCAForL0(int kpiNewId, DateTime currentTime);
        Task<object> GetTargetForUpdatePDCA(int kpiNewId, DateTime currentTime);
        Task<object> GetActionsForUpdatePDCA(int kpiNewId, DateTime currentTime);
        Task<object> GetKPIForUpdatePDC(int kpiNewId, DateTime currentTime);



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
        private readonly IRepositoryBase<Models.Status> _repoStatus;
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
             IRepositoryBase<Models.Status> repoStatus,
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
            _repoStatus = repoStatus;
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
            var kpiModel = await _repoKPINew.FindAll(x => x.Id == kpiNewId).ProjectTo<KPINewDto>(_configMapper).FirstOrDefaultAsync();
            var policyModel = await _repoPolicy.FindAll(x => x.Id == kpiModel.PolicyId).ProjectTo<PolicyDto>(_configMapper).FirstOrDefaultAsync();

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
                TargetYTD = targetYTD // Target YTD	
            };


        }

        public async Task<object> GetActionsForUpdatePDCA(int kpiNewId, DateTime currentTime)
        {
            var nextMonth = currentTime.Month;
            var nextYear = currentTime.Year;
            var actions = await _repoAction.FindAll(x => x.KPIId == kpiNewId && x.CreatedTime.Year == nextYear && x.CreatedTime.Month >= nextMonth).ProjectTo<ActionDto>(_configMapper).ToListAsync();
            return new
            {
                Actions = actions,
            };

        }


        public async Task<object> GetKPIForUpdatePDC(int kpiNewId, DateTime currentTime)
        {
            var kpiModel = await _repoKPINew.FindAll(x => x.Id == kpiNewId).ProjectTo<KPINewDto>(_configMapper).FirstOrDefaultAsync();
            var policyModel = await _repoPolicy.FindAll(x => x.Id == kpiModel.PolicyId).ProjectTo<PolicyDto>(_configMapper).FirstOrDefaultAsync();
            var pic = await _repoAccount.FindAll(x => x.Id == kpiModel.Pic).ProjectTo<AccountDto>(_configMapper).FirstOrDefaultAsync();
            var kpi = kpiModel.Name;
            var policy = policyModel.Name;
      
            return new
            {
                Kpi = kpi,
                Policy = policy,
                Pic = pic.FullName,
            };

        }

        public async Task<object> GetPDCAForL0(int kpiNewId, DateTime currentTime)
        {
         
            var thisMonthResult = currentTime.Month == 1 ? 12 : currentTime.Month - 1;
            var thisYearResult = currentTime.Month == 1 ? currentTime.Year - 1 : currentTime.Year;
            var result = await _repoResult.FindAll(x => x.KPIId == kpiNewId && x.UpdateTime.Year == thisYearResult && x.UpdateTime.Month == thisMonthResult)
                .ProjectTo<ResultDto>(_configMapper)
                .FirstOrDefaultAsync();
            var model = from a in _repoAction.FindAll(x=>x.KPIId == kpiNewId && x.CreatedTime.Year == thisYearResult && x.CreatedTime.Month == thisMonthResult)
                        join b in _repoDo.FindAll() on a.Id equals b.ActionId into ab
                        from sub in ab.DefaultIfEmpty()
                        select new UpdatePDCADto
                        {
                            ActionId = a.Id,
                            DoId = sub == null ? 0 : sub.Id,
                            Content = a.Content,
                            DoContent = sub == null ? "" : sub.Content,
                            Achievement = sub == null ? "" : sub.Achievement,
                            Deadline = a.Deadline.HasValue ? a.Deadline.Value.ToString("MM/dd/yyyy") : "",
                            StatusId = a.StatusId,
                            Target = a.Target
                        };
            var data = await model.ToListAsync();
            return new
            {
                Data = data,
                Result = result
            };

        }

        public async Task<object> GetStatus()
        {
            return await _repoStatus.FindAll().ToListAsync();

        }


        public async Task<object> GetTargetForUpdatePDCA(int kpiNewId, DateTime currentTime)
        {
            var displayStatus = new List<int> { Constants.Status.Processing, Constants.Status.NotYetStart, Constants.Status.Postpone };
            var nextMonth = currentTime.Month;
            var nextYear = currentTime.Year;

            var nextMonthTarget = await _repoTarget.FindAll(x => x.KPIId == kpiNewId && x.TargetTime.Year == currentTime.Year && x.TargetTime.Month == currentTime.Month).ProjectTo<TargetDto>(_configMapper).FirstOrDefaultAsync();
            var thisMonth = currentTime.Month == 1 ? 12 : currentTime.Month - 1;
            var thisYear = currentTime.Month == 1 ? currentTime.Year - 1 : currentTime.Year;
            var target = await _repoTarget.FindAll(x => x.KPIId == kpiNewId && x.TargetTime.Year == thisYear && x.TargetTime.Month == thisMonth).ProjectTo<TargetDto>(_configMapper).FirstOrDefaultAsync();
          
            var targetYTD = await _repoTargetYTD.FindAll(x => x.KPIId == kpiNewId && x.CreatedTime.Year == thisYear).ProjectTo<TargetYTDDto>(_configMapper).FirstOrDefaultAsync();

            var thisMonthResult = currentTime.Month == 1 ? 12 : currentTime.Month - 1;
            var thisYearResult = currentTime.Month == 1 ? currentTime.Year - 1 : currentTime.Year;
           
            return new
            {
                ThisMonthYTD = target,
                ThisMonthPerformance = target,
                ThisMonthTarget = target,
                TargetYTD = targetYTD,
                NextMonthTarget = nextMonthTarget,
            };

        }

        public async Task<object> L0(DateTime currentTime)
        {
            var ct = currentTime;
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
                Type = "Action",
                CurrentTarget = false,
            }).ToListAsync();
            var latestMonth = ct.Month - 1;
            var month2 = currentTime.Month == 1 ? 12 : currentTime.Month - 1;
            var year = month2 == 1 ? currentTime.Year - 1 : currentTime.Year;
            var updatePDCA = await _repoKPINew.FindAll(x => x.Pic == accountId && x.Actions.Any()).Select(x => new
            {
                Id = x.Id,
                Topic = x.Name,
                Type = "UpdatePDCA",
                CurrentTarget =  x.Targets.Any(a => a.TargetTime.Year == year && a.TargetTime.Month == month2 && (a.Performance == 0 || !a.Submitted && a.Performance > 0)),
            }).Where(x => x.CurrentTarget).ToListAsync();

            // 
            var setting = await _repoSettingMonthly.FindAll(x => x.Month.Date <= ct).OrderByDescending(x => x.DisplayTime).FirstOrDefaultAsync();
            if (setting != null)
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
                }
                else
                {
                    var targetTime = new DateTime(currentTime.Year, currentTime.Month + 1, currentTime.Day);
                    target.TargetTime = targetTime;
                }
                var updateActions = _mapper.Map<List<Models.Action>>(updateActionList);
                var addActions = _mapper.Map<List<Models.Action>>(addActionList);
                if (target.Id > 0)
                    _repoTarget.Update(target);
                else
                {
                    target.Submitted = false;
                    _repoTarget.Add(target);
                }

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

        public async Task<OperationResult> SubmitUpdatePDCA(PDCARequestDto model)
        {
            var updateActionList = model.Actions.Where(x => x.Id > 0).ToList();
            var addActionList = model.Actions.Where(x => x.Id == 0).ToList();
            var updateActionForThisMonth = model.UpdatePDCA.ToList();

            try
            {
                var currentTime = model.CurrentTime;

                var targetYTD = _mapper.Map<TargetYTD>(model.TargetYTD);
                var target = _mapper.Map<Target>(model.Target);
                var nextMonthTarget = _mapper.Map<Target>(model.NextMonthTarget);
                nextMonthTarget.TargetTime = new DateTime(currentTime.Year, currentTime.Month, 1);

                var result = _mapper.Map<Result>(model.Result);

                if (currentTime.Month == 1)
                {
                    var updateTime = new DateTime(currentTime.Year - 1, 12, 1);
                    result.UpdateTime = updateTime;
                }
                else
                {
                    var updateTime = new DateTime(currentTime.Year, currentTime.Month - 1, 1);
                    result.UpdateTime = updateTime;

                }
                result.CreatedTime = model.CurrentTime;

                if (currentTime.Month == 1)
                {
                    var targetTime = new DateTime(currentTime.Year - 1, 12, 1);
                    target.TargetTime = targetTime;
                }
                else
                {
                    var targetTime = new DateTime(currentTime.Year, currentTime.Month - 1, 1);
                    target.TargetTime = targetTime;
                }
                var updateActions = _mapper.Map<List<Models.Action>>(updateActionList);
                var addActions = _mapper.Map<List<Models.Action>>(addActionList);
                if (target.Id > 0)
                    _repoTarget.Update(target);
                else
                    _repoTarget.Add(target);

                if (result.Id > 0)
                    _repoResult.Update(result);
                else
                    _repoResult.Add(result);

                if (nextMonthTarget.Id > 0)
                    _repoTarget.Update(nextMonthTarget);
                else
                    _repoTarget.Add(nextMonthTarget);

                _repoTargetYTD.Update(targetYTD);
                // dynamic currentime
                addActions.ForEach(item =>
                {
                    item.CreatedTime = model.CurrentTime;
                });
                _repoAction.AddRange(addActions);
                _repoAction.UpdateRange(updateActions);
                var updatethisMonthAction = new List<Models.Action>();
                var addDoList = new List<Do>();
                var updatedoList = new List<Do>();
                foreach (var item in updateActionForThisMonth)
                {
                    var action = await _repoAction.FindAll(x => x.Id == item.ActionId).FirstOrDefaultAsync();
                    action.StatusId = item.StatusId;
                    updatethisMonthAction.Add(action);

                    if (item.DoId == 0)
                        addDoList.Add(new Do(item.DoContent, item.Achievement, item.ActionId));
                    else
                    {
                        var doItem = await _repoDo.FindAll(x => x.Id == item.DoId).FirstOrDefaultAsync();
                        doItem.Content = item.DoContent;
                        doItem.Achievement = item.Achievement;
                        updatedoList.Add(doItem);
                    }

                }
                _repoAction.UpdateRange(updatethisMonthAction);
                _repoDo.AddRange(addDoList);
                _repoDo.UpdateRange(updatedoList);

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
