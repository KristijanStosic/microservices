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
using OvlascenoLiceService.Data;
using OvlascenoLiceService.Data.Interfaces;
using OvlascenoLiceService.Entities.DataContext;
using OvlascenoLiceService.Models.OtherServices;
using OvlascenoLiceService.ServiceCalls;
using OvlascenoLiceService.ServiceCalls.Mocks;
using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace OvlascenoLiceService
{
    /// <summary>
    /// Startup class
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Startup klasa konstruktor
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// IConfiguration property
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
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

            services.AddScoped<IOvlascenoLiceRepository, OvlascenoLiceRepository>();
            services.AddScoped<IBrojTableRepository, BrojTableRepository>();
            services.AddScoped<IServiceCall<AdresaDto>, ServiceCall<AdresaDto>>();
            services.AddScoped<IServiceCall<DrzavaDto>, ServiceCall<DrzavaDto>>();
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
                        Title = "Ovlasceno lice API",
                        Version = "v1",
                        Description = "API Ovlašćeno lice omogućava unos i pregled podataka o ovlašćenim licima i o brojevima tabli",
                        Contact = new Microsoft.OpenApi.Models.OpenApiContact
                        {
                            Name = "Dragan Majkić",
                            Email = "dragan.majkic@uns.ac.rs",
                            Url = new Uri(Configuration["Swagger:Github"])
                        }
                    });
                //Korisitmo refleksiju za dobijanje XML fajla za komentarima
                var xmlComments = $"{ Assembly.GetExecutingAssembly().GetName().Name }.xml";
                var xmlCommentsPath = Path.Combine(AppContext.BaseDirectory, xmlComments);
                setup.IncludeXmlComments(xmlCommentsPath);
            });

            services.AddDbContext<OvlascenoLiceContext>();
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
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
                setupAction.SwaggerEndpoint("/swagger/v1/swagger.json", "Ovlasceno Lice API");
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
