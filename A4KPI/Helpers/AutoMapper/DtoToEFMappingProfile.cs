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
    public class DtoToEFMappingProfile : Profile
    {
        public DtoToEFMappingProfile()
        {
            CreateMap<AccountDto, Account>()
                .ForMember(d => d.AccountType, o => o.Ignore());
            CreateMap<AccountTypeDto, AccountType>()
                .ForMember(d => d.Accounts, o => o.Ignore());
            CreateMap<AccountGroupDto, AccountGroup>();

            CreateMap<PeriodDto, Period>() ;
            CreateMap<PeriodReportTimeDto, PeriodReportTime>();
            CreateMap<PlanDto, Plan>();
            CreateMap<ObjectiveDto, Objective>();
            CreateMap<ProgressDto, Progress>();
            CreateMap<MailingDto, Mailing>();
            CreateMap<ToDoListDto, ToDoList>();
            CreateMap<AccountGroupPeriodDto, AccountGroupPeriod>()
                  .ForMember(d => d.AccountGroup, o => o.Ignore())
                .ForMember(d => d.Period, o => o.Ignore())
                ;
            CreateMap<ObjectiveRequestDto, Objective>();


            CreateMap<KPIDto, KPI>();
            CreateMap<AttitudeDto, Attitude>();
            CreateMap<ResultOfMonthDto, ResultOfMonth>();
            CreateMap<UserForDetailDto, Account>();

            CreateMap<KPIScoreDto, KPIScore>();

            CreateMap<AttitudeScoreDto, AttitudeScore>();
            CreateMap<CommentDto, Comment>();
            CreateMap<ContributionDto, Contribution>();
            CreateMap<PeriodTypeDto, PeriodType>();
            CreateMap<OCDto, OC>();
            CreateMap<AccountGroupAccountDto, AccountGroupAccount>();

            CreateMap<OcUserDto, OCUser>();
            CreateMap<SpecialContributionScoreDto, SpecialContributionScore>();
            CreateMap<SpecialScoreDto, SpecialScore>();
            CreateMap<SmartScoreDto, SmartScore>();
        }
    }
}
