// -----------------------------------------------------------------------
// <copyright file="UserService.cs" company="Wahine Kai">
// Copyright (c) Wahine Kai. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace WahineKai.Backend.Service
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using WahineKai.Backend.Common;
    using WahineKai.Backend.DTO;
    using WahineKai.Backend.DTO.Contracts;
    using WahineKai.Backend.DTO.Models;
    using WahineKai.Backend.DTO.Properties;
    using WahineKai.Backend.Service.Contracts;

    /// <inheritdoc/>
    public class UserService : ServiceBase, IUserService
    {
        private readonly IUserRepository<AdminUser> userRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserService"/> class.
        /// </summary>
        /// <param name="loggerFactory">Logger factory for this service</param>
        /// <param name="configuration">Application configuration</param>
        public UserService(ILoggerFactory loggerFactory, IConfiguration configuration)
            : base(loggerFactory, configuration)
        {
            this.Logger.LogTrace("Construction of User Service beginning");

            // Build cosmos configuration
            var cosmosConfiguration = CosmosConfiguration.BuildFromConfiguration(this.Configuration);

            this.userRepository = new CosmosUserRepository<AdminUser>(cosmosConfiguration, loggerFactory);

            this.Logger.LogTrace("Construction of User Service complete");
        }

        /// <inheritdoc/>
        public async Task<AdminUser> GetByEmailAsync(string userEmail, string? callingUserEmail = null)
        {
            // Sanity check input
            userEmail = Ensure.IsNotNullOrWhitespace(() => userEmail);

            // Calling user is user to get
            callingUserEmail ??= userEmail;

            await this.EnsureCallingUserPermissions(callingUserEmail);

            this.Logger.LogDebug($"Getting user with email {userEmail} from repository");

            var user = await this.userRepository.GetUserByEmailAsync(userEmail);

            this.Logger.LogTrace($"Got user with email {user.Email} and id {user.Id} from the user repository");

            return user;
        }

        /// <inheritdoc/>
        public async Task<AdminUser> GetByIdAsync(Guid id, string? callingUserEmail)
        {
            // Sanity check input
            id = Ensure.IsNotNull(() => id);

            await this.EnsureCallingUserPermissions(callingUserEmail);

            this.Logger.LogDebug($"Getting user with id {id} from repository");

            var user = await this.userRepository.GetUserByIdAsync(id);

            this.Logger.LogTrace($"Got user with email {user.Email} and id {user.Id} from the user repository");

            return user;
        }

        /// <inheritdoc/>
        public async Task<AdminUser> CreateAsync(AdminUser user, string callingUserEmail)
        {
            await this.EnsureCallingUserPermissions(callingUserEmail);

            this.Logger.LogDebug("Creating a user in the repository");

            // input sanity checking
            user = Ensure.IsNotNull(() => user);
            user.Validate();

            // Create in repository
            var userFromRepository = await this.userRepository.CreateUserAsync(user);

            // Sanity check output
            userFromRepository = Ensure.IsNotNull(() => userFromRepository);
            userFromRepository.Validate();

            this.Logger.LogTrace($"Create user with id {user.Id} and email {user.Email} in the repository");

            return userFromRepository;
        }

        /// <inheritdoc/>
        public async Task<AdminUser> ReplaceByEmailAsync(string userEmail, AdminUser updatedUser, string? callingUserEmail = null)
        {
            // Sanity check input
            userEmail = Ensure.IsNotNullOrWhitespace(() => userEmail);
            updatedUser = Ensure.IsNotNull(() => updatedUser);

            // CallingUserEmail is userEmail by default
            callingUserEmail ??= userEmail;

            // Ensure user who making this request has the permissions to perform this action
            await this.EnsureCallingUserPermissions(callingUserEmail);

            // Get old user
            var oldUser = await this.userRepository.GetUserByEmailAsync(userEmail);

            // Update user and return it
            this.Logger.LogTrace($"Replacing user with email {userEmail} with new values");

            var replacedUser = AdminUser.Replace(oldUser, updatedUser);
            var userFromDatabase = await this.userRepository.ReplaceUserAsync(replacedUser, replacedUser.Id);

            // Sanity check output
            userFromDatabase = Ensure.IsNotNull(() => userFromDatabase);
            userFromDatabase.Validate();

            this.Logger.LogDebug("Updated 1 user in the database");

            return userFromDatabase;
        }

        /// <inheritdoc/>
        public async Task<AdminUser> ReplaceByIdAsync(Guid id, AdminUser updatedUser, string? callingUserEmail)
        {
            // Sanity check input
            id = Ensure.IsNotNull(() => id);
            updatedUser = Ensure.IsNotNull(() => updatedUser);

            // Ensure user who making this request has the permissions to perform this action
            await this.EnsureCallingUserPermissions(callingUserEmail);

            // Get old user
            var oldUser = await this.userRepository.GetUserByIdAsync(id);

            // Update user and return it
            this.Logger.LogTrace($"Replacing user with id {id} with new values");

            var replacedUser = AdminUser.Replace(oldUser, updatedUser);
            var userFromDatabase = await this.userRepository.ReplaceUserAsync(replacedUser, replacedUser.Id);

            // Sanity check output
            userFromDatabase = Ensure.IsNotNull(() => userFromDatabase);
            userFromDatabase.Validate();

            this.Logger.LogDebug("Updated 1 user in the database");

            return userFromDatabase;
        }
    }
}
