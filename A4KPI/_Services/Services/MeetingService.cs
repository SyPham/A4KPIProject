using AutoMapper;
using A4KPI.Data;
using A4KPI.DTO;
using A4KPI.Models;
using A4KPI._Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;
using Microsoft.EntityFrameworkCore;
using AutoMapper.QueryableExtensions;
using A4KPI.Constants;
using A4KPI._Repositories.Interface;

namespace A4KPI._Services.Services
{
   
    public class MeetingService :  IMeetingService
    {
        private readonly IPICRepository _repo;
        private readonly IAccountRepository _repoAc;
        private readonly IKPINewRepository _repoKPINew;
        private readonly IDoRepository _repoDo;
        private readonly IResultRepository _repoResult;
        private readonly IActionStatusRepository _repoAcs;
        private readonly IOCRepository _repoOC;
        private readonly ITypeRepository _repoType;
        private readonly IPolicyRepository _repoPo;
        private readonly ITargetRepository _repoTarget;
        private readonly ITargetYTDRepository _repoTargetYTD;
        private readonly IActionRepository _repoAction;
        private readonly IStatusRepository _repoStatus;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        private readonly IKPIAccountRepository _repoKPIAc;
        public MeetingService(
            IPICRepository repo,
            IOCRepository repoOC,
            ITypeRepository repoType,
            IPolicyRepository repoPo,
            IKPIAccountRepository repoKPIAc,
            ITargetRepository repoTarget,
            ITargetYTDRepository repoTargetYTD,
            IActionStatusRepository repoAcs,
            IAccountRepository repoAc,
            IKPINewRepository repoKPINew,
            IDoRepository repoDo,
            IResultRepository repoResult,
            IActionRepository repoAction,
            IStatusRepository repoStatus,
            IMapper mapper, 
            MapperConfiguration configMapper
            )
        {
            _repo = repo;
            _repoOC = repoOC;
            _repoAc = repoAc;
            _repoAcs = repoAcs;
            _repoPo = repoPo;
            _repoKPIAc = repoKPIAc;
            _repoType = repoType;
            _repoTarget = repoTarget;
            _repoTargetYTD = repoTargetYTD;
            _repoKPINew = repoKPINew;
            _repoDo = repoDo;
            _repoResult = repoResult;
            _repoAction = repoAction;
            _repoStatus = repoStatus;
            _mapper = mapper;
            _configMapper = configMapper;
        }


       
        public async Task<object> GetAllKPI(int userId)
        {
            var data = new List<KPINewDto>();

            data = (await _repoKPINew.FindAll().OrderBy(x => x.Name).ToListAsync()).Select(x => new KPINewDto {

                Id = x.Id,
                Name = x.Name,
                TypeId = x.TypeId,
                TypeName = x.TypeId == 0 ? "" : _repoType.FindAll(y => y.Id == x.TypeId).FirstOrDefault().Name,
                Level = x.Level,
                TypeText = x.TypeId == 0 ? "" : _repoType.FindAll(y => y.Id == x.TypeId).FirstOrDefault().Description,
                PICName = _repoKPIAc.FindAll(y => y.KpiId == x.Id).ToList().Count > 0 ? String.Join(" , ", x.KPIAccounts.Select(x => _repoAc.FindById(x.AccountId).FullName)) : null,
                FactName = String.Join(" , ", x.KPIAccounts.Select(x => _repoOC.FindAll(y => y.Id == x.FactId).FirstOrDefault() != null ? _repoOC.FindById(x.FactId).Name : null).Where(x => !String.IsNullOrEmpty(x))),
                CenterName = String.Join(" , ", x.KPIAccounts.Select(x => _repoOC.FindAll(y => y.Id == x.CenterId).FirstOrDefault() != null ? _repoOC.FindById(x.CenterId).Name : null).Where(x => !String.IsNullOrEmpty(x))),
                DeptName = String.Join(" , ", x.KPIAccounts.Select(x => _repoOC.FindAll(y => y.Id == x.DeptId).FirstOrDefault() != null ? _repoOC.FindById(x.DeptId).Name : null).Where(x => !String.IsNullOrEmpty(x)))

            }).OrderBy(x => x.Level).ToList();
            var model = data.Select(x => new KPINewDto
            {
                Id = x.Id,
                Name = x.Name,
                PICName = x.PICName,
                TypeId = x.TypeId,
                Level = x.Level,
                TypeName = x.TypeName,
                TypeText = x.TypeText,
                FactName = x.FactName,
                CenterName = x.CenterName,
                DeptName = x.DeptName,

            }).Where(x => x.Level != Level.Level_1).OrderBy(x => x.Level).ToList();
            return model;
        }



        public async Task<ChartDtoDateTime> GetChartWithDateTime(int kpiId, DateTime currentTime)
        {
            var thisMonthResult = currentTime.Month == 1 ? 12 : currentTime.Month - 1;
            var thisYearResult = currentTime.Month == 1 ? currentTime.Year - 1 : currentTime.Year;
            var typeId = _repoKPINew.FindById(kpiId).TypeId;
            List<string> listLabels = new List<string>();
            List<int> listLabel = new List<int>();
            List<int> listLabelData = new List<int>();
            List<double> listTarget = new List<double>();
            List<double> listPerfomance = new List<double>();
            List<double> listYTD = new List<double>();
            var dataTable = new List<DataTable>();
            var kpiModel = await _repoKPINew.FindAll(x => x.Id == kpiId).FirstOrDefaultAsync();
            var parentKpi = await _repoKPINew.FindAll(x => x.Id == kpiModel.ParentId).ProjectTo<KPINewDto>(_configMapper).FirstOrDefaultAsync();
            var policy = parentKpi.Name;
            var data = await _repoTarget.FindAll(x => x.KPIId == kpiId && x.TargetTime.Year == thisYearResult).ToListAsync();

            for (int i = 1; i <= 12; i++)
            {
                listLabel.Add(i);
            }
            for (int i = 1; i <= thisMonthResult; i++)
            {
                listLabelData.Add(i);
            }

            foreach (var a in listLabel)
            {
                switch (a)
                {
                    case 1:
                        listLabels.Add("Jan");
                        break;
                    case 2:
                        listLabels.Add("Feb"); break;
                    case 3:
                        listLabels.Add("Mar"); break;
                    case 4:
                        listLabels.Add("Apr"); break;
                    case 5:
                        listLabels.Add("May");
                        break;
                    case 6:
                        listLabels.Add("Jun"); break;
                    case 7:
                        listLabels.Add("Jul"); break;
                    case 8:
                        listLabels.Add("Aug"); break;
                    case 9:
                        listLabels.Add("Sep");
                        break;
                    case 10:
                        listLabels.Add("Oct"); break;
                    case 11:
                        listLabels.Add("Nov"); break;
                    case 12:
                        listLabels.Add("Dec"); break;
                }
            }

            foreach (var item in listLabel)
            {
                var dataExist = data.Where(x => x.TargetTime.Month == item).ToList();
                if (dataExist.Count > 0)
                {
                    double dataTarget = data.FirstOrDefault(x => x.TargetTime.Month == item).Value;
                    listTarget.Add(dataTarget);

                }
                else
                {
                    listTarget.Add(0);
                }

            }

            foreach (var item in listLabel)
            {
                var dataExist = data.Where(x => x.TargetTime.Month == item).ToList();
                if (dataExist.Count > 0)
                {
                    var dataPerfomance = data.FirstOrDefault(x => x.TargetTime.Month == item).Performance;
                    listPerfomance.Add(dataPerfomance);

                }
                else
                {
                    listPerfomance.Add(0);
                }
            }

            foreach (var item in listLabel)
            {
                var dataExist = data.Where(x => x.TargetTime.Month == item).ToList();
                if (dataExist.Count > 0)
                {
                    double dataYTD = data.FirstOrDefault(x => x.TargetTime.Month == item).YTD;
                    listYTD.Add(dataYTD);

                }
                else
                {
                    listYTD.Add(0);
                }

            }

            double YTD = 0;
            var YTDs = _repoTargetYTD.FindAll(x => x.KPIId == kpiId).ToList();
            if (YTDs.Count > 0)
            {
                YTD = _repoTargetYTD.FindAll(x => x.KPIId == kpiId).FirstOrDefault().Value;
            }
            double TargetYTD = 0;

            var TargetYTDs = await _repoTarget.FindAll(x => x.KPIId == kpiId && x.TargetTime.Month == thisMonthResult && x.CreatedTime.Year == thisYearResult).ToListAsync();
            if (TargetYTDs.Count > 0)
            {
                TargetYTD = _repoTarget.FindAll(x => x.KPIId == kpiId && x.TargetTime.Month == thisMonthResult && x.CreatedTime.Year == thisYearResult).FirstOrDefault().YTD;
            }

            foreach (var item in listLabelData)
            {
                string content = null;
                var contentExist = await _repoResult.FindAll(x => x.KPIId == kpiId && x.CreatedTime.Month == item + 1).ToListAsync();
                if (contentExist.Count > 0)
                {
                    content = _repoResult.FindAll(x => x.KPIId == kpiId && x.CreatedTime.Month == item + 1).FirstOrDefault().Content.Trim();
                }
                //var thisMonthResults = currentTime.Month == 1 ? 12 : currentTime.Month - 1;
                var displayStatus = new List<int> { Constants.Status.Processing, Constants.Status.Processing, Constants.Status.NotYetStart, Constants.Status.Postpone };
                var model = new List<UpdatePDCADto>();
                var hideStatus = new List<int> { Constants.Status.Complete, Constants.Status.Terminate };

                //start tim lai list cong viec cua thang truoc chua lam xong
                var acs = (from a in _repoAction.FindAll(x => x.KPIId == kpiId && x.CreatedTime.Year == thisYearResult && x.CreatedTime.Month < item)
                        .Where(x =>
                         (x.ActionStatus.FirstOrDefault(c => hideStatus.Contains(c.StatusId)) == null && x.ActionStatus.Count > 0)
                        ||
                        (x.ActionStatus.FirstOrDefault(c => x.CreatedTime.Year == thisYearResult && x.CreatedTime.Month == item - 1 && !c.Submitted) != null)
                        || x.ActionStatus.Count == 0
                        )
                           select new UpdatePDCADto
                           {
                               ActionId = a.Id,
                               StatusId = a.ActionStatus.Any(x => x.CreatedTime.Year == thisYearResult && x.CreatedTime.Month == item - 1) ?
                               a.ActionStatus.FirstOrDefault(x => x.CreatedTime.Year == thisYearResult && x.CreatedTime.Month == item - 1).StatusId : null,
                               ActionStatusId = a.ActionStatus.Any(x => x.CreatedTime.Year == thisYearResult && x.CreatedTime.Month == item - 1) ?
                               a.ActionStatus.FirstOrDefault(x => x.CreatedTime.Year == thisYearResult && x.CreatedTime.Month == item - 1).Id : null
                           }).ToList();
                //end tim lai list cong viec cua thang truoc chua lam xong

                if (acs.Count > 0)
                {
                    model = (from a in _repoAction.FindAll(x => x.KPIId == kpiId && x.CreatedTime.Month == item)
                             join b in _repoDo.FindAll(x => x.CreatedTime.Month == item) on a.Id equals b.ActionId into ab
                             from sub in ab.DefaultIfEmpty()
                             select new UpdatePDCADto
                             {
                                 Month = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(item),
                                 ActionId = a.Id,
                                 DoId = sub == null ? 0 : sub.Id,
                                 Content = a.Content,
                                 CreatedTime = a.CreatedTime,
                                 DoContent = sub == null ? "" : sub.Content,
                                 ResultContent = sub == null ? "" : sub.ReusltContent,
                                 Achievement = sub == null ? "" : sub.Achievement,
                                 Deadline = a.Deadline.HasValue ? a.Deadline.Value.ToString("MM/dd") : "",
                                 StatusId = a.StatusId,
                                 StatusName = a.ActionStatus.FirstOrDefault(x => x.ActionId == a.Id && x.CreatedTime.Month <= item).Status.Name.Trim(),
                                 Target = a.Target,
                             }).ToList();
                    //add them list cong viec chua lam xong cua thang truoc vao thang hien tai

                    foreach (var itemAcs in acs)
                    {
                        model.Add(new UpdatePDCADto
                        {
                            Month = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(item),
                            ActionId = itemAcs.ActionId,
                            Content = _repoAction.FindAll(x => x.Id == itemAcs.ActionId).FirstOrDefault().Content,
                            CreatedTime = _repoAction.FindAll(x => x.Id == itemAcs.ActionId).FirstOrDefault().CreatedTime,
                            DoContent = _repoDo.FindAll().Where(x => x.ActionId == itemAcs.ActionId && x.CreatedTime.Month == item).ToList().Count == 0 ? "" : _repoDo.FindAll(x => x.ActionId == itemAcs.ActionId && x.CreatedTime.Month == item).FirstOrDefault().Content,
                            ResultContent = _repoDo.FindAll(x => x.ActionId == itemAcs.ActionId && x.CreatedTime.Month == item).ToList().Count == 0 ? "" : _repoDo.FindAll(x => x.ActionId == itemAcs.ActionId && x.CreatedTime.Month == item).FirstOrDefault().ReusltContent,
                            Achievement = _repoDo.FindAll(x => x.ActionId == itemAcs.ActionId && x.CreatedTime.Month == item).ToList().Count == 0 ? "" : _repoDo.FindAll(x => x.ActionId == itemAcs.ActionId && x.CreatedTime.Month == item).FirstOrDefault().Achievement,
                            Deadline = _repoAction.FindAll(x => x.Id == itemAcs.ActionId).FirstOrDefault().Deadline.Value.ToString("MM/dd"),
                            StatusName = _repoStatus.FindAll(x => x.Id == itemAcs.StatusId).ToList().Count == 0 ? "" : _repoStatus.FindAll(x => x.Id == itemAcs.StatusId).FirstOrDefault().Name.Trim(),
                            Target = _repoAction.FindAll(x => x.Id == itemAcs.ActionId).FirstOrDefault().Target,
                        });
                    }
                    //end
                }
                else
                {
                    model = (from a in _repoAction.FindAll(x => x.KPIId == kpiId && x.CreatedTime.Month == item)
                             join c in _repoAcs.FindAll(x => x.CreatedTime.Month == item) on a.Id equals c.ActionId
                             join b in _repoDo.FindAll(x => x.CreatedTime.Month == item) on a.Id equals b.ActionId into ab
                             from sub in ab.DefaultIfEmpty()
                             select new UpdatePDCADto
                             {
                                 Month = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(item),
                                 ActionId = a.Id,
                                 DoId = sub == null ? 0 : sub.Id,
                                 Content = a.Content,
                                 DoContent = sub == null ? "" : sub.Content,
                                 ResultContent = sub == null ? "" : sub.ReusltContent,
                                 Achievement = sub == null ? "" : sub.Achievement,
                                 Deadline = a.Deadline.HasValue ? a.Deadline.Value.ToString("MM/dd") : "",
                                 StatusId = a.StatusId,
                                 StatusName = a.ActionStatus.FirstOrDefault(x => x.ActionId == a.Id && a.CreatedTime.Month == item).Status.Name.Trim(),
                                 Target = a.Target,

                             }).ToList();
                }
                //var 
                var model2 = from a in _repoAction.FindAll(x => x.KPIId == kpiId && x.CreatedTime.Month == item + 1)
                             join b in _repoDo.FindAll(x => x.CreatedTime.Month == item + 1) on a.Id equals b.ActionId into ab
                             from sub in ab.DefaultIfEmpty()
                             select new UpdatePDCADto
                             {
                                 Month = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(item),
                                 ActionId = a.Id,
                                 DoId = sub == null ? 0 : sub.Id,
                                 Content = a.Content,
                                 DoContent = sub == null ? "" : sub.Content,
                                 ResultContent = sub == null ? "" : sub.ReusltContent,
                                 Achievement = sub == null ? "" : sub.Achievement,
                                 Deadline = a.Deadline.HasValue ? a.Deadline.Value.ToString("MM/dd") : "",
                                 StatusId = a.StatusId,
                                 StatusName = a.ActionStatus.FirstOrDefault(x => x.ActionId == a.Id && x.CreatedTime.Month == item).Status.Name.Trim(),
                                 Target = a.Target

                             };

                var dataAdd = new DataTable()
                {
                    Month = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(item),
                    CurrentMonthData = model.OrderBy(x => x.CreatedTime),
                    Content = content,
                    Date = $"{thisYearResult}/{item}/01",
                    KpiId = kpiId,
                    NextMonthData = model2
                };
                dataTable.Add(dataAdd);
            }


            return new ChartDtoDateTime
            {
                labels = listLabels.ToArray(),
                perfomances = listPerfomance.ToArray(),
                targets = listTarget.ToArray(),
                ytds = listYTD.ToArray(),
                YTD = YTD,
                TypeId = typeId,
                TargetYTD = TargetYTD,
                DataTable = dataTable,
                Policy = policy
            };
        }


    }
}
