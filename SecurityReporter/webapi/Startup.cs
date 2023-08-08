using Microsoft.AspNetCore.Authorization;
using webapi.Login.Utils.Authorization;
using webapi.ProjectSearch.Services;
using webapi.Service;
using webapi.Login.Utils;
using System.Data;
using Microsoft.OpenApi.Models;
using webapi.Login.Services;

namespace webapi
{
    public class Startup
    {
        public IWebHostEnvironment Environment { get; }

        public Startup(IWebHostEnvironment environment)
        {
            Environment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSingleton<ICosmosService, CosmosService>();
            services.AddSingleton<IProjectDataValidator, ProjectDataValidator>();
            services.AddSingleton<IProjectDataParser, ProjectDataParser>();
            services.AddSingleton<IProjectReportService, ProjectReportService>();
            services.AddSingleton<RoleService>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });
            });


            var builder = services.AddIdentityServer(options =>
            {
                options.EmitStaticAudienceClaim = true;
            })
                .AddInMemoryIdentityResources(Config.GetIdentityResources())
                .AddInMemoryClients(Config.GetClients())
                .AddInMemoryApiScopes(Config.GetApiScopes);

            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminCoordinatorPolicy", policy =>
                {
                    policy.Requirements.Add(new RoleRequirement("admin", "coordinator"));
                });

                options.AddPolicy("AdminPentesterPolicy", policy =>
                {
                    policy.Requirements.Add(new RoleRequirement("admin", "pentester"));
                });


                options.AddPolicy("AdminCoordinatorClientPolicy", policy =>
                {
                    policy.Requirements.Add(new RoleRequirement("admin", "coordinator", "client"));
                });

                options.AddPolicy("DefaultPolicy", policy =>
                {
                    policy.Requirements.Add(new RoleRequirement("default"));
                });
            });

            services.AddSingleton<IAuthorizationHandler, RoleAuthorizationHandler>();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseCors(builder => builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseRouting();

            app.UseIdentityServer();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API V1");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
