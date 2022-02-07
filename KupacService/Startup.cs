using KupacService.Data;
using KupacService.Data.Interfaces;
using KupacService.Entities.DataContext;
using KupacService.Helpers;
using KupacService.Model.OtherServices;
using KupacService.ServiceCalls;
using KupacService.ServiceCalls.Mocks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Reflection;
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

            var secret = Configuration["ApplicationSettings:JWT_Secret"].ToString();
            var key = Encoding.ASCII.GetBytes(secret);

            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            services.AddSwaggerGen(c =>
            {
                var securitySchema = new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                };

                c.AddSecurityDefinition("Bearer", securitySchema);

                var securityRequirement = new OpenApiSecurityRequirement
                {
                    { securitySchema, new[] { "Bearer" } }
                };

                c.AddSecurityRequirement(securityRequirement);
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Adresa API",
                    Version = "v1",
                    Description = "API Adresa omogućava unos i pregled adresa i država",
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact
                    {
                        Name = "Gavrilo Stanić",
                        Email = "stanic.gavrilo@uns.ac.rs",
                        Url = new Uri(Configuration["Swagger:Github"])
                    }
                });

                //Pomocu refleksije dobijamo ime XML fajla sa komentarima (ovako smo ga nazvali u Project -> Properties)
                var xmlComments = $"{ Assembly.GetExecutingAssembly().GetName().Name }.xml";

                //Pravimo putanju do XML fajla sa komentarima
                var xmlCommentsPath = Path.Combine(AppContext.BaseDirectory, xmlComments);

                //Govorimo swagger-u gde se nalazi dati xml fajl sa komentarima
                c.IncludeXmlComments(xmlCommentsPath);
            });
            services.AddDbContext<KupacContext>();
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
                setupAction.SwaggerEndpoint("/swagger/v1/swagger.json", "Kupac API");
                setupAction.RoutePrefix = "";
            });
                
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}
