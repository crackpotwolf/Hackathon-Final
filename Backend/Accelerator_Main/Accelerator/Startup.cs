using Data.Extensions.DI;
using Data.Services.DB;
using Data_Path.Models;
using Hangfire;
using Hangfire.Dashboard;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace Accelerator
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
            #region Базовая инициализация DI

            services.AddBaseModuleDI(Configuration.GetConnectionString("DefaultConnection"));

            #endregion

            #region Services

            services.Configure<PathConfig>(Configuration.GetSection("PathConfig"));
            services.Configure<ApiConfig>(Configuration.GetSection("ApiConfig"));

            #endregion

            #region Swagger

            // Текущее имя проекта
            var curProjectName = $"{Assembly.GetExecutingAssembly().GetName().Name}";

            // Swagger docs
            services.AddSwagger(curProjectName, c =>
            {
                c.AddXmlComments("Accelerator", curProjectName);
                c.AddXmlComments("Data", curProjectName);
                c.AddXmlComments("Data_Path", curProjectName);
            });

            #endregion

            #region Hangfire

            // Add Hangfire services
            services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UsePostgreSqlStorage(Configuration.GetConnectionString("HangfireConnection"), new PostgreSqlStorageOptions
                {
                    DistributedLockTimeout = TimeSpan.FromMinutes(1)
                }));

            // Add the processing server as IHostedService
            services.AddHangfireServer(options =>
            {
                options.WorkerCount = 5;
            });

            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
            IApiVersionDescriptionProvider provider,
            IWebHostEnvironment env,
            IOptions<PathConfig> pathConfig,
            InitDB InitDB)
        {
            if (env.EnvironmentName != "Development")
            {
                // Если папки нету
                if (!Directory.Exists(pathConfig.Value.UserPhotos))
                {
                    DirectoryInfo di = Directory.CreateDirectory(pathConfig.Value.UserPhotos);
                }

                app.UseStaticFiles(new StaticFileOptions
                {
                    FileProvider = new PhysicalFileProvider(pathConfig.Value.UserPhotos),
                    RequestPath = "/photos"
                });
            }

            app.UseBaseServices(env, provider);

            app.UseHangfireDashboard("/hangfire", new DashboardOptions()
            {
                Authorization = new[] { new AllowAllConnectionsFilter() },
                IgnoreAntiforgeryToken = true
            });

            #region Init


            #endregion
        }

        /// <summary>
        /// Проверка доступа для планировщика
        /// </summary>
        public class AllowAllConnectionsFilter : IDashboardAuthorizationFilter
        {
            public bool Authorize(DashboardContext context)
            {
                // Allow outside

                return true;
            }
        }
    }
}