using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using UgovorOZakupu.Data.UgovorOZakupu;
using UgovorOZakupu.Data.UnitOfWork;
using UgovorOZakupu.DbContext;
using UgovorOZakupu.Models.Dokument;
using UgovorOZakupu.Models.JavnoNadmetanje;
using UgovorOZakupu.Models.Kupac;
using UgovorOZakupu.Models.Licnost;
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

            // services.AddScoped<ILoggerService, LoggerService>();
            services.AddScoped<ILoggerService, LoggerServiceMock>();

            // services.AddScoped<IServiceCalls, ServiceCalls>();
            services.AddScoped<IServiceCalls, ServiceCallsMock>();
            
            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(
                    "v1",
                    new OpenApiInfo
                    {
                        Title = "Ugovor o zakupu API", 
                        Version = "v1",
                        Description = "API Ugovor o zakupu omogućava unos, izmenu i pregled podataka o ugovorima o zakupu, tipovima garancije i rokovima dospeca.",
                        Contact = new OpenApiContact
                        {
                            Name = "Vuk Pekez",
                            Email = "vukpekez@uns.ac.rs",
                            Url = new Uri("https://github.com/vukpekez")
                        }
                    });
                
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger(c =>
            {
                c.SerializeAsV2 = true;
            });
            
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Ugovor o zakupu API");
                options.RoutePrefix = string.Empty;
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}