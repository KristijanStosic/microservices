using AdresaService.Data;
using AdresaService.Data.Interfaces;
using AdresaService.Entities.DataContext;
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
using System.Linq;
using System.Threading.Tasks;

namespace AdresaService
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
                setup.ReturnHttpNotAcceptable = true;
            }
            ).AddXmlDataContractSerializerFormatters() //Dodajemo mogucnost za Xml format
            .ConfigureApiBehaviorOptions(setupAction => //Podrzavanje Problem Details for http APIs
            {
                setupAction.InvalidModelStateResponseFactory = context =>
                {
                    ProblemDetailsFactory problemDetailsFactory = context.HttpContext.RequestServices
                        .GetRequiredService<ProblemDetailsFactory>();

                    //Prevodi validacione gre�ke iz ModelState-a u RFC format
                    ValidationProblemDetails problemDetails = problemDetailsFactory.CreateValidationProblemDetails(context.HttpContext, context.ModelState);
                    problemDetails.Detail = "Pogledajte Error polje za detaljnije informacije.";
                    problemDetails.Instance = context.HttpContext.Request.Path;

                    //Definisemo da za validacione greske ne zelimo status kod 400 nego 422 - UnprocessibleEntity
                    var actionExecutiongContext = context as ActionExecutingContext;
                    if ((context.ModelState.ErrorCount > 0) && (actionExecutiongContext?.ActionArguments.Count == context.ActionDescriptor.Parameters.Count))
                    {
                        problemDetails.Status = StatusCodes.Status422UnprocessableEntity;
                        problemDetails.Title = "Desila se gre�ka prilikom validacije.";

                        //Sve se vraca kao UnprocessibleEntity objekat
                        return new UnprocessableEntityObjectResult(problemDetails)
                        {
                            ContentTypes = { "application/problem+json" }
                        };
                    }

                    //Ako nesto ne moze da se parsira vraca se status kod 400 - Bad Request
                    problemDetails.Status = StatusCodes.Status400BadRequest;
                    problemDetails.Title = "Desila se gre�ka prilikom parsiranja.";
                    return new BadRequestObjectResult(problemDetails)
                    {
                        ContentTypes = { "application/problem+json" }
                    };
                };
            });


            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddScoped<IDrzavaRepository,DrzavaRepository>();
            services.AddScoped<IAdresaRepository, AdresaRepository>();
            services.AddScoped<ILoggerService, LoggerServiceMock>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AdresaService", Version = "v1" });
            });

            services.AddDbContext<AdresaContext>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AdresaService v1"));
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
