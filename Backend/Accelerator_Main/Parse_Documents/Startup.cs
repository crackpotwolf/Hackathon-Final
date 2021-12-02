using Data.Extensions.DI;
using Data.Services.DB;
using Data;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Search_Data.Models;
using Data_Path.Models;
using System.Reflection;

namespace Parse_Documents
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

            #endregion

            #region Swagger

            // Текущее имя проекта
            var curProjectName = $"{Assembly.GetExecutingAssembly().GetName().Name}";

            // Swagger docs
            services.AddSwagger(curProjectName, c =>
            {
                c.AddXmlComments("Data", curProjectName);
                c.AddXmlComments("Parse_Documents", curProjectName);
                c.AddXmlComments("Data_Path", curProjectName);
            });

            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
            IApiVersionDescriptionProvider provider,
            IWebHostEnvironment env,
            InitDB InitDB)
        {
            app.UseBaseServices(env, provider);

            #region Init


            #endregion
        }
    }
}