using System;
using System.IO;
using System.Reflection;
using DokumentService.Data.Dokument;
using DokumentService.Data.TipDokumenta;
using DokumentService.Data.UnitOfWork;
using DokumentService.DbContext;
using DokumentService.Services.Logger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace DokumentService
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DokumentDbContext>();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ITipDokumentaRepository, TipDokumentaRepository>();
            services.AddScoped<IDokumentRepository, DokumentRepository>();

            services.AddScoped<ILoggerService, LoggerMockService>();
            // services.AddScoped<ILoggerService, LoggerService>();
            
            services.AddControllers();
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(
                    "v1",
                    new OpenApiInfo
                    {
                        Title = "Dokument API", 
                        Version = "v1",
                        Description = "API Dokument omogućava unos izmenu i pregled podataka o dokumentima i tipu dokumenata.",
                        Contact = new OpenApiContact
                        {
                            Name = "Vuk Pekez",
                            Email = "vukpekez@uns.ac.rs",
                            Url = new Uri("https://github.com/vukpekez")
                        }
                    });
                
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                Console.WriteLine(xmlPath);
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
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Dokument API");
                options.RoutePrefix = string.Empty;
            });
            
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}