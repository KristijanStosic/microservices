using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using UgovorOZakupu.Data.RokDospeca;
using UgovorOZakupu.Data.TipGarancije;
using UgovorOZakupu.Data.UgovorOZakupu;
using UgovorOZakupu.Data.UnitOfWork;
using UgovorOZakupu.DbContext;
using UgovorOZakupu.Models.Dokument;
using UgovorOZakupu.Models.JavnoNadmetanje;
using UgovorOZakupu.Models.Kupac;
using UgovorOZakupu.Services;
using UgovorOZakupu.Services.Logger;
using UgovorOZakupu.Services.ServiceCalls;

namespace UgovorOZakupu
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<UgovorOZakupuDbContext>();
         
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ITipGaranceijeRepository, TipGaranceijeRepository>();
            services.AddScoped<IRokDospecaRepository, RokDospecaRepository>();
            services.AddScoped<IUgovorOZakupuRepository, UgovorOZakupuRepository>();

            // services.AddScoped<ILoggerService, LoggerService>();
            services.AddScoped<ILoggerService, LoggerServiceMock>();

            services.AddScoped<IService<DokumentDto>, Service<DokumentDto>>();
            services.AddScoped<IService<JavnoNadmetanjeDto>, Service<JavnoNadmetanjeDto>>();
            services.AddScoped<IService<KupacDto>, Service<KupacDto>>();
            
            // services.AddScoped<IServiceCalls, ServiceCalls>();
            services.AddScoped<IServiceCalls, ServiceCallsMock>();
            
            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "UgovorOZakupu", Version = "v1"});
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "UgovorOZakupu v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}