using LicitacijaService.Data.Interfaces;
using LicitacijaService.Entities.DataContext;
using LicitacijaService.ServiceCalls;
using LicitacijaService.ServiceCalls.Mocks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using LicitacijaService.Data;
using LicitacijaService.Models.OtherServices;

namespace LicitacijaService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(setup =>
            {
                setup.ReturnHttpNotAcceptable = true;
            }
           ).AddXmlDataContractSerializerFormatters()
           .ConfigureApiBehaviorOptions(setupAction =>
           {
               setupAction.InvalidModelStateResponseFactory = context =>
               {
                   ProblemDetailsFactory problemDetailsFactory = context.HttpContext.RequestServices
                       .GetRequiredService<ProblemDetailsFactory>();


                   ValidationProblemDetails problemDetails = problemDetailsFactory.CreateValidationProblemDetails(context.HttpContext, context.ModelState);
                   problemDetails.Detail = "Pogledajte Error polje za detaljnije informacije";
                   problemDetails.Instance = context.HttpContext.Request.Path;

                   var actionExecutiongContext = context as ActionExecutingContext;
                   if ((context.ModelState.ErrorCount > 0) && (actionExecutiongContext?.ActionArguments.Count == context.ActionDescriptor.Parameters.Count))
                   {
                       problemDetails.Status = StatusCodes.Status422UnprocessableEntity;
                       problemDetails.Title = "Desila se greska prilikom validacije";

                       return new UnprocessableEntityObjectResult(problemDetails)
                       {
                           ContentTypes = { "application/problem+jsom" }
                       };
                   }

                   problemDetails.Status = StatusCodes.Status400BadRequest;
                   problemDetails.Title = "Desila se greska prilikom parsiranja";
                   return new BadRequestObjectResult(problemDetails)
                   {
                       ContentTypes = { "application/problem+json" }
                   };

               };
           });

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddDbContext<LicitacijaContext>();

            services.AddScoped<ILicitacijaRepository, LicitacijaRepository>();

            services.AddScoped<ILoggerService, LoggerServiceMock>();

            services.AddScoped < IServiceCall<JavnoNadmetanjeDto>, ServiceCallJavnoNadmetanjeMock<JavnoNadmetanjeDto>>();

            services.AddControllers();
            services.AddSwaggerGen(setup =>
            {
                setup.SwaggerDoc("v1",
                    new OpenApiInfo()
                    {
                        Title = "Licitacija API",
                        Version = "v1",
                        Description = "API licitacija omogucava unos i pregled podataka licitacije",
                        Contact = new Microsoft.OpenApi.Models.OpenApiContact
                        {
                            Name = "Đorđe Atanackov",
                            Email = "djordje_atanackov@uns.ac.rs",
                            Url = new Uri("https://github.com/AtanackovDjordje")
                        }
                    });

                var xmlComments = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlCommentsPath = Path.Combine(AppContext.BaseDirectory, xmlComments);
                setup.IncludeXmlComments(xmlCommentsPath);

            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(appBuilder =>
                {
                    appBuilder.Run(async context =>
                    {
                        context.Response.StatusCode = 500;
                        await context.Response.WriteAsync("Desila se greska!");
                    });
                });
            }

            app.UseHttpsRedirection();
            app.UseSwagger();
            app.UseSwaggerUI(setupAction =>
            {
                setupAction.SwaggerEndpoint("/swagger/v1/swagger.json", "Licitacija API");
                setupAction.RoutePrefix = "";
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
