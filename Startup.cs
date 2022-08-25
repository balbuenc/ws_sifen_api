using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoldenGateAPI.Data;
using GoldenGateAPI.Helpers;
using GoldenGateAPI.Repositories;
using GoldenGateAPI.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace GoldenGateAPI
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
          


            services.AddCors();
            services.AddControllers();

            // configure basic authentication 
            //services.AddAuthentication("BasicAuthentication")
            //    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

            // configure DI for application services
            
            //var sqlConnectionConfiguration = new SqlConfiguration(Configuration.GetConnectionString("ERPConnection"));
            //var sqlConnectionConfiguration = new SqlConfiguration(Configuration.GetConnectionString("WebConnectionString"));
            var sqlConnectionConfiguration = new SqlConfiguration(Configuration.GetConnectionString("SqlConnectionString"));
            services.AddSingleton(sqlConnectionConfiguration);

            services.AddScoped<IUserService, UserService>();
            //services.AddScoped<IOraFracctionRepository, OraFracctionRepository>();
            services.AddScoped<ISifenRepository, SifenRepository>();

            services.AddSwaggerGen();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();
            app.UseSwaggerUI(
                c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sifen API v1")
                ) ;

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            // global cors policy
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
