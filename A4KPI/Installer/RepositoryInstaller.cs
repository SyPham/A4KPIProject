using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using A4KPI.Data;
using A4KPI._Repositories.Interface;

namespace A4KPI.Installer
{
    public class RepositoryInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped(typeof(IRepositoryBase<>), typeof(IRepositoryBase<>));
        }
    }
}
