using System;
using System.IO;
using System.Reflection;
using AutoMapper;
using Contacts.API.Extensions;
using Contacts.API.Factories;
using Contacts.API.Middlewares;
using Contacts.BL.Factories;
using Contacts.BL.Validators;
using Contacts.Common.Services;
using Contacts.Infrastructure.DAL;
using Contacts.Infrastructure.DAL.Core.Repositories;
using Contacts.Infrastructure.DAL.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace Contacts.API
{
    public class Startup
    {
        private const string CorsPolicy = "CorsPolicy";
        private const string SessionCookieName = "Session";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region CORS

            // NB! Very loose configuration that allows requests from anywhere
            services.AddCors(options =>
            {
                options.AddPolicy(CorsPolicy,
                    builder => builder
                        .SetIsOriginAllowed(policy => true)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });

            #endregion


            #region Automapper
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            #endregion

            #region Database

            services.AddDbContext<DatabaseContext>(options => options.UseSqlite("Data Source=Contacts.db"));

            #endregion


            #region Session

            services.AddDistributedMemoryCache(); // Currently used for session storage.
            services.AddHttpContextAccessor();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
                options.Cookie.Name = SessionCookieName;
            });

            #endregion


            services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            #region Swagger
            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Contacts API", Version = "v1" });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.XML";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                c.IncludeXmlComments(xmlPath);
            });

            // required to use newtonsoft as the default string parser
            // as the new Json.Text is missing some features we need
            services.AddSwaggerGenNewtonsoftSupport();
            #endregion

            services.AddScoped(typeof(IDatabaseContext), typeof(DatabaseContext));

            services.AddDerived(typeof(IBaseService), typeof(BaseService), ServiceLifetime.Scoped);
            services.AddDerived(typeof(IBaseEntityRepository<>), typeof(BaseEntityRepository<>), ServiceLifetime.Scoped);
            services.AddDerived(typeof(IBaseResultFactory<,>), typeof(BaseResultFactory<,>), ServiceLifetime.Scoped);
            services.AddDerived(typeof(IBaseFactory<,>), typeof(BaseFactory<,>), ServiceLifetime.Scoped);
            services.AddDerived(typeof(IBaseValidator<>), typeof(BaseValidator<>), ServiceLifetime.Scoped);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSession();

            app.UseMiddleware(typeof(ErrorHandlingMiddleware));

            app.UseCors(CorsPolicy);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("v1/swagger.json", "Contacts API V1");
                c.RoutePrefix = "swagger";
                c.DisplayRequestDuration();
            });

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
