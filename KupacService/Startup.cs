using KupacService.Data;
using KupacService.Data.Interfaces;
using KupacService.Entities.DataContext;
using KupacService.Helpers;
using KupacService.Model.OtherServices;
using KupacService.ServiceCalls;
using KupacService.ServiceCalls.Mocks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KupacService
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

            
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddScoped<IPrioritetRepository,PrioritetRepository>();
            services.AddScoped<IKontaktOsobaRepository, KontaktOsobaRepository>();
            services.AddScoped<IFizickoLiceRepository, FizickoLiceRepository>();
            services.AddScoped<IPravnoLiceRepository, PravnoLiceRepository>();
            services.AddScoped<IKupacRepository, KupacRepository>();
            services.AddScoped<ILoggerService,LoggerServiceMock>();
            services.AddScoped<IServiceCall<AdresaDto>,ServiceCallAdresaMock<AdresaDto>>();
            services.AddScoped<IServiceCall<OvlascenoLiceDto>, ServiceCallOvlascenoLiceMock<OvlascenoLiceDto>>();
            services.AddScoped<IServiceCall<UplataDto>, ServiceCallUplataMock<UplataDto>>();
            services.AddScoped<IKupacCalls, KupacCalls>();


            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "KupacService", Version = "v1" });
            });

            services.AddDbContext<KupacContext>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "KupacService v1"));
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
