// -----------------------------------------------------------------------
// <copyright file="Startup.cs" company="Wahine Kai">
// Copyright (c) Wahine Kai. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace WahineKai.Backend.Host
{
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc.Authorization;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Identity.Web;
    using Microsoft.OpenApi.Models;
    using WahineKai.Backend.Common;
    using WahineKai.Backend.Host.Middleware;

    /// <summary>
    /// Startup configuration
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Cors Policy used by this application
        /// </summary>
        private const string CorsPolicy = "AllowAllOrigins";

        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class
        /// </summary>
        /// <param name="configuration">ASP.NET configuration</param>
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
            this.Settings = new Settings(this.Configuration);
        }

        /// <summary>
        /// Gets ASP.NET configuration.
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Gets configurable settings we care about for this application
        /// </summary>
        public Settings Settings { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services">Service object passed in from ASP.NET</param>
        public void ConfigureServices(IServiceCollection services)
        {
            // This is required to be instantiated before the OpenIdConnectOptions starts getting configured.
            // By default, the claims mapping will map claim names in the old format to accommodate older SAML applications.
            // 'http://schemas.microsoft.com/ws/2008/06/identity/claims/role' instead of 'roles'
            // This flag ensures that the ClaimsIdentity claims collection will be built from the claims in the token
            // JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

            // Adds Microsoft Identity platform (AAD v2.0) support to protect this Api
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApi(
                    options =>
                {
                    this.Configuration.Bind("AzureAdB2C", options);
                },
                    options => { this.Configuration.Bind("AzureAdB2C", options); });

            // Require authentication on all controllers
            services.AddControllers(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "backend", Version = "v1" });
            });

            services.AddCors(options =>
            {
                options.AddPolicy(Startup.CorsPolicy, builder =>
                {
                    builder.AllowAnyOrigin();
                    builder.AllowAnyHeader();
                    builder.AllowAnyMethod();
                });
            });
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">ASP.NET application builder</param>
        /// <param name="env">ASP.NET Web host environeent</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "backend v1"));
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            // Allow all authenticated requests
            app.UseCors(Startup.CorsPolicy);

            app.UseMiddleware<RequestLoggingMiddleware>();

            // Allow authentication & authorization for endpoints
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
