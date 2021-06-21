using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ScoreKPI.Services;

namespace ScoreKPI.Installer
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
            services.AddScoped<IResultOfMonthService, ResultOfMonthService>();
            services.AddScoped<IKPIScoreService, KPIScoreService>();
            services.AddScoped<IAuthService, AuthService>();
        }
    }
}
