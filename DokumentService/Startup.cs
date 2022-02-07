using System;
using System.IO;
using System.Reflection;
using System.Text;
using DokumentService.Data.UnitOfWork;
using DokumentService.DbContext;
using DokumentService.Services.Logger;
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

namespace DokumentService
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DokumentDbContext>();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<ILoggerService, LoggerService>();

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

            var secret = _configuration.GetValue<string>("ApplicationSettings:JWT_Secret");
            var key = Encoding.ASCII.GetBytes(secret);

            services.AddAuthentication(option =>
                {
                    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
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
                    Description =
                        "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
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
                    {securitySchema, new[] {"Bearer"}}
                };

                c.AddSecurityRequirement(securityRequirement);

                c.SwaggerDoc(
                    "v1",
                    new OpenApiInfo
                    {
                        Title = "Dokument API",
                        Version = "v1",
                        Description =
                            "API Dokument omogućava unos izmenu i pregled podataka o dokumentima i tipu dokumenata.",
                        Contact = new OpenApiContact
                        {
                            Name = "Vuk Pekez",
                            Email = "vukpekez@uns.ac.rs",
                            Url = new Uri(_configuration.GetValue<string>("Swagger:Github"))
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

            app.UseSwagger();

            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Dokument API");
                options.RoutePrefix = string.Empty;
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}