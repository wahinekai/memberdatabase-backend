// -----------------------------------------------------------------------
// <copyright file="ServiceBase.cs" company="Wahine Kai">
// Copyright (c) Wahine Kai. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace WahineKai.Common.Api.Services
{
    using System.Threading.Tasks;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using WahineKai.Common;
    using WahineKai.MemberDatabase.Dto;
    using WahineKai.MemberDatabase.Dto.Contracts;
    using WahineKai.MemberDatabase.Dto.Models;
    using WahineKai.MemberDatabase.Dto.Properties;

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
        /// <param name="configurationMaybeNull">Application configuration</param>
        public ServiceBase(ILoggerFactory loggerFactory, IConfiguration? configurationMaybeNull)
        {
            this.Configuration = Ensure.IsNotNull(() => configurationMaybeNull);

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
        protected async Task EnsureCallingUserPermissionsAsync(string? callingUserEmail)
        {
            var user = await this.GetCallingUser(callingUserEmail);
            this.EnsureCallingUserPermissions(user);
        }

        /// <summary>
        /// Ensures that the calling user exists and is an active admin user
        /// </summary>
        /// <param name="user">The calling user</param>
        protected void EnsureCallingUserPermissions(AdminUser user)
        {
            this.Logger.LogTrace($"Ensuring that user with id {user.Id} is an active administrator");

            Ensure.IsNotNull(() => user);
            Ensure.IsTrue(() => user.Admin);
            Ensure.IsTrue(() => user.Active);
        }

        /// <summary>
        /// Get the user calling the requset
        /// </summary>
        /// <param name="callingUserEmail">The email of the calling user</param>
        /// <returns>The user calling the request</returns>
        protected async Task<AdminUser> GetCallingUser(string? callingUserEmail)
        {
            // Sanity check input
            callingUserEmail = Ensure.IsNotNullOrWhitespace(() => callingUserEmail);

            var user = await this.userRepository.GetUserByEmailAsync(callingUserEmail);
            return user;
        }
    }
}
