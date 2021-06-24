using AutoMapper;
using ScoreKPI.DTO;
using ScoreKPI.DTO.auth;
using ScoreKPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScoreKPI.Helpers.AutoMapper
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
            CreateMap<AccountGroupAccountDto, AccountGroupAccount>();


        }
    }
}
