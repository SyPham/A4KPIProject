using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using A4KPI.Helpers;
using System.IO;
using System;
using A4KPI.DTO;
using A4KPI._Services.Services;
using A4KPI.Data;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using A4KPI.Helpers.AutoMapper;
using System.Collections.Generic;
using Microsoft.OpenApi.Models;
using A4KPI._Repositories.Interface;
using A4KPI._Repositories.Repositories;

namespace A4KPI
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
            services.Configure<MailSettings>(Configuration.GetSection("MailSettings"));
            services.AddHttpContextAccessor();
            services.AddControllers()
             .AddNewtonsoftJson(options =>
             {
                 options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Unspecified;
                 options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
             });
            var connetionString = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlServer(connetionString,
                o => o.MigrationsAssembly("A4KPI"));
                options.UseLazyLoadingProxies();
            });

            services.AddAutoMapper(typeof(Startup))
                .AddScoped<IMapper>(sp =>
                {
                    return new Mapper(AutoMapperConfig.RegisterMappings());
                })
                .AddSingleton(AutoMapperConfig.RegisterMappings());

            //Swagger
            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Score KPI", Version = "v1" });

                var security = new Dictionary<string, IEnumerable<string>>
                {
                    {"Bearer", new string[0]}
                };
                x.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                x.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                });
            });

            services.AddCors();


            //Repository
            services.AddScoped<IAccountGroupPeriodRepository, AccountGroupPeriodRepository>();
            services.AddScoped<IAccountGroupAccountRepository, AccountGroupAccountRepository>();
            services.AddScoped<IAccountGroupRepository, AccountGroupRepository>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IDoRepository, DoRepository>();
            services.AddScoped<IFunctionTranslationRepository, FunctionTranslationRepository>();
            services.AddScoped<IKPIAccountRepository, KPIAccountRepository>();
            services.AddScoped<IKPINewRepository, KPINewRepository>();
            services.AddScoped<IOCAccountRepository, OCAccountRepository>();
            services.AddScoped<IOCNewRepository, OCNewRepository>();
            services.AddScoped<IOCPolicyRepository, OCPolicyRepository>();
            services.AddScoped<IOptionFunctionSystemRepository, OptionFunctionSystemRepository>();
            services.AddScoped<IOptionRepository, OptionRepository>();
            services.AddScoped<IPICRepository, PICRepository>();
            services.AddScoped<IPermissionRepository, PermissionRepository>();
            services.AddScoped<IPolicyRepository, PolicyRepository>();
            services.AddScoped<IResultOfMonthRepository, ResultOfMonthRepository>();
            services.AddScoped<IResultRepository, ResultRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IStatusRepository, StatusRepository>();
            services.AddScoped<ITargetPICRepository, TargetPICRepository>();
            services.AddScoped<ITargetRepository, TargetRepository>();
            services.AddScoped<ITargetYTDRepository, TargetYTDRepository>();
            services.AddScoped<ITypeRepository, TypeRepository>();
            services.AddScoped<IUserRoleRepository, UserRoleRepository>();
            services.AddScoped<IAccountTypeRepository, AccountTypeRepository>();
            services.AddScoped<IKPINewRepository, KPINewRepository>();
            services.AddScoped<IOCNewRepository, OCNewRepository>();
            services.AddScoped<IOCPolicyRepository, OCPolicyRepository>();
            services.AddScoped<IOCRepository, OCRepository>();
            services.AddScoped<IPeriodReportTimeRepository, PeriodReportTimeRepository>();
            services.AddScoped<IPeriodRepository, PeriodRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IUserRoleRepository, UserRoleRepository>();
            services.AddScoped<ISettingMonthRepository, SettingMonthRepository>();
            services.AddScoped<ITargetYTDRepository, TargetYTDRepository>();
            services.AddScoped<ITodolist2Repository, Todolist2Repository>();
            services.AddScoped<IActionStatusRepository, ActionStatusRepository>();
            services.AddScoped<IActionRepository, ActionRepository>();

            //Services
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
            services.AddScoped<IKPINewService, KPINewService>();
            services.AddScoped<IToDoList2Service, Todolist2Service>();

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
