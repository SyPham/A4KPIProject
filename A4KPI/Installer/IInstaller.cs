using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace A4KPI.Installer
{
    public interface IInstaller
    {
        void InstallServices(IServiceCollection services, IConfiguration configuration);
    }
}
