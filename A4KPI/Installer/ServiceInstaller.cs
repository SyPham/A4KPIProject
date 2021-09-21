using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using A4KPI.Services;

namespace A4KPI.Installer
{
    public class ServiceInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IAccountTypeService, AccountTypeService>();

            services.AddScoped<IPeriodService, PeriodService>();
            services.AddScoped<IPeriodReportTimeService, PeriodReportTimeService>();

            services.AddScoped<IMailingService, MailingService>();
            services.AddScoped<IProgressService, ProgressService>();

            services.AddScoped<IPlanService, PlanService>();
            services.AddScoped<IToDoListService, ToDoListService>();

            services.AddScoped<IAccountGroupPeriodService, AccountGroupPeriodService>();
            services.AddScoped<IAccountGroupService, AccountGroupService>();
            services.AddScoped<IObjectiveService, ObjectiveService>();
            services.AddScoped<IPICService, PICService>();

            services.AddScoped<IKPIService, KPIService>();
            services.AddScoped<IAttitudeService, AttitudeService>();
            services.AddScoped<IAttitudeScoreService, AttitudeScoreService>();
            services.AddScoped<IResultOfMonthService, ResultOfMonthService>();
            services.AddScoped<IKPIScoreService, KPIScoreService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<IContributionService, ContributionService>();
            services.AddScoped<IOCService, OCService>();
            services.AddScoped<IPeriodTypeService, PeriodTypeService>();
            services.AddScoped<IAccountGroupAccountService, AccountGroupAccountService>();
            services.AddScoped<ISpecialContributionScoreService, SpecialContributionScoreService>();
            services.AddScoped<ISpecialScoreService, SpecialScoreService>();
            services.AddScoped<ISmartScoreService, SmartScoreService>();
            services.AddScoped<IPerformanceService, PerformanceService>();
            services.AddScoped<IQ1Q3Service, Q1Q3Service>();
            services.AddScoped<IH1H2Service, H1H2Service>();
            services.AddScoped<IGHRService, GHRService>();
            services.AddScoped<IHQHRService, HQHRService>();
            services.AddScoped<IScoreService, ScoreService>();
            services.AddScoped<IOCPolicyService, OCPolicyService>();
            services.AddScoped<IOCNewService, OCNewService>();
            services.AddScoped<IKPINewService, KPINewService>();
            services.AddScoped<IToDoList2Service, ToDoList2Service>();
            services.AddScoped<ITargetYTDService, TargetYTDService>();
            services.AddScoped<IMeetingService, MeetingService>();
        }
    }
}
