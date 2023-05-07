using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Sat.Recruitment.Core.IServices;
using Sat.Recruitment.Core.Services;
using Sat.Recruitment.Infraestructure.Common;
using Sat.Recruitment.Infraestructure.IRepository;
using Sat.Recruitment.Infraestructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sat.Recruitment.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen();

            services.AddScoped<IQueryableUnitOfWork>(s => s.GetRequiredService<RecruitmentDbContext>());
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IUserService, UserService>();

            services.AddDbContext<RecruitmentDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("ConnectionString"),
                    sqlServerOptionsAction =>
                    {
                        sqlServerOptionsAction.MigrationsHistoryTable("__MicroMigrationHistory", Configuration.GetConnectionString("SchemaName"));
                    });

                options.ConfigureWarnings(warnings =>
                {
                    warnings.Default(WarningBehavior.Log);
                });
            });

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", p => p.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().WithExposedHeaders());
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
            app.UseRouting();

            app.UseAuthorization();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseCors("AllowAll");
        }
    }
}
