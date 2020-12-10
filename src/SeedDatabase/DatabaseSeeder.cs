// -----------------------------------------------------------------------
// <copyright file="DatabaseSeeder.cs" company="Wahine Kai">
// Copyright (c) Wahine Kai. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace WahineKai.Backend.SeedDatabase
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Azure.Cosmos.Linq;
    using WahineKai.Backend.Common;
    using WahineKai.Backend.DTO.Enums;
    using WahineKai.Backend.DTO.Models;
    using WahineKai.Backend.DTO.Properties;

    /// <summary>
    /// Project for seeding the development database with data
    /// </summary>
    public sealed class DatabaseSeeder
    {
        private static readonly string[] Boards = { "5'10\" custom", "8'2\" funboard" };

        private static readonly User[] UsersArray =
            {
                new User
                {
                    Admin = false,
                    FirstName = "Test",
                    LastName = "User",
                    Active = true,
                    FacebookName = "Test User",
                    PayPalName = "test-user",
                    Email = "user@user.com",
                    PhoneNumber = "1234567890",
                    StreetAddress = "1234 Test Drive",
                    City = "Orange",
                    Region = "California",
                    Country = Country.UnitedStates,
                    Occupation = "Software Testing",
                    Chapter = Chapter.OrangeCountyLosAngeles,
                    Birthdate = new DateTime(1982, 09, 05),
                    Level = Level.Intermediate,
                    Boards = Boards.ToList(),
                    PhotoUrl = string.Empty,
                    Biography = "I am a test user",
                    StartedSurfing = new DateTime(1989, 05, 01),
                    JoinedDate = new DateTime(2000, 08, 15),
                    RenewalDate = new DateTime(2021, 08, 15),
                    EnteredInFacebookChapter = EnteredStatus.Accepted,
                    EnteredInFacebookWki = EnteredStatus.Entered,
                    NeedsNewMemberBag = true,
                    WonSurfboard = true,
                    DateSurfboardWon = new DateTime(2019, 12, 25),
                },
                new User
                {
                    Admin = true,
                    FirstName = "Admin",
                    LastName = "User",
                    Active = true,
                    FacebookName = "Admin User",
                    PayPalName = "admin-a-user",
                    Email = "admin@admin.com",
                    PhoneNumber = "2345678901",
                    StreetAddress = "1234 Admin Circle",
                    City = "Vancouver",
                    Region = "British Columbia",
                    Country = Country.Canada,
                    Occupation = "Hardware Administration",
                    Chapter = Chapter.Canada,
                    Birthdate = new DateTime(1964, 02, 18),
                    Level = Level.Expert,
                    Boards = Boards.ToList(),
                    PhotoUrl = string.Empty,
                    Biography = "I am an administrator",
                    StartedSurfing = new DateTime(1977, 11, 01),
                    JoinedDate = new DateTime(1994, 01, 22),
                    RenewalDate = new DateTime(2021, 01, 22),
                    EnteredInFacebookChapter = EnteredStatus.Accepted,
                    EnteredInFacebookWki = EnteredStatus.Accepted,
                    Position = Position.ChapterDirector,
                    DateStartedPosition = new DateTime(2016, 01, 01),
                },
            };

        private readonly CosmosConfiguration cosmosConfiguration;

        private readonly Microsoft.Azure.Cosmos.CosmosClient cosmosClient;

        private bool databaseCleared;

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseSeeder"/> class.
        /// </summary>
        /// <param name="cosmosConfiguration">Configuration to access Cosmos DB</param>
        /// <param name="databaseCleared">Whether the database is currently clear.  Defaults to false.</param>
        public DatabaseSeeder(CosmosConfiguration? cosmosConfiguration, bool databaseCleared = false)
        {
            this.databaseCleared = databaseCleared;

            // Set and validate cosmos configuration
            this.cosmosConfiguration = Ensure.IsNotNull(() => cosmosConfiguration);
            this.cosmosConfiguration.Validate();

            this.cosmosClient = new Microsoft.Azure.Cosmos.CosmosClient(this.cosmosConfiguration.EndpointUrl, this.cosmosConfiguration.PrimaryKey);
        }

        /// <summary>
        /// Gets public collection of users to be seeded into the database
        /// </summary>
        public static ICollection<User> Users { get => UsersArray.ToList(); }

        /// <summary>
        /// Adds new data to the database.  Assumes an empty database before running.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task Seed()
        {
            // If database is not cleared yet, we can't seed it
            if (!this.databaseCleared)
            {
                throw new InvalidOperationException("Database is not clear");
            }

            // Create a database
            var databaseResponse = await this.cosmosClient.CreateDatabaseIfNotExistsAsync(this.cosmosConfiguration.DatabaseId);
            var database = Ensure.IsNotNull(() => databaseResponse).Database;

            var containerProperties = new Microsoft.Azure.Cosmos.ContainerProperties(User.ContainerId, User.PartitionKey);

            // Create a Users container
            var containerResponse = await database.CreateContainerIfNotExistsAsync(containerProperties);
            var container = Ensure.IsNotNull(() => containerResponse).Container;

            // Add users to the container
            foreach (var user in Users)
            {
                user.Validate();
                var createUserResponse = await container.CreateItemAsync(user);
            }
        }

        /// <summary>
        /// Clears the database
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task Clear()
        {
            // Clear users container
            var container = this.cosmosClient.GetContainer(this.cosmosConfiguration.DatabaseId, User.ContainerId);

            // Get all users
            using var iterator = container.GetItemLinqQueryable<User>()
                .ToFeedIterator();

            while (iterator.HasMoreResults)
            {
                foreach (var user in await iterator.ReadNextAsync())
                {
                    // Delete each user
                    await container.DeleteItemAsync<User>(user.Id.ToString(), new Microsoft.Azure.Cosmos.PartitionKey(user.Email));
                }
            }

            // Set database cleared to be true
            this.databaseCleared = true;
        }
    }
}
