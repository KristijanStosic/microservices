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
using ParcelaService.Data;
using ParcelaService.Data.Interfaces;
using ParcelaService.Entities.DataContext;
using ParcelaService.Models.OtherServices;
using ParcelaService.ServiceCalls;
using ParcelaService.ServiceCalls.Mocks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ParcelaService
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

            services.AddScoped<IKlasaRepository, KlasaRepository>();
            services.AddScoped<IKulturaRepository, KulturaRepository>();
            services.AddScoped<IOblikSvojineRepository, OblikSvojineRepository>();
            services.AddScoped<IObradivostRepository, ObradivostRepository>();
            services.AddScoped<IOdvodnjavanjeRepository, OdvodnjavanjeRepository>();
            services.AddScoped<IZasticenaZonaRepository, ZasticenaZonaRepository>();
            services.AddScoped<IKatastarskaOpstinaRepository, KatastarskaOpstinaRepository>();
            services.AddScoped<IParcelaRepository, ParcelaRepository>();
            services.AddScoped<IDeoParceleRepository, DeoParceleRepository>();

            services.AddScoped<ILoggerService, LoggerServiceMock>();
            services.AddScoped<IServiceCall<KupacDto>, ServiceCallKupacMock<KupacDto>>();



            services.AddSwaggerGen(setup =>
            {
            setup.SwaggerDoc("v1",
                new OpenApiInfo()
                {
                    Title = "Parcela API",
                    Version = "v1",
                    Description = "API Parcela omogucava unos i pregled podataka o parcelama, delovima parcela, katarske opstine, klase, kulture, oblika svojina, obradivosti, odvodnjavanju i zasticenoj zoni parcela",
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact
                    {
                        Name = "Đorđe Atanackov",
                        Email = "djordje_atanackov@uns.ac.rs",
                        Url = new Uri(Configuration["Swagger:Github"])
                    }
                });

            var xmlComments = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlCommentsPath = Path.Combine(AppContext.BaseDirectory, xmlComments);
                setup.IncludeXmlComments(xmlCommentsPath);

            });

            services.AddDbContext<ParcelaContext>();
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
                setupAction.SwaggerEndpoint("/swagger/v1/swagger.json", "Parcela API");
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
