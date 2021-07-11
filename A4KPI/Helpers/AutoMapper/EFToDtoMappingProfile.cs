using AutoMapper;
using A4KPI.DTO;
using A4KPI.DTO.auth;
using A4KPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A4KPI.Helpers.AutoMapper
{
    public class EFToDtoMappingProfile : Profile
    {
        public EFToDtoMappingProfile()
        {

            CreateMap<Account, AccountDto>()
                .ForMember(d => d.AccountGroupText, o => o.MapFrom(s => s.AccountGroupAccount.Count > 0 ? String.Join(",", s.AccountGroupAccount.Select(x=>x.AccountGroup.Name)) : ""))
                .ForMember(d => d.AccountGroupIds, o => o.MapFrom(s => s.AccountGroupAccount.Select(x=>x.AccountGroup.Id) ))
                ;
            CreateMap<AccountType, AccountTypeDto>();
            CreateMap<AccountGroup, AccountGroupDto>();
            CreateMap<Objective, ObjectiveDto>()
                 .ForMember(d => d.Creator, o => o.Ignore())
                .ForMember(d => d.AccountIdList, o => o.MapFrom(s => s.PICs.Count > 0 ? s.PICs.Select(x=>x.AccountId) : s.PICs.Select(x => x.AccountId)))
                .ForMember(d => d.Accounts, o => o.MapFrom(s => s.PICs.Count > 0 ?  String.Join(",",s.PICs.Select(x=>x.Account.FullName).ToArray()): "" ))
                ;

            CreateMap<PeriodReportTime, PeriodReportTimeDto>();
            CreateMap<Plan, PlanDto>();
            CreateMap<Progress, ProgressDto>();
            CreateMap<Mailing, MailingDto>();
            CreateMap<ToDoList, ToDoListDto>();
            CreateMap<AccountGroupPeriod, AccountGroupPeriodDto>();
            CreateMap<Objective, ObjectiveRequestDto>();

            CreateMap<KPI, KPIDto>();
            CreateMap<Attitude, AttitudeDto>();
            CreateMap<ResultOfMonth, ResultOfMonthDto>();
            CreateMap<Account, UserForDetailDto>();

            CreateMap<KPIScore, KPIScoreDto>();
            CreateMap<AttitudeScore, AttitudeScoreDto>();
            CreateMap<Comment, CommentDto>();
            CreateMap<Contribution, ContributionDto>();
            CreateMap<PeriodType, PeriodTypeDto>();
            CreateMap<OC, OCDto>();
            CreateMap<Period, PeriodDto>();
            CreateMap<AccountGroupAccount, AccountGroupAccountDto>();

            CreateMap<OCUser, OcUserDto>();
            CreateMap<SpecialContributionScore, SpecialContributionScoreDto>();
            CreateMap<SpecialScore, SpecialScoreDto>();
            CreateMap<SmartScore, SmartScoreDto>();

        }
    }
}
