using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using ScoreKPI.Helpers;
using ScoreKPI.Installer;

namespace ScoreKPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddHttpContextAccessor();
            services.AddControllers()
             .AddNewtonsoftJson(options =>
             {
                 options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Unspecified;
                 options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                 //options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
             });

            services.InstallServicesInAssembly(Configuration);
            services.AddCors();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var swaggerOptions = new SwaggerOptions();
            Configuration.GetSection(nameof(SwaggerOptions)).Bind(swaggerOptions);
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors(x => x.AllowAnyHeader()
               .AllowAnyMethod()
               .AllowAnyOrigin());
            app.UseSwagger();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSwagger(option => { option.RouteTemplate = swaggerOptions.JsonRoute; });
            app.UseSwaggerUI(option => { option.SwaggerEndpoint(swaggerOptions.UIEndpoint, swaggerOptions.Description); });
            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
