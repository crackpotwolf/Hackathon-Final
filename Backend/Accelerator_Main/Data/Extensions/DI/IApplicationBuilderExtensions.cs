using Data.Extensions.Reflection;
using Data.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Data.Extensions.DI
{
    /// <summary>
    /// Middleware
    /// </summary>
    public static class IApplicationBuilderExtensions
    {
        /// <summary>
        /// Использование базовых сервисов 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="provider"></param>
        /// <param name="env"></param>
        public static void UseBaseServices(this IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseJWTAuthentication();

            #region Middleware

            app.UseMiddleware<LoggingMiddleware>();

            #endregion

            #region Swagger

            app.UseSwaggerService(provider);

            #endregion

            app.UseCors(builder =>
            {
                builder.AllowAnyOrigin();
                builder.AllowAnyMethod();
                builder.AllowAnyHeader();
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller:slugify}/{action:slugify}/{id?}");
            });
        }

        /// <summary>
        /// Применение миграций БД
        /// </summary>
        /// <param name="app"></param>
        public static void MigrateDatabase(this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();

            serviceScope.ServiceProvider.GetRequiredService<AcceleratorContext>().Database.Migrate();
        }

        /// <summary>
        /// Внедрнение автогенерируемой документации API - Swagger
        /// </summary>
        /// <param name="app"></param>
        /// <param name="provider"></param>
        private static void UseSwaggerService(this IApplicationBuilder app, IApiVersionDescriptionProvider provider)
        {
            var isDevelopment = app.ApplicationServices.GetService<IWebHostEnvironment>().IsDevelopment();
            string prefixApi = isDevelopment ? "" : $"/api/{Assembly.GetEntryAssembly().GetName().GetNameDashCase()}";

            app.UseSwagger();

            app.UseSwaggerUI(
                options =>
                {
                    foreach (var description in provider.ApiVersionDescriptions)
                    {
                        options.SwaggerEndpoint(
                            $"{prefixApi}/swagger/{description.GroupName}/swagger.json",
                            description.GroupName.ToUpperInvariant());
                    }
                    options.DocExpansion(DocExpansion.None);
                });
        }

        /// <summary>
        /// Использование JWT токенов
        /// </summary>
        /// <param name="app"></param>
        private static void UseJWTAuthentication(this IApplicationBuilder app)
        {
            app.UseAuthentication();
            app.UseAuthorization();
        }
    }
}