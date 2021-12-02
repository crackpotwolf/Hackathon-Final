using AutoMapper;
using Data.Configurations.Account;
using Data.Configurations.Swagger;
using Data.Interfaces.Repositories;
using Data.Middleware;
using Data.Repositories;
using Data.Services.Account;
using Data.Services.DB;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Search_Data.Services;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Data.Extensions.DI
{
    /// <summary>
    /// Сервисы
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Базовая инициализация сервисов для модулей
        /// </summary>
        /// <param name="services"></param>
        /// <param name="connectionString">Строка подключения к БД</param>
        public static void AddBaseModuleDI(this IServiceCollection services, string connectionString)
        {
            // DB
            services.AddContext(connectionString);

            // Routes
            services.AddControllersWithNewtonsoft();

            // Auth services
            services.AddJWTAuthentication();

            // CORS
            services.AddCors();

            // Other services (Email, UserManager, etc.)
            services.AddServices();

            #region AutoMapper

            // Auto Mapper Configurations
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);

            #endregion
        }

        /// <summary>
        /// Подключение Context
        /// </summary>
        /// <param name="services"></param>
        /// <param name="connectionString">Строка подключения к БД</param>
        private static void AddContext(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<AcceleratorContext>(options =>
            {
                options.UseNpgsql(connectionString,
                b =>
                {
                    b.MigrationsAssembly("Data");
                });
            });
        }

        /// <summary>
        /// Внедрение контроллеров и сериализацию Newtonsoft
        /// </summary>
        /// <param name="services"></param>
        private static void AddControllersWithNewtonsoft(this IServiceCollection services)
        {
            services.AddControllers(o =>
            {
                o.Conventions.Add(new ControllerDocumentationConvention());
                o.Conventions.Add(new RouteTokenTransformerConvention(new SlugifyParameterTransformer()));
            })
            .AddNewtonsoftJson(o =>
            {
                o.SerializerSettings.Converters.Add(new StringEnumConverter
                {
                    NamingStrategy = new CamelCaseNamingStrategy(),
                });
            });
        }

        /// <summary>
        /// Название Controller
        /// </summary>
        private class ControllerDocumentationConvention : IControllerModelConvention
        {
            /// <inheritdoc />
            void IControllerModelConvention.Apply(ControllerModel controller)
            {
                if (controller == null)
                    return;

                foreach (var attribute in controller.Attributes)
                {
                    if (attribute.GetType() == typeof(DisplayNameAttribute))
                    {
                        var routeAttribute = (DisplayNameAttribute)attribute;
                        if (!string.IsNullOrWhiteSpace(routeAttribute.DisplayName))
                            controller.ControllerName = routeAttribute.DisplayName;
                    }
                }
            }
        }

        /// <summary>
        /// Трансформатор пути запрос, пример: AccountSettings -> account-settings
        /// </summary>
        private class SlugifyParameterTransformer : IOutboundParameterTransformer
        {
            /// <inheritdoc />
            public string? TransformOutbound(object value)
            {
                // Slugify value
                return value == null ? null : Regex.Replace(value.ToString(), "([a-z])([A-Z])", "$1-$2").ToLower();
            }
        }

        /// <summary>
        /// Внедрение авторизации по JWT токенам
        /// </summary>
        /// <param name="services"></param>
        private static void AddJWTAuthentication(this IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = AuthOptions.ISSUER,

                        ValidateAudience = true,
                        ValidAudience = AuthOptions.AUDIENCE,
                        ValidateLifetime = true,

                        IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                        ValidateIssuerSigningKey = true,
                        RoleClaimType = "Role"
                    };
                });
        }

        /// <summary>
        /// Внедрение автогенерируемой документации Swagger
        /// </summary>
        /// <param name="services"></param>
        /// <param name="curProjectName"></param>
        public static void AddSwagger(this IServiceCollection services, string curProjectName, Action<SwaggerGenOptions> addXmlComments)
        {

            #region Swagger

            services.AddApiVersioning(
                options =>
                {
                    // options.ReportApiVersions = true;
                });

            services.AddVersionedApiExplorer(
                options =>
                {
                    options.GroupNameFormat = "'v'VVV";
                    options.SubstituteApiVersionInUrl = true;
                });

            services.AddSwaggerGenNewtonsoftSupport();
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

            services.AddSwaggerGen(c =>
            {
                c.DocumentFilter<EnumTypesDocumentFilter>();

                addXmlComments(c);

                c.EnableAnnotations();

                c.OperationFilter<SwaggerDefaultValues>();

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
                c.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();
            });

            #endregion
        }

        /// <summary>
        /// Добавить коментарии
        /// </summary>
        /// <param name="options"></param>
        /// <param name="externalProjectName"></param>
        public static void AddXmlComments(this SwaggerGenOptions options, string externalProjectName, string curProjectName)
        {
            // Имя файла с коментами для стороннего проекта
            var externalProjectXmlFile = $"{externalProjectName}.xml";

            // Путь для стороннего проекта
            var externalProjectPath = AppContext.BaseDirectory.Replace($"\\{curProjectName}\\", $"\\{externalProjectName}\\");

            // Путь к файлу стороннего проекта
            var externalProjectXmlFilePath = Path.Combine(externalProjectPath, externalProjectXmlFile);

            // Включить коментарии
            options.IncludeXmlComments(externalProjectXmlFilePath, true);

            // Добавить фильтр
            options.SchemaFilter<EnumTypesSchemaFilter>(externalProjectXmlFilePath);
        }

        /// <summary>
        /// Внедрение сервисов
        /// </summary>
        /// <param name="services"></param>
        private static void AddServices(this IServiceCollection services)
        {
            // Repositories
            services.AddTransient(typeof(IBaseEntityRepository<>), typeof(BaseEntityRepository<>));

            // Services
            services.AddTransient(typeof(InitDB), typeof(InitDB));
            services.AddTransient(typeof(UserManager), typeof(UserManager));
            services.AddTransient(typeof(EmailService), typeof(EmailService));
            services.AddTransient(typeof(IndicesManager), typeof(IndicesManager));
        }
    }
}