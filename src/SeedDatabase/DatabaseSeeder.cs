// -----------------------------------------------------------------------
// <copyright file="DatabaseSeeder.cs" company="Wahine Kai">
// Copyright (c) Wahine Kai. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace WahineKai.Backend.SeedDatabase
{
    using System;
    using Microsoft.Azure.Cosmos;
    using WahineKai.Backend.Common;
    using WahineKai.Backend.DTO.Properties;

    /// <summary>
    /// Project for seeding the development database with data
    /// </summary>
    public sealed class DatabaseSeeder
    {
        private readonly CosmosClient cosmosClient;

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
            cosmosConfiguration = Ensure.IsNotNull(() => cosmosConfiguration);
            cosmosConfiguration.Validate();

            this.cosmosClient = new CosmosClient(cosmosConfiguration.EndpointUrl, cosmosConfiguration.PrimaryKey);
        }

        /// <summary>
        /// Adds new data to the database.  Assumes an empty database before running.
        /// </summary>
        public void Seed()
        {
            // If database is not cleared yet, we can't seed it
            if (!this.databaseCleared)
            {
                throw new InvalidOperationException("Database is not clear");
            }

            Console.WriteLine("Seeding database");
        }

        /// <summary>
        /// Clears the database
        /// </summary>
        public void Clear()
        {
            Console.WriteLine("Clearing database");

            // Set database cleared to be true
            this.databaseCleared = true;
        }
    }
}
