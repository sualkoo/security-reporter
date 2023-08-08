﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using webapi.Login.Services;
using webapi.Login.Utils.Authorization;
using webapi.ProjectSearch.Services;
using webapi.Service;
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
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
                .Enrich.FromLogContext()
                // uncomment to write to Azure diagnostics stream
                //.WriteTo.File(
                //    @"D:\home\LogFiles\Application\identityserver.txt",
                //    fileSizeLimitBytes: 1_000_000,
                //    rollOnFileSizeLimit: true,
                //    shared: true,
                //    flushToDiskInterval: TimeSpan.FromSeconds(1))
                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}", theme: AnsiConsoleTheme.Code)
                .CreateLogger();
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