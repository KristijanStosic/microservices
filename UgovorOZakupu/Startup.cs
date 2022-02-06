using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using UgovorOZakupu.Data.UnitOfWork;
using UgovorOZakupu.DbContext;
using UgovorOZakupu.Services;
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

            services.AddScoped<IServices, Services.Services>();

            // services.AddScoped<IServiceCalls, ServiceCalls>();
            services.AddScoped<IServiceCalls, ServiceCallsMock>();

            services.AddControllers(options => { options.ReturnHttpNotAcceptable = true; })
                .ConfigureApiBehaviorOptions(setupAction =>
                {
                    setupAction.InvalidModelStateResponseFactory = context =>
                    {
                        var problemDetailsFactory = context.HttpContext.RequestServices
                            .GetRequiredService<ProblemDetailsFactory>();

                        //Prevodi validacione greške iz ModelState-a u RFC format
                        var problemDetails =
                            problemDetailsFactory.CreateValidationProblemDetails(context.HttpContext,
                                context.ModelState);
                        problemDetails.Detail = "Pogledajte Error polje za detaljnije informacije.";
                        problemDetails.Instance = context.HttpContext.Request.Path;

                        //Definisemo da za validacione greske ne zelimo status kod 400 nego 422 - UnprocessibleEntity
                        var actionExecutiongContext = context as ActionExecutingContext;
                        if (context.ModelState.ErrorCount > 0 && actionExecutiongContext?.ActionArguments.Count ==
                            context.ActionDescriptor.Parameters.Count)
                        {
                            problemDetails.Status = StatusCodes.Status422UnprocessableEntity;
                            problemDetails.Title = "Desila se greška prilikom validacije.";

                            //Sve se vraca kao UnprocessibleEntity objekat
                            return new UnprocessableEntityObjectResult(problemDetails)
                            {
                                ContentTypes = {"application/problem+json"}
                            };
                        }

                        //Ako nesto ne moze da se parsira vraca se status kod 400 - Bad Request
                        problemDetails.Status = StatusCodes.Status400BadRequest;
                        problemDetails.Title = "Desila se greška prilikom parsiranja.";
                        return new BadRequestObjectResult(problemDetails)
                        {
                            ContentTypes = {"application/problem+json"}
                        };
                    };
                });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(
                    "v1",
                    new OpenApiInfo
                    {
                        Title = "Ugovor o zakupu API",
                        Version = "v1",
                        Description =
                            "API Ugovor o zakupu omogućava unos, izmenu i pregled podataka o ugovorima o zakupu, tipovima garancije i rokovima dospeca.",
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
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseSwagger(c => { c.SerializeAsV2 = true; });

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