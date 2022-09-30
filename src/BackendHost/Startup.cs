// -----------------------------------------------------------------------
// <copyright file="Startup.cs" company="Wahine Kai">
// Copyright (c) Wahine Kai. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace WahineKai.MemberDatabase.Backend.Host
{
    using System.Text.Json.Serialization;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc.Authorization;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Identity.Web;
    using Microsoft.Net.Http.Headers;
    using WahineKai.Common.Api.Middleware;
    using WebApiContrib.Core.Formatter.Csv;

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
        }

        /// <summary>
        /// Gets ASP.NET configuration.
        /// </summary>
        public IConfiguration Configuration { get; }

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

            // Add new authentication version
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApi(this.Configuration, configSectionName: "AzureAdB2C");

            // Require authentication on all controllers
            services.AddControllers(options =>
            {
                // Require authentication
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .RequireClaim("emails")
                    .Build();
                options.Filters.Add(new AuthorizeFilter(policy));

                var csvFormatterOptions = new CsvFormatterOptions() { CsvDelimiter = "," };

                // Allow CSV Input & Output Formatter
                options.InputFormatters.Add(new CsvInputFormatter(csvFormatterOptions));
                options.OutputFormatters.Add(new CsvOutputFormatter(csvFormatterOptions));
                options.FormatterMappings.SetMediaTypeMappingForFormat("csv", MediaTypeHeaderValue.Parse("text/csv"));
            })
               .AddJsonOptions(options =>
               {
                   // Enums are serialized to strings with custom names
                   options.JsonSerializerOptions.Converters.Add(new JsonStringEnumMemberConverter());
                   options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
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
            }

            app.UseRouting();

            // Allow all authenticated requests
            app.UseCors(Startup.CorsPolicy);

            // Allow authentication & authorization for endpoints
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseMiddleware<RequestLoggingMiddleware>();
        }
    }
}
