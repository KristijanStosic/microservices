using JavnoNadmetanjeService.Data;
using JavnoNadmetanjeService.Data.Interfaces;
using JavnoNadmetanjeService.Entities;
using JavnoNadmetanjeService.Helpers;
using JavnoNadmetanjeService.Models.Other;
using JavnoNadmetanjeService.ServiceCalls;
using JavnoNadmetanjeService.ServiceCalls.Mocks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace JavnoNadmetanjeService
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
            .ConfigureApiBehaviorOptions(setupAction =>
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

            services.AddScoped<IStatusRepository, StatusRepository>();
            services.AddScoped<ITipRepository, TipRepository>();
            services.AddScoped<IEtapaRepository, EtapaRepository>();
            services.AddScoped<IJavnoNadmetanjeRepository, JavnoNadmetanjeRepository>();

            services.AddScoped<IServiceCall<AdresaDto>, ServiceCall<AdresaDto>>();
            services.AddScoped<IServiceCall<KupacDto>, ServiceCall<KupacDto>>();
            services.AddScoped<IServiceCall<OvlascenoLiceDto>, ServiceCall<OvlascenoLiceDto>>();
            services.AddScoped<IServiceCall<DeoParceleDto>, ServiceCall<DeoParceleDto>>();

            services.AddScoped<IJavnoNadmetanjeCalls, JavnoNadmetanjeCalls>();
            services.AddScoped<IRabbitMQProducer, RabbitMQProducer>();
            services.AddScoped<ILoggerService, LoggerService>();

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

            services.AddSwaggerGen(setup =>
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

                setup.AddSecurityDefinition("Bearer", securitySchema);

                var securityRequirement = new OpenApiSecurityRequirement
                {
                    { securitySchema, new[] { "Bearer" } }
                };

                setup.AddSecurityRequirement(securityRequirement);

                setup.SwaggerDoc("v1",
                    new OpenApiInfo()
                    {
                        Title = "Javno nadmetanje API",
                        Version = "v1",
                        Description = "API Javno nadmetanje omogućava unos i pregled podataka o javnim nadmetanjima, statusu, tipu i etapama javnih nadmetanja.",
                        Contact = new Microsoft.OpenApi.Models.OpenApiContact
                        {
                            Name = "Dragan Majkić",
                            Email = "dragan.majkic@uns.ac.rs",
                            Url = new Uri(Configuration["Swagger:Github"])
                        }
                    });
                //Korisitmo refleksiju za dobijanje XML fajla sa komentarima
                var xmlComments = $"{ Assembly.GetExecutingAssembly().GetName().Name }.xml";
                var xmlCommentsPath = Path.Combine(AppContext.BaseDirectory, xmlComments);
                setup.IncludeXmlComments(xmlCommentsPath);
            });

            services.AddDbContext<JavnoNadmetanjeContext>();
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
                        await context.Response.WriteAsync("Desila se greška!");
                    });
                });
            }

            app.UseHttpsRedirection();

            app.UseSwagger();

            app.UseSwaggerUI(setupAction =>
            {
                setupAction.SwaggerEndpoint("/swagger/v1/swagger.json", "Javno nadmetanje API");
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
