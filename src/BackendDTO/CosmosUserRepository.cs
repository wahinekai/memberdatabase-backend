// -----------------------------------------------------------------------
// <copyright file="CosmosUserRepository.cs" company="Wahine Kai">
// Copyright (c) Wahine Kai. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace WahineKai.Backend.DTO
{
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
            this.container = this.CosmosClient.GetContainer(this.CosmosConfiguration.DatabaseId, User.ContainerId);
        }

        /// <inheritdoc/>
        public async Task<User> GetUserByEmailAsync(string email)
        {
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

            return user;
        }

        /// <inheritdoc/>
        public async Task<ICollection<User>> GetAllUsersAsync()
        {
            using var iterator = this.container.GetItemQueryIterator<User>();

            var users = new Collection<User>();

            while (iterator.HasMoreResults)
            {
                foreach (var user in await iterator.ReadNextAsync())
                {
                    users.Add(user);
                }
            }

            return users;
        }

        /// <inheritdoc/>
        public async Task<User> CreateUserAsync(User user)
        {
            // Input sanity checking
            user = Ensure.IsNotNull(() => user);
            user.Validate();

            // Add user to the database
            var userResponse = await this.container.CreateItemAsync(user);

            // Check that the user has been added and is valid
            var userFromDatabase = userResponse.Resource;
            userFromDatabase = Ensure.IsNotNull(() => user);
            userFromDatabase.Validate();

            return userFromDatabase;
        }
    }
}
