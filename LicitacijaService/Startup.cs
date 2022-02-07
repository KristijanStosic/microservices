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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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

            services.AddScoped<ILoggerService, LoggerService>();

            services.AddScoped < IServiceCall<JavnoNadmetanjeDto>, ServiceCall<JavnoNadmetanjeDto>>();

            services.AddControllers();

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

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
