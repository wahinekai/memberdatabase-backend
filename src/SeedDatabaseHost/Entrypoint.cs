// -----------------------------------------------------------------------
// <copyright file="Entrypoint.cs" company="Wahine Kai">
// Copyright (c) Wahine Kai. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace WahineKai.Backend.SeedDatabase.Host
{
    using System;
    using Microsoft.Extensions.Configuration;
    using WahineKai.Backend.Common.Contracts;
    using WahineKai.Backend.DTO.Properties;

    /// <summary>
    /// Entrypoint for SeedDatabaseHost console application
    /// </summary>
    public sealed class Entrypoint : IEntrypoint
    {
        private readonly CosmosConfiguration cosmosConfiguration;

        /// <summary>
        /// Initializes a new instance of the <see cref="Entrypoint"/> class.
        /// </summary>
        public Entrypoint()
        {
            var builder = new ConfigurationBuilder();

            // Tell the builder to look for the appsettings.json file
            builder.AddJsonFile("Properties/appsettings.json", optional: false, reloadOnChange: true);

            // Add user secrets
            builder.AddUserSecrets<Entrypoint>();

            // Build configuration
            var configuration = builder.Build();

            // Build and validate Cosmos Configuration
            this.cosmosConfiguration = CosmosConfiguration.BuildFromConfiguration(configuration);
        }

        /// <summary>
        /// Main method entrypoint
        /// </summary>
        public static void Main()
        {
            var program = new Entrypoint();
            program.Start();
        }

        /// <inheritdoc/>
        public void Start()
        {
            var seeder = new DatabaseSeeder(this.cosmosConfiguration);
            seeder.Clear();
            seeder.Seed();
        }
    }
}
