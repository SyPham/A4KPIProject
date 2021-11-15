using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using A4KPI._Services.Services;
using A4KPI.Helpers;
using A4KPI.DTO;

namespace A4KPI.Installer
{
    public class ServiceInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {

            
            services.AddScoped<IMailExtension, MailExtension>();
            services.AddScoped<IMailHelper, MailHelper>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IAccountTypeService, AccountTypeService>();

            services.AddScoped<IPeriodService, PeriodService>();
            services.AddScoped<IPeriodReportTimeService, PeriodReportTimeService>();
            services.AddScoped<IAccountGroupPeriodService, AccountGroupPeriodService>();
            services.AddScoped<IAccountGroupService, AccountGroupService>();
            services.AddScoped<IPICService, PICService>();
            services.AddScoped<IResultOfMonthService, ResultOfMonthService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IOCService, OCService>();
            services.AddScoped<IPeriodTypeService, PeriodTypeService>();
            services.AddScoped<IAccountGroupAccountService, AccountGroupAccountService>();
            services.AddScoped<IOCPolicyService, OCPolicyService>();
            services.AddScoped<IOCNewService, OCNewService>();
            services.AddScoped<ITargetYTDService, TargetYTDService>();
            services.AddScoped<IMeetingService, MeetingService>();
            services.AddScoped<ISettingMonthService, SettingMonthService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IPermissionService, PermissionService>();
        }
    }
}
