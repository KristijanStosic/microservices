using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using PrijavaService.Data;
using PrijavaService.Data.Interfaces;
using PrijavaService.Entities.DataContext;
using PrijavaService.Helpers;
using PrijavaService.Models.Other;
using PrijavaService.ServiceCalls;
using PrijavaService.ServiceCalls.Mocks;
using System;
using System.Text.Json.Serialization;

namespace PrijavaService
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

            services.AddControllers(setup =>
            {
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "PrijavaService", Version = "v1" });
            });

            services.AddControllers().AddJsonOptions(x =>
                    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve);

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddScoped<IDokPravnaLicaRepository, DokPravnaLicaRepository>();
            services.AddScoped<IDokFizickaLicaRepository, DokFizickaLicaRepository>();
            services.AddScoped<IPrijavaRepository, PrijavaRepository>();

            services.AddScoped<IServiceCall<JavnoNadmetanjeDto>, ServiceCallJavnoNadmetanjeMock<JavnoNadmetanjeDto>>();


            services.AddScoped<IPrijavaCalls, PrijavaCalls>();

            services.AddDbContext<PrijavaContext>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PrijavaService v1"));
            }

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
