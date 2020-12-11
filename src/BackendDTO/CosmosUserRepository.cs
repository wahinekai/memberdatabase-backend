// -----------------------------------------------------------------------
// <copyright file="CosmosUserRepository.cs" company="Wahine Kai">
// Copyright (c) Wahine Kai. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace WahineKai.Backend.DTO
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Azure.Cosmos.Linq;
    using Microsoft.Extensions.Logging;
    using WahineKai.Backend.Common;
    using WahineKai.Backend.DTO.Contracts;
    using WahineKai.Backend.DTO.Models;
    using WahineKai.Backend.DTO.Properties;

    /// <summary>
    /// Implementaion of IUserRepository
    /// </summary>
    public sealed class CosmosUserRepository : CosmosRepositoryBase, IUserRepository
    {
        private readonly Microsoft.Azure.Cosmos.Container container;

        /// <summary>
        /// Initializes a new instance of the <see cref="CosmosUserRepository"/> class.
        /// </summary>
        /// <param name="cosmosConfiguration">Configuration to create connection with an Azure Cosmos DB Database</param>
        /// <param name="loggerFactory">Logger factory to create a logger</param>
        public CosmosUserRepository(CosmosConfiguration cosmosConfiguration, ILoggerFactory loggerFactory)
            : base(cosmosConfiguration, loggerFactory)
        {
            this.Logger.LogTrace("Beginning construction of Cosmos User Repository");

            this.container = this.CosmosClient.GetContainer(this.CosmosConfiguration.DatabaseId, User.ContainerId);

            this.Logger.LogTrace("Construction of Cosmos User Repository complete");
        }

        /// <inheritdoc/>
        public async Task<User> GetUserByEmailAsync(string email)
        {
            // Sanity check input
            email = Ensure.IsNotNullOrWhitespace(() => email);

            this.Logger.LogTrace($"Getting user with email ${email} from Cosmos DB");

            using var iterator = this.container.GetItemLinqQueryable<User>()
                .Where(user => user.Email.Equals(email))
                .Take(1)
                .ToFeedIterator();

            var feedResponse = await iterator.ReadNextAsync();

            // There should be only one result
            Ensure.IsFalse(() => iterator.HasMoreResults);
            Ensure.AreEqual(() => 1, () => feedResponse.Count);

            var maybeNullUser = feedResponse.Single(user => user.Email == email);

            // Ensure user is not null
            var user = Ensure.IsNotNull(() => maybeNullUser);
            user.Validate();

            this.Logger.LogInformation("Got 1 user from Cosmos DB");

            return user;
        }

        /// <inheritdoc/>
        public async Task<User> GetUserByIdAsync(Guid id)
        {
            // Sanity check input
            id = Ensure.IsNotNull(() => id);

            this.Logger.LogTrace($"Getting user with id {id} from Cosmos DB");

            using var iterator = this.container.GetItemLinqQueryable<User>()
                .Where(user => id.ToString() == user.Id.ToString())
                .Take(1)
                .ToFeedIterator();

            var feedResponse = await iterator.ReadNextAsync();

            // There should be only one result
            Ensure.IsFalse(() => iterator.HasMoreResults);
            Ensure.AreEqual(() => 1, () => feedResponse.Count);

            var maybeNullUser = feedResponse.Single(user => id.Equals(user.Id));

            // Ensure user is not null
            var user = Ensure.IsNotNull(() => maybeNullUser);
            user.Validate();

            this.Logger.LogInformation("Got 1 user from Cosmos DB");

            return user;
        }

        /// <inheritdoc/>
        public async Task DeleteUserAsync(User user)
        {
            // Sanity check input
            Ensure.IsNotNull(() => user);
            user.Validate();

            this.Logger.LogTrace($"Deleting user with id {user.Id} and email {user.Email} from the database");

            await this.container.DeleteItemAsync<User>(user.Id.ToString(), new Microsoft.Azure.Cosmos.PartitionKey(user.Email));

            this.Logger.LogInformation("Deleted 1 user from the database");
        }

        /// <inheritdoc/>
        public async Task<ICollection<User>> GetAllUsersAsync()
        {
            this.Logger.LogDebug("Getting all users from Cosmos DB");

            using var iterator = this.container.GetItemQueryIterator<User>();

            var users = new Collection<User>();

            while (iterator.HasMoreResults)
            {
                foreach (var user in await iterator.ReadNextAsync())
                {
                    Ensure.IsNotNull(() => user);
                    user.Validate();

                    users.Add(user);
                }
            }

            this.Logger.LogInformation($"Got {users.Count} users from Cosmos DB");

            return users;
        }

        /// <inheritdoc/>
        public async Task<User> CreateUserAsync(User user)
        {
            // Input sanity checking
            user = Ensure.IsNotNull(() => user);
            user.Validate();

            this.Logger.LogTrace($"Creating user with id {user.Id} and email {user.Email} in Cosmos DB");

            // Add user to the database
            var userResponse = await this.container.CreateItemAsync(user);

            // Check that the user has been added and is valid
            var userFromDatabase = userResponse.Resource;
            userFromDatabase = Ensure.IsNotNull(() => userFromDatabase);
            userFromDatabase.Validate();

            this.Logger.LogInformation("Created 1 user in the database");

            return userFromDatabase;
        }

        /// <inheritdoc/>
        public async Task<User> ReplaceUserAsync(User updatedUser, Guid id)
        {
            // Input sanity checking
            updatedUser = Ensure.IsNotNull(() => updatedUser);
            updatedUser.Validate();
            id = Ensure.IsNotNull(() => id);

            this.Logger.LogTrace($"Replacing user with id {id} with new information");

            var userResponse = await this.container.ReplaceItemAsync(updatedUser, id.ToString());

            // Check that the user has been added and is valid
            var userFromDatabase = userResponse.Resource;
            userFromDatabase = Ensure.IsNotNull(() => userFromDatabase);
            userFromDatabase.Validate();

            this.Logger.LogInformation("Replaced 1 user in the database");

            return userFromDatabase;
        }
    }
}
