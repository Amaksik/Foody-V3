using BAL.Interfaces;
using BAL.Options;
using BAL.Services;
using DAL;
using DAL.EF_Core;
using DAL.Interfaces;
using DAL.Repositories;
using Foody_V3.Controllers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Foody_V3
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

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Foody_V3", Version = "v1" });
            });


            //// use in-memory database
            //services.AddDbContext<AspnetRunContext>(c =>
            //    c.UseInMemoryDatabase("DBConnection"));

            // use real database

            string mySqlConnectionStr = Configuration["ConnectionStrings:DefaultConnection"]; //Configuration.GetConnectionString("DatabaseSettings:DefaultConnection");
            services.AddDbContextPool<Context>(options => options.UseMySql(mySqlConnectionStr, ServerVersion.AutoDetect(mySqlConnectionStr)));
            
            //services.AddDbContext<Context>(options => options.UseMySql(mySqlConnectionStr, 
            //                                                           ServerVersion.AutoDetect(mySqlConnectionStr)));





            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();

            services.AddOptions<RecognitionServiceOptions>()
                .Bind(Configuration.GetSection(RecognitionServiceOptions.Tokens))
                .ValidateDataAnnotations();

            services.AddScoped<IRecognitionService, RecognitionService>();
            services.AddScoped<UsersController>();
            services.AddScoped<MealsController>();
            services.AddScoped<UsersServiceController>();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Foody_V3 v1"));
            

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
