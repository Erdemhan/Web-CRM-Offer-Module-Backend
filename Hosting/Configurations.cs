using System;
using System.Linq;
using CRMWeb;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using crmweb.Common.Auxiliary;
using crmweb.Data;
using crmweb.Hosting.Options;
using crmweb.Services;


namespace crmweb.Hosting
{
    public static class Configurations
    {
        public static void ConfigureDependencyInjection(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<MainDb>(options =>
                options.UseSqlServer(configuration.GetConnectionString("ConnectionString")));

            services.AddScoped<AuthService>();
            services.AddScoped<UserService>();
            services.AddScoped<CompanyService>();
            services.AddScoped<CompanyContactService>();
            services.AddScoped<OfferService>();
            services.AddScoped<DashboardService>();

            services.AddAutoMapper(typeof(Startup));

        }

        public static void ConfigureMiddleware(this IApplicationBuilder app, IWebHostEnvironment env)
        {

        }

        public static IMvcBuilder ConfigureFilters(this IMvcBuilder builder)
        {
            return builder.AddMvcOptions(options =>
            {

            });
        }

        public static IMvcBuilder ConfigureModelValidationOptions(this IMvcBuilder builder)
        {
            return builder.ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory =
                    context => new OkObjectResult(Result.PrepareFailure(
                        context.ModelState.Values.FirstOrDefault()
                        .Errors.FirstOrDefault()
                        .ErrorMessage)
                    );
               
            });
        }

        public static IMvcBuilder ConfigureJsonOptions(this IMvcBuilder builder)
        {
            return builder.AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new DefaultContractResolver(); //For PascalCase Fields
                options.SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
                options.SerializerSettings.DateParseHandling = DateParseHandling.DateTimeOffset;
                options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Local;

                options.SerializerSettings.NullValueHandling = NullValueHandling.Include;
                options.SerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
                options.SerializerSettings.Formatting = Formatting.None;

                options.SerializerSettings.FloatParseHandling = FloatParseHandling.Decimal;

            });
        }

        public static void ConfigureAuthentication(this IServiceCollection services)
        {
            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, configureOptions: null);

            services.AddTransient<IConfigureOptions<JwtBearerOptions>, ConfigureJwtBearerOptions>();
        }

        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo()
                {
                    Title = "Teklif Api",
                    Version = "v1.0",
                    Contact = new OpenApiContact
                    {
                        Name = "Edu",
                        Email = "erdmhn@gmail.com"
                    }
                });

                //c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());

                var vScheme = new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Description = "JWT Authorization header using the Bearer scheme. " +
                                  "\r\n\r\n Enter 'Bearer' [space] and then your token in the text input below." +
                                  "\r\n\r\nExample: \"Bearer 12345abcdef\"",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                };

                c.AddSecurityDefinition("Bearer", vScheme);
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {vScheme, new string[]{}}
                });
            });
            //services.AddSwaggerGenNewtonsoftSupport();
        }

        public static void ConfigureSwagger(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsProduction())
                return;

            app.UseSwagger(options =>
            {
                options.RouteTemplate = "docs/{documentName}/docs.json";
            });

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(options =>
            {
                options.RoutePrefix = "docs";
                options.SwaggerEndpoint("/docs/v1/docs.json", "Teklif Api V1");
            });
        }
    }

}
