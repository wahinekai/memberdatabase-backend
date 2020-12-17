// -----------------------------------------------------------------------
// <copyright file="ServiceBase.cs" company="Wahine Kai">
// Copyright (c) Wahine Kai. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace WahineKai.Backend.Service
{
    using System.Threading.Tasks;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using WahineKai.Backend.Common;
    using WahineKai.Backend.DTO;
    using WahineKai.Backend.DTO.Contracts;
    using WahineKai.Backend.DTO.Models;
    using WahineKai.Backend.DTO.Properties;

    /// <summary>
    /// Base class for all services
    /// </summary>
    public abstract class ServiceBase
    {
        private readonly IUserRepository<AdminUser> userRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceBase"/> class.
        /// </summary>
        /// <param name="loggerFactory">Logger factory for this service</param>
        /// <param name="configuration">Application configuration</param>
        public ServiceBase(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            this.Configuration = Ensure.IsNotNull(() => configuration);

            loggerFactory = Ensure.IsNotNull(() => loggerFactory);
            this.Logger = loggerFactory.CreateLogger<ServiceBase>();

            // Build cosmos configuration
            var cosmosConfiguration = CosmosConfiguration.BuildFromConfiguration(this.Configuration);

            this.userRepository = new CosmosUserRepository<AdminUser>(cosmosConfiguration, loggerFactory);

            this.Logger.LogTrace("Construction of ServiceBase complete");
        }

        /// <summary>
        /// Gets logger for console logging
        /// </summary>
        protected ILogger Logger { get; init; }

        /// <summary>
        /// Gets global configuration
        /// </summary>
        protected IConfiguration Configuration { get; init; }

        /// <summary>
        /// Ensure that the calling user exists and is an admin user
        /// </summary>
        /// <param name="callingUserEmail">The calling user's email address</param>
        /// <returns>A <see cref="Task"/></returns>
        protected async Task EnsureCallingUserPermissions(string? callingUserEmail)
        {
            // Sanity check input
            callingUserEmail = Ensure.IsNotNullOrWhitespace(() => callingUserEmail);

            this.Logger.LogTrace($"Checkeing that user with email {callingUserEmail} exists is an administrator");

            var user = await this.userRepository.GetUserByEmailAsync(callingUserEmail);
            Ensure.IsNotNull(() => user);
            Ensure.IsTrue(() => user.Admin);
            Ensure.IsTrue(() => user.Active);
        }
    }
}
