// -----------------------------------------------------------------------
// <copyright file="UserService.cs" company="Wahine Kai">
// Copyright (c) Wahine Kai. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace WahineKai.MemberDatabase.Backend.Service
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using CsvHelper;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using WahineKai.Common;
    using WahineKai.Common.Api.Services;
    using WahineKai.MemberDatabase.Backend.Service.Contracts;
    using WahineKai.MemberDatabase.Backend.Service.Extensions;
    using WahineKai.MemberDatabase.Backend.Service.Models;
    using WahineKai.MemberDatabase.Dto;
    using WahineKai.MemberDatabase.Dto.Contracts;
    using WahineKai.MemberDatabase.Dto.Models;
    using WahineKai.MemberDatabase.Dto.Properties;

    /// <inheritdoc/>
    public class UserService : ServiceBase, IUserService
    {
        private readonly IUserRepository userRepository;
        private readonly IAzureActiveDirectoryRepository azureActiveDirectoryRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserService"/> class.
        /// </summary>
        /// <param name="loggerFactory">Logger factory for this service</param>
        /// <param name="configuration">Application configuration</param>
        public UserService(ILoggerFactory loggerFactory, IConfiguration configuration)
            : base(loggerFactory, configuration)
        {
            this.Logger.LogTrace("Construction of User Service beginning");

            // Build user repository
            var cosmosConfiguration = CosmosConfiguration.BuildFromConfiguration(this.Configuration);
            this.userRepository = new CosmosUserRepository(cosmosConfiguration, loggerFactory);

            // Build AAD Repository
            var azureActiveDirectoryConfiguration = AzureActiveDirectoryConfiguration.BuildFromConfiguration(this.Configuration);
            this.azureActiveDirectoryRepository = new AzureActiveDirectoryRepository(loggerFactory, azureActiveDirectoryConfiguration);

            this.Logger.LogTrace("Construction of User Service complete");
        }

        /// <inheritdoc/>
        public async Task<ImportReturn> UploadUsersFromCsvAsync(Stream usersStream, string callingUserEmail)
        {
            // Sanity check input
            callingUserEmail = Ensure.IsNotNullOrWhitespace(() => callingUserEmail);

            await this.EnsureCallingUserPermissionsAsync(callingUserEmail);

            using var csvReader = new CsvReader(new StreamReader(usersStream), CultureInfo.InvariantCulture).Configure();

            var users = await csvReader.GetRecordsAsync<AdminUser>().ToListAsync();

            var validUsers = new List<AdminUser>();
            var invalidUsers = new List<AdminUser>();

            foreach (var user in users)
            {
                try
                {
                    user.Validate();
                    validUsers.Add(user);
                }
                catch (Exception)
                {
                    invalidUsers.Add(user);
                }
            }

            var duplicateUsers = new List<AdminUser>();
            var importedUsers = new List<AdminUser>();

            // Try to import users into database and get duplicates
            foreach (var user in validUsers)
            {
                try
                {
                    var userFromDatabase = await this.userRepository.CreateUserAsync(user);
                    importedUsers.Add(userFromDatabase);
                }
                catch (Exception)
                {
                    duplicateUsers.Add(user);
                }
            }

            // Log results
            this.Logger.LogInformation($"{validUsers.Count} valid, {invalidUsers.Count} invalid, and {duplicateUsers.Count} duplicate users out of {users.Count} total users");

            return new ImportReturn()
            {
                ImportedUsers = importedUsers,
                DuplicateUsers = duplicateUsers,
                InvalidUsers = invalidUsers,
            };
        }

        /// <inheritdoc/>
        public async Task<ICollection<AdminUser>> GetAllUsersAsync(string userEmail)
        {
            // Sanity check input
            userEmail = Ensure.IsNotNullOrWhitespace(() => userEmail);

            await this.EnsureCallingUserPermissionsAsync(userEmail);

            this.Logger.LogDebug("Getting all users from repository");

            var users = await this.userRepository.GetAllUsersAsync();

            this.Logger.LogTrace($"Got {users.Count} users from the user repository");

            return users;
        }

        /// <inheritdoc/>
        public async Task<AdminUser> GetByEmailAsync(string userEmail, string? callingUserEmail = null)
        {
            // Sanity check input
            userEmail = Ensure.IsNotNullOrWhitespace(() => userEmail);

            // Calling user is user to get
            callingUserEmail ??= userEmail;

            await this.EnsureCallingUserPermissionsAsync(callingUserEmail);

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

            await this.EnsureCallingUserPermissionsAsync(callingUserEmail);

            this.Logger.LogDebug($"Getting user with id {id} from repository");

            var user = await this.userRepository.GetUserByIdAsync(id);

            this.Logger.LogTrace($"Got user with email {user.Email} and id {user.Id} from the user repository");

            return user;
        }

        /// <inheritdoc/>
        public async Task DeleteByIdAsync(Guid id, string callingUserEmail)
        {
            // Sanity check input
            id = Ensure.IsNotNull(() => id);

            var me = await this.userRepository.GetUserByEmailAsync(callingUserEmail);

            this.EnsureCallingUserPermissions(me);

            // Cannot delete yourself
            if (me.Id == id)
            {
                throw new ArgumentException("You cannot delete yourself!");
            }

            this.Logger.LogDebug($"Deleting user with id {id} from repository");

            await this.userRepository.DeleteUserByIdAsync(id);

            this.Logger.LogTrace($"Deleted user with and id {id} from the user repository");
        }

        /// <inheritdoc/>
        public async Task<AdminUser> CreateAsync(AdminUser user, string callingUserEmail)
        {
            await this.EnsureCallingUserPermissionsAsync(callingUserEmail);

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
        public async Task<AdminUser> UpdateByEmailAsync(string userEmail, AdminUser updatedUser, string? callingUserEmail = null)
        {
            // Sanity check input
            userEmail = Ensure.IsNotNullOrWhitespace(() => userEmail);

            // CallingUserEmail is userEmail by default
            callingUserEmail ??= userEmail;

            // Ensure user who making this request has the permissions to perform this action
            await this.EnsureCallingUserPermissionsAsync(callingUserEmail);

            // Get old user
            var oldUser = await this.userRepository.GetUserByEmailAsync(userEmail);

            // Update user and return it
            this.Logger.LogTrace($"Updating user with email {userEmail} with new values");

            return await this.UpdateUserAsync(oldUser, updatedUser);
        }

        /// <inheritdoc/>
        public async Task<AdminUser> UpdateByIdAsync(Guid id, AdminUser updatedUser, string callingUserEmail)
        {
            // Sanity check input
            id = Ensure.IsNotNull(() => id);
            callingUserEmail = Ensure.IsNotNullOrWhitespace(() => callingUserEmail);

            // Ensure user who making this request has the permissions to perform this action
            await this.EnsureCallingUserPermissionsAsync(callingUserEmail);

            // Update user and return it
            this.Logger.LogTrace($"Replacing user with id {id} with new values");

            var oldUser = await this.userRepository.GetUserByIdAsync(id);
            return await this.UpdateUserAsync(oldUser, updatedUser);
        }

        /// <summary>
        /// Replaces the user in the database and updates Azure Active Directory if properties have changed
        /// </summary>
        /// <param name="oldUser">The user to change</param>
        /// <param name="updatedUser">The parameters to update the user with</param>
        /// <returns>The updated user from the database</returns>
        private async Task<AdminUser> UpdateUserAsync(AdminUser oldUser, AdminUser updatedUser)
        {
            // Sanity check input
            oldUser = Ensure.IsNotNull(() => oldUser);
            updatedUser = Ensure.IsNotNull(() => updatedUser);

            // Replace user in database
            var userFromDatabase = await this.userRepository.UpdateUserAsync(updatedUser);

            // Update email in AAD B2C if needed
            if (oldUser.Email != updatedUser.Email)
            {
                var oldEmail = Ensure.IsNotNullOrWhitespace(() => oldUser.Email);
                var newEmail = Ensure.IsNotNullOrWhitespace(() => userFromDatabase.Email);

                await this.azureActiveDirectoryRepository.UpdateUserEmailAsync(oldEmail, newEmail);
            }

            // Sanity check output
            userFromDatabase = Ensure.IsNotNull(() => userFromDatabase);
            userFromDatabase.Validate();

            this.Logger.LogDebug("Updated 1 user in the database");

            return userFromDatabase;
        }
    }
}
