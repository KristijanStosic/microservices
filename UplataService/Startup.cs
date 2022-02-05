using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;
using UplataService.Entities.DataContext;
using UplataService.Model.Services;
using UplataService.Repository;
using UplataService.ServiceCalls;
using UplataService.ServiceCalls.Mocks;

namespace UplataService
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

                   //Prevodi validacione greške iz ModelState-a u RFC format
                   ValidationProblemDetails problemDetails = problemDetailsFactory.CreateValidationProblemDetails(context.HttpContext, context.ModelState);
                   problemDetails.Detail = "Pogledajte Error polje za detaljnije informacije.";
                   problemDetails.Instance = context.HttpContext.Request.Path;

                   //Definisemo da za validacione greske ne zelimo status kod 400 nego 422 - UnprocessibleEntity
                   var actionExecutiongContext = context as ActionExecutingContext;
                   if ((context.ModelState.ErrorCount > 0) && (actionExecutiongContext?.ActionArguments.Count == context.ActionDescriptor.Parameters.Count))
                   {
                       problemDetails.Status = StatusCodes.Status422UnprocessableEntity;
                       problemDetails.Title = "Desila se greška prilikom validacije.";

                        //Sve se vraca kao UnprocessibleEntity objekat
                        return new UnprocessableEntityObjectResult(problemDetails)
                        {
                           ContentTypes = { "application/problem+json" }
                        };
                   }

                    //Ako nesto ne moze da se parsira vraca se status kod 400 - Bad Request
                   problemDetails.Status = StatusCodes.Status400BadRequest;
                   problemDetails.Title = "Desila se greška prilikom parsiranja.";
                   return new BadRequestObjectResult(problemDetails)
                   {
                       ContentTypes = { "application/problem+json" }
                   };
               };
           });

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddScoped<IUplataRepository, UplataRepository>();

            services.AddScoped<IServiceCall<JavnoNadmetanjeDto>, ServiceCallJavnoNadmetanjeMock<JavnoNadmetanjeDto>>();
            services.AddScoped<ILoggerService, LoggerServiceMock>();
            //services.AddScoped<ILoggerService, LoggerService>();

            services.AddSwaggerGen(setup =>
            {
                setup.SwaggerDoc("v1",
                    new OpenApiInfo()
                    {
                        Title = "Uplata API",
                        Version = "v1",
                        Description = "API Uplata omogućava unos i pregled podataka o uplatama.",
                        Contact = new Microsoft.OpenApi.Models.OpenApiContact
                        {
                            Name = "Kristijan Stošić",
                            Email = "stosic@uns.ac.rs",
                            Url = new Uri("https://github.com/KristijanStosic")
                        }
                    });
                //Korisitmo refleksiju za dobijanje XML fajla za komentarima
                var xmlComments = $"{ Assembly.GetExecutingAssembly().GetName().Name }.xml";
                var xmlCommentsPath = Path.Combine(AppContext.BaseDirectory, xmlComments);
                setup.IncludeXmlComments(xmlCommentsPath);
            });

            services.AddDbContext<UplataContext>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseSwagger();

            app.UseSwaggerUI(setupAction =>
            {
                setupAction.SwaggerEndpoint("/swagger/v1/swagger.json", "Uplata API");
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
